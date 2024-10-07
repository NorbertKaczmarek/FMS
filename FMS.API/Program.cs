using FluentValidation.AspNetCore;
using FluentValidation;
using FMS.API;
using FMS.API.Entities;
using FMS.API.Middleware;
using FMS.API.Models.Validators;
using FMS.API.Models;
using FMS.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using NLog;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policybuilder =>
        policybuilder
            .WithOrigins(builder.Configuration["AllowedOrigins"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("location")
    );
});

// Authentication, JWT
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
        ValidateIssuer = true,
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidateAudience = true,
        ValidAudience = authenticationSettings.JwtIssuer,
    };
});

// Authorization
builder.Services.AddAuthorization();

// Automapper
builder.Services.AddAutoMapper(typeof(Program));

// Enums in SQL
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Password hasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddScoped<IValidator<FlightCreateDto>, FlightCreateDtoValidator>();
builder.Services.AddScoped<IValidator<UserLoginDto>, UserLoginDtoValidator>();
builder.Services.AddScoped<IValidator<UserSignupDto>, UserSignupDtoValidator>();

// Services
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Middleware
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<FMSDbContext>
    (options => options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Error Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

// migrate database
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<FMSDbContext>();
seeder.UpdateDatabase();

app.UseCors();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

// JWT Authorization and Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { } // used by Unit Testing
