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
using GovServices.Server.Services.RegistryApis;
using GovServices.Server.Services.Numbering;
using GovServices.Server.Middleware;
using GovServices.Server.BackgroundServices;
using GovServices.Server.Entities;
using MudBlazor.Services;

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
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseNpgsql(defaultConn, npgsql => npgsql.UseNetTopologySuite())
);
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());

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
        var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ??
                      new[] { "http://localhost:5093", "https://localhost:7197" };
        policy.WithOrigins(origins)
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
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOutgoingService, OutgoingService>();
builder.Services.AddScoped<IGeoService, GeoService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<IServiceTemplateService, ServiceTemplateService>();
builder.Services.AddScoped<INumberTemplateService, NumberTemplateService>();
builder.Services.AddScoped<INumberGenerator, NumberGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOcrService, OcrService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDocumentStorageService, DocumentStorageService>();
builder.Services.AddScoped<IDocumentClassifierService, DocumentClassifierService>();
builder.Services.AddScoped<IDictionaryService, DictionaryService>();
builder.Services.AddSingleton<IDictionaryCacheService, DictionaryCacheService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IPageAccessService, PageAccessService>();
builder.Services.AddSingleton<MinioService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();

// Регистрация IEmailService (и реализация EmailService)
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<ISedIntegrationService, SedIntegrationService>();
builder.Services.AddScoped<IRosreestrIntegrationService, RosreestrIntegrationService>();
builder.Services.AddScoped<IZagsIntegrationService, ZagsIntegrationService>();
builder.Services.AddScoped<IEcpService, EcpService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IExtractRepository, ExtractRepository>();
builder.Services.AddScoped<IExtractChecker, ExtractChecker>();
builder.Services.AddScoped<IRegistryApi, RegistryApi>();
builder.Services.AddKeyedScoped<IRegistryApi, ZagcApi>(RegistrySource.Zags);
builder.Services.AddKeyedScoped<IRegistryApi, RosreestrApi>(RegistrySource.Rosreestr);


// Background services
builder.Services.AddHostedService<ExampleBackgroundService>();
builder.Services.AddHostedService<PasswordReminderService>();
builder.Services.AddHostedService<StatusNotificationService>();
builder.Services.AddHostedService<ExtractMonitoringService>();
builder.Services.AddHostedService<DeadlineMonitoringService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await DatabaseInitializer.SeedAsync(scope.ServiceProvider);
    await DataSeeder.SeedAsync(scope.ServiceProvider);
    var cache = scope.ServiceProvider.GetRequiredService<IDictionaryCacheService>();
    await cache.ReloadAsync();
}

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

// app.UseHttpsRedirection(); // Disabled for local development

app.UseRouting();

app.UseCors("SmartCors");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AuditMiddleware>();

app.MapControllers();

app.Run();

