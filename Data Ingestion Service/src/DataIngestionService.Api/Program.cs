using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using DataIngestionService.Api.Services;
using DataIngestionService.Application.BackgroundServices;
using DataIngestionService.Application.Configuration;
using DataIngestionService.Application.Interfaces;
using DataIngestionService.Application.Services;
using DataIngestionService.Infrastructure.Persistence;
using DataIngestionService.Infrastructure.Persistence.Resilience;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Polly;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

// Configure Serilog for structured, asynchronous logging to the console.
// This is the initial "bootstrap" logger.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.Console(new CompactJsonFormatter()))
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // --- 1. Logging Configuration ---
    // Replace the bootstrap logger with the fully configured logger.
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // --- 2. Configuration ---
    // Bind strongly-typed options from appsettings.json.
    // Use data annotations for validation at startup.
    builder.Services.AddOptions<IngestionOptions>()
        .Bind(builder.Configuration.GetSection(IngestionOptions.SectionName))
        .ValidateDataAnnotations()
        .ValidateOnStart();
    
    builder.Services.AddOptions<DatabaseResilienceOptions>()
        .Bind(builder.Configuration.GetSection(DatabaseResilienceOptions.SectionName))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    // --- 3. Service Registration (Dependency Injection) ---
    // Infrastructure Services
    builder.Services.AddNpgsqlDataSource(
        builder.Configuration.GetConnectionString("TimescaleDb")!,
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());

    builder.Services.AddSingleton<IAsyncPolicy>(sp =>
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<DatabaseResilienceOptions>>();
        return DatabaseResiliencePolicy.CreatePolicy(logger, options);
    });

    builder.Services.AddScoped<IDataPointWriter, TimescaleDataPointWriter>();

    // Application Services
    builder.Services.AddSingleton<DataPointBatchingService>();
    builder.Services.AddHostedService<BatchFlushingService>();

    // API Layer Services
    builder.Services.AddGrpc(options =>
    {
        // Add interceptors, etc. here if needed in the future.
        options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    });

    // --- 4. Observability ---
    var serviceName = builder.Configuration["OTEL_SERVICE_NAME"] ?? "DataIngestionService";
    var resourceBuilder = ResourceBuilder.CreateDefault().AddService(serviceName);

    builder.Services.AddOpenTelemetry()
        .WithMetrics(metrics => metrics
            .SetResourceBuilder(resourceBuilder)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddProcessInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter())
        .WithTracing(tracing => tracing
            .SetResourceBuilder(resourceBuilder)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddNpgsql()
            .AddOtlpExporter()); // Assumes OTLP endpoint is configured via environment variables

    // --- 5. Security (mTLS Authentication & Authorization) ---
    // Configure Kestrel to require a client certificate for all connections.
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ConfigureHttpsDefaults(httpsOptions =>
        {
            httpsOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
        });
    });

    // Add certificate authentication handler.
    builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
        .AddCertificate(options =>
        {
            // Allow self-signed certs for development/testing if configured.
            options.AllowedCertificateTypes = builder.Environment.IsDevelopment()
                ? CertificateTypes.All
                : CertificateTypes.Chained;

            options.Events = new CertificateAuthenticationEvents
            {
                OnCertificateValidated = context =>
                {
                    // This event is crucial for production readiness.
                    // Here, you would validate the certificate against a trusted CA or a list of thumbprints.
                    // For this implementation, we trust the certificate and extract claims from it.

                    var claims = new List<Claim>();
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

                    // Extract the tenantId from the certificate's subject common name (CN).
                    // This is a critical step for fulfilling REQ-1-025 (Tenant Isolation).
                    var commonName = context.ClientCertificate.Subject
                        .Split(',')
                        .FirstOrDefault(s => s.Trim().StartsWith("CN="))?
                        .Split('=')[1].Trim();

                    if (!string.IsNullOrEmpty(commonName) && Guid.TryParse(commonName, out var tenantId))
                    {
                        claims.Add(new Claim("tenantId", tenantId.ToString(), ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        logger.LogInformation("Successfully extracted tenantId '{TenantId}' from client certificate.", tenantId);
                    }
                    else
                    {
                        logger.LogWarning("Could not extract a valid tenantId (GUID) from client certificate subject common name: '{Subject}'.", context.ClientCertificate.Subject);
                        // Optionally fail validation if tenantId is mandatory
                        // context.Fail("Client certificate does not contain a valid tenantId in its Common Name.");
                        // return Task.CompletedTask;
                    }

                    // You could add more claims from certificate properties if needed.
                    // claims.Add(new Claim(ClaimTypes.SerialNumber, context.ClientCertificate.SerialNumber, ClaimValueTypes.String, context.Options.ClaimsIssuer));

                    context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name));
                    context.Success();

                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(context.Exception, "Client certificate authentication failed.");
                    return Task.CompletedTask;
                }
            };
        });

    // Add authorization policy that requires the custom `tenantId` claim.
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("mTLS", policy =>
        {
            policy.AddAuthenticationSchemes(CertificateAuthenticationDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("tenantId"); // Ensure the tenantId was successfully extracted.
        });
    });

    var app = builder.Build();

    // --- 6. Middleware Pipeline Configuration ---
    app.UseSerilogRequestLogging();

    app.UseRouting();

    // Authentication and Authorization middleware must be placed correctly.
    app.UseAuthentication();
    app.UseAuthorization();

    // Map Prometheus endpoint for metrics scraping.
    app.MapPrometheusScrapingEndpoint();

    // Map a simple liveness health check endpoint.
    app.MapGet("/healthz", () => Results.Ok("Healthy")).AllowAnonymous();

    // Map the gRPC service and apply the mTLS authorization policy.
    app.MapGrpcService<IngestionGrpcService>().RequireAuthorization("mTLS");

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Data Ingestion Service terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}