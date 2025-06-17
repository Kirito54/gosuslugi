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
var defaultConn = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(defaultConn))
{
    throw new InvalidOperationException("ConnectionString 'DefaultConnection' is not configured in appsettings.json.");
}
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(defaultConn, npgsql => npgsql.UseNetTopologySuite())
);

// JWT configuration
var jwtKey = builder.Configuration["JwtSettings:SecretKey"];
if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 16)
    throw new Exception("JWT ключ не задан или слишком короткий");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("SmartCors", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
                origin.StartsWith("http://localhost") ||
                origin.StartsWith("https://localhost"))
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
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

using (var scope = app.Services.CreateScope())
{
    await DataSeeder.SeedAsync(scope.ServiceProvider);
}

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Disabled for local development

app.UseRouting();

app.UseCors("SmartCors");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AuditMiddleware>();

app.MapControllers();

app.Run();

