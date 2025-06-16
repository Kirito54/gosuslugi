using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using GovServices.Server.Data;
using GovServices.Server.Interfaces;
using GovServices.Server.Services;
using GovServices.Server.Services.Templates;
using GovServices.Server.Services.Integrations;
using GovServices.Server.Middleware;
using GovServices.Server.BackgroundServices;
using GovServices.Server.Entities;

var builder = WebApplication.CreateBuilder(args);

// --- Инфраструктурные сервисы ---
// Data Protection для ProtectedBrowserStorage и других компонентов
builder.Services.AddDataProtection();

// HttpClientFactory для интеграций
builder.Services.AddHttpClient();

// ASP.NET Identity (UserManager, SignInManager, RoleManager)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Lockout.MaxFailedAccessAttempts = 5;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// DinkToPdf конвертер для генерации PDF из шаблонов
builder.Services.AddSingleton(typeof(DinkToPdf.Contracts.IConverter),
    new DinkToPdf.SynchronizedConverter(new DinkToPdf.PdfTools()));

// --- Регистрация бизнес-сервисов ---

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// --- Настройка DbContext с подключением к PostgreSQL ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(conn))
        throw new InvalidOperationException("ConnectionString 'DefaultConnection' is not configured.");
    options.UseNpgsql(conn, npgsql =>
    {
        npgsql.UseNetTopologySuite();
    });
});

// Identity + JWT configuration
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server.Api", Version = "v1" });
});

// DI for services
builder.Services.AddScoped<IExampleService, ExampleService>();

// Регистрация сервисов бизнес-логики
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOutgoingService, OutgoingService>();
builder.Services.AddScoped<IGeoService, GeoService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOcrService, OcrService>();

// Регистрация IEmailService (и реализация EmailService)
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<ISedIntegrationService, SedIntegrationService>();
builder.Services.AddScoped<IRosreestrIntegrationService, RosreestrIntegrationService>();
builder.Services.AddScoped<IEcpService, EcpService>();
builder.Services.AddScoped<IAuditService, AuditService>();


// Background services
builder.Services.AddHostedService<ExampleBackgroundService>();
builder.Services.AddHostedService<PasswordReminderService>();
builder.Services.AddHostedService<StatusNotificationService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AuditMiddleware>();

app.MapControllers();

app.Run();

