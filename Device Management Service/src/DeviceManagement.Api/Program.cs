using System.Text.Json;
using System.Text.Json.Serialization;
using DeviceManagement.Api.Endpoints;
using DeviceManagement.Application.Interfaces;
using DeviceManagement.Application.Services;
using DeviceManagement.Domain.Interfaces;
using DeviceManagement.Infrastructure.Configuration;
using DeviceManagement.Infrastructure.Mqtt;
using DeviceManagement.Infrastructure.Persistence;
using DeviceManagement.Infrastructure.Persistence.Repositories;
using Microsoft of.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

// --- 1. Bootstrap: Configure Serilog for early-stage logging ---
// It's configured to read from appsettings.json, allowing flexible configuration for different environments.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("DeviceManagement.Api starting up...");

try
{
    // --- 2. Builder Setup: Initialize the WebApplicationBuilder ---
    var builder = WebApplication.CreateBuilder(args);

    // Replace the default logger with Serilog
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter()));


    // --- 3. Service Registration (Dependency Injection) ---

    #region Infrastructure Services
    // Configure strongly-typed MQTT settings from appsettings.json
    builder.Services.Configure<MqttSettings>(builder.Configuration.GetSection("Mqtt"));

    // Register Entity Framework DbContext for PostgreSQL
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<DeviceManagementDbContext>(options =>
        options.UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention());

    // Register Repositories with a scoped lifetime
    builder.Services.AddScoped<IClientRepository, ClientRepository>();
    builder.Services.AddScoped<IProvisioningTokenRepository, ProvisioningTokenRepository>();

    // Register MQTT infrastructure
    // MqttCommandPublisher is a singleton as it depends on the singleton MqttClientService
    builder.Services.AddSingleton<IMqttCommandPublisher, MqttCommandPublisher>();
    // MqttClientService runs as a background service for the application's lifetime
    builder.Services.AddHostedService<MqttClientService>();
    #endregion

    #region Application Services
    // Register application services with a scoped lifetime
    // Note: Assuming interfaces for services are defined for better decoupling, though not explicitly in file list
    // If not, this would be builder.Services.AddScoped<DeviceService>();
    builder.Services.AddScoped<DeviceService>();
    builder.Services.AddScoped<ProvisioningService>();
    #endregion
    
    #region API & Security Services
    // Configure services for API controllers and endpoints
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
    
    builder.Services.AddEndpointsApiExplorer();
    
    // Configure Swagger/OpenAPI for API documentation and testing
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Device Management Service API", Version = "v1" });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    // Configure JWT Bearer Authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            // Authority is the URL of the Keycloak realm
            options.Authority = builder.Configuration["Jwt:Authority"];
            options.Audience = builder.Configuration["Jwt:Audience"];
            options.RequireHttpsMetadata = builder.Environment.IsProduction();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        });

    // Configure Authorization policies
    builder.Services.AddAuthorization(options =>
    {
        // Default policy requires an authenticated user for all endpoints unless otherwise specified.
        options.FallbackPolicy = options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });
    
    // Add health checks for container orchestration
    builder.Services.AddHealthChecks()
        .AddNpgSql(connectionString!);

    // Add CORS policy
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    #endregion

    // --- 4. Build the WebApplication ---
    var app = builder.Build();

    // --- 5. Configure the HTTP Request Pipeline (Middleware) ---
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeviceManagement.Api v1"));
    }
    else
    {
        // Add a production-ready exception handler
        app.UseExceptionHandler("/error");
        // Enforce HTTPS in production
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    
    // Use Serilog for rich request logging
    app.UseSerilogRequestLogging();
    
    app.UseRouting();

    app.UseCors("AllowAll");
    
    app.UseAuthentication();
    app.UseAuthorization();

    // --- 6. Map Endpoints ---
    app.MapClientEndpoints();
    app.MapProvisioningEndpoints();
    app.MapHealthChecks("/healthz");
    
    // A simple error endpoint for production
    app.MapGet("/error", () => Results.Problem("An unexpected error occurred."))
        .ExcludeFromDescription();

    // --- 7. Run Application ---
    app.Run();

}
catch (Exception ex)
{
    // Catch startup errors
    Log.Fatal(ex, "DeviceManagement.Api failed to start.");
}
finally
{
    // Ensure logs are flushed on exit
    Log.CloseAndFlush();
}