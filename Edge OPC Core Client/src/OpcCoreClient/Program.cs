using System.Security.Cryptography.X509Certificates;
using System.Threading.Channels;
using Grpc.Net.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Polly;
using Polly.Extensions.Http;
using System.Net.Security;
using System.Reflection;
using System.Edge.OpcCoreClient.Services;
using System.Edge.OpcCoreClient.Persistence;
using System.Edge.OpcCoreClient.Configuration;
using System.Edge.OpcCoreClient.Models;
using System.Edge.OpcCoreClient.Communication;
using System.Edge.OpcCoreClient.HostedServices;
using Ingestion; // Assuming this is the namespace for the gRPC generated client

public class Program
{
    public static async Task Main(string[] args)
    {
        // Configure Serilog for structured logging
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            var host = CreateHostBuilder(args).Build();
            Log.Information("OpcCoreClient Host built. Starting application...");
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "OpcCoreClient application terminated unexpectedly during startup.");
        }
        finally
        {
            Log.Information("OpcCoreClient application shutting down.");
            await Log.CloseAndFlushAsync();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", Assembly.GetExecutingAssembly().GetName().Name))
            .ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;

                // --- Strongly-typed Configuration Registration (IOptions<T>) ---
                services.Configure<OpcOptions>(configuration.GetSection("Opc"));
                services.Configure<MqttOptions>(configuration.GetSection("Mqtt"));
                services.Configure<GrpcOptions>(configuration.GetSection("Grpc"));
                services.Configure<BufferOptions>(configuration.GetSection("Buffer"));
                services.Configure<ClientInfoOptions>(configuration.GetSection("ClientInfo"));
                
                // --- Core Service Registration (Singletons for state/resource management) ---

                // Producer-Consumer channel for decoupling OPC polling from cloud dispatching
                services.AddSingleton(Channel.CreateUnbounded<DataPoint>(new UnboundedChannelOptions { SingleReader = true }));
                services.AddSingleton(sp => sp.GetRequiredService<Channel<DataPoint>>().Reader);
                services.AddSingleton(sp => sp.GetRequiredService<Channel<DataPoint>>().Writer);
                
                // State machine for connectivity management
                services.AddSingleton<IConnectivityStateManager, ConnectivityStateManager>();

                // On-disk persistent buffer for store-and-forward
                services.AddSingleton<IDataBuffer, FileBasedDataBuffer>();
                
                // --- Communication Service Registration ---

                // MQTT Client and Command Handler
                services.AddSingleton<MqttCommandHandler>();
                services.AddSingleton<MqttClientWrapper>();
                
                // OPC UA Adapter
                services.AddSingleton<OpcUaAdapter>(); // Assuming we are starting with UA, can be extended with a factory for DA/XML-DA

                // gRPC Data Sender with mTLS and Polly resilience policies
                services.AddSingleton<GrpcDataSender>();

                var grpcRetryPolicy = HttpPolicyExtensions
                    .Handle<Exception>() // Catches RpcException, HttpRequestException etc.
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (outcome, timespan, retryAttempt, context) =>
                        {
                            var logger = context.GetLogger();
                            logger?.LogWarning(outcome.Exception, "Delaying for {delay}ms, then making retry {retry}.", timespan.TotalMilliseconds, retryAttempt);
                        });

                services.AddGrpcClient<IngestionService.IngestionServiceClient>(o =>
                {
                    o.Address = new Uri(configuration.GetSection("Grpc:Endpoint").Value ?? 
                                        throw new InvalidOperationException("gRPC Endpoint is not configured."));
                })
                .ConfigureChannel(o =>
                {
                    o.Credentials = GrpcChannelCredentials.SecureSsl;
                })
                .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var grpcOptions = configuration.GetSection("Grpc").Get<GrpcOptions>()!;
                    var handler = new SocketsHttpHandler
                    {
                        SslOptions = new SslClientAuthenticationOptions()
                    };
                    
                    Log.Information("Configuring gRPC mTLS. Loading client certificate from {CertPath}", grpcOptions.ClientCertPath);
                    var clientCert = new X509Certificate2(grpcOptions.ClientCertPath, grpcOptions.ClientCertPassword);
                    handler.SslOptions.ClientCertificates = new X509CertificateCollection { clientCert };
                    
                    if (!string.IsNullOrEmpty(grpcOptions.CaCertPath))
                    {
                        Log.Information("Configuring gRPC mTLS. Loading custom CA from {CaPath}", grpcOptions.CaCertPath);
                        var caCert = new X509Certificate2(grpcOptions.CaCertPath);
                        handler.SslOptions.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                        {
                            if (sslPolicyErrors == SslPolicyErrors.None) return true;
                            
                            using var customChain = new X509Chain();
                            customChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                            customChain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                            customChain.ChainPolicy.ExtraStore.Add(caCert);
                            
                            var serverCert = new X509Certificate2(certificate.Export(X509ContentType.Cert));
                            var isValid = customChain.Build(serverCert);
                            
                            if (!isValid)
                            {
                                Log.Error("Custom CA validation failed for gRPC server certificate.");
                                foreach (var status in customChain.ChainStatus)
                                {
                                    Log.Warning("Chain status: {Status}, {Information}", status.Status, status.StatusInformation);
                                }
                            }
                            return isValid;
                        };
                    }

                    return handler;
                })
                .AddPolicyHandler(grpcRetryPolicy);


                // --- Edge AI and Provisioning Services ---
                services.AddSingleton<OnnxInferenceService>();
                services.AddSingleton<ProvisioningService>();

                // --- Hosted Service Registration (Main application loops) ---
                services.AddHostedService<OpcPollerHostedService>();
                services.AddHostedService<CloudDispatcherHostedService>();
                services.AddHostedService(provider => provider.GetRequiredService<MqttClientWrapper>()); // MqttClientWrapper is also a hosted service

                // --- Health Checks Registration ---
                services.AddHealthChecks()
                    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "liveness" })
                    .AddCheck<OpcConnectionHealthCheck>("opc_connection", tags: new[] { "readiness" })
                    .AddCheck<CloudConnectionHealthCheck>("cloud_connection", tags: new[] { "readiness" });
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                // Expose health check endpoints as required by REQ-1-091
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHealthChecks("/healthz", new() { Predicate = check => check.Tags.Contains("liveness") });
                        endpoints.MapHealthChecks("/readyz", new() { Predicate = check => check.Tags.Contains("readiness") });
                    });
                });
            });
}

// Example Health Check for OPC Connection
public class OpcConnectionHealthCheck : IHealthCheck
{
    // In a real implementation, this would be injected and would check the actual connection state
    // For now, it's a placeholder to demonstrate the pattern.
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // bool isConnected = _opcAdapter.IsConnected;
        bool isConnected = true; // Placeholder
        if (isConnected)
        {
            return Task.FromResult(HealthCheckResult.Healthy("OPC server connection is active."));
        }
        return Task.FromResult(HealthCheckResult.Unhealthy("OPC server connection is lost."));
    }
}

// Example Health Check for Cloud Connection
public class CloudConnectionHealthCheck : IHealthCheck
{
    private readonly IConnectivityStateManager _stateManager;
    public CloudConnectionHealthCheck(IConnectivityStateManager stateManager)
    {
        _stateManager = stateManager;
    }
    
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var currentState = _stateManager.CurrentState;
        if (currentState == ConnectivityState.Online)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Cloud connection is Online."));
        }
        if (currentState == ConnectivityState.Recovery)
        {
            return Task.FromResult(HealthCheckResult.Degraded("Cloud connection is in Recovery mode."));
        }
        return Task.FromResult(HealthCheckResult.Unhealthy("Cloud connection is Offline."));
    }
}