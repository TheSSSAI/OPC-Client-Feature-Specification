using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DataIngestionService.Application.Configuration;
using DataIngestionService.Application.Interfaces;
using DataIngestionService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DataIngestionService.Application.BackgroundServices
{
    /// <summary>
    /// A long-running background service that periodically flushes the data point buffer
    /// to the persistent data store. This acts as the "consumer" in the Producer-Consumer pattern.
    /// </summary>
    public class BatchFlushingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DataPointBatchingService _batchingService;
        private readonly ILogger<BatchFlushingService> _logger;
        private readonly IngestionOptions _ingestionOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchFlushingService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to create DI scopes.</param>
        /// <param name="batchingService">The singleton batching service containing the buffer.</param>
        /// <param name="logger">The logger for this service.</param>
        /// <param name="ingestionOptions">The ingestion configuration options.</param>
        public BatchFlushingService(
            IServiceProvider serviceProvider,
            DataPointBatchingService batchingService,
            ILogger<BatchFlushingService> logger,
            IOptions<IngestionOptions> ingestionOptions)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _batchingService = batchingService ?? throw new ArgumentNullException(nameof(batchingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ingestionOptions = ingestionOptions?.Value ?? throw new ArgumentNullException(nameof(ingestionOptions));
        }

        /// <summary>
        /// Executes the background task to periodically flush data.
        /// </summary>
        /// <param name="stoppingToken">A token that is triggered on application shutdown.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Batch Flushing Service is starting.");
            
            // Using PeriodicTimer is the modern and recommended approach for timed loops in BackgroundServices.
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_ingestionOptions.FlushIntervalSeconds));

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    await FlushBatchAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // This is expected when the application is shutting down.
                    _logger.LogInformation("Batch Flushing Service is stopping due to cancellation request.");
                    break;
                }
                catch (Exception ex)
                {
                    // This catch block is critical to prevent the entire BackgroundService from crashing
                    // due to an unhandled exception in the loop (e.g., a catastrophic database failure).
                    _logger.LogCritical(ex, "An unhandled exception occurred in the batch flushing loop. The service will continue, but data may have been lost.");
                }
            }

            _logger.LogInformation("Batch Flushing Service has stopped.");
        }

        private async Task FlushBatchAsync(CancellationToken cancellationToken)
        {
            var batch = _batchingService.DrainBatch();

            if (batch.Count == 0)
            {
                return;
            }
            
            var stopwatch = Stopwatch.StartNew();
            _logger.LogDebug("Drained a batch of {BatchSize} data points. Starting persistence.", batch.Count);

            // We must create a new scope for each iteration because IDataPointWriter is a scoped service,
            // while this BackgroundService is a singleton. This is the correct pattern for using scoped
            // services inside a long-running singleton.
            await using var scope = _serviceProvider.CreateScope();
            var dataPointWriter = scope.ServiceProvider.GetRequiredService<IDataPointWriter>();

            try
            {
                await dataPointWriter.WriteBatchAsync(batch, cancellationToken);
                stopwatch.Stop();

                _logger.LogInformation(
                    "Successfully persisted a batch of {BatchSize} data points in {ElapsedMilliseconds}ms. Current buffer size: {CurrentBufferSize}",
                    batch.Count,
                    stopwatch.ElapsedMilliseconds,
                    _batchingService.CurrentBufferSize);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                // If the WriteBatchAsync fails after all its resilience policies (retries/circuit breaker)
                // have been exhausted, we log the failure and discard the batch. Re-queuing a failing
                // batch could lead to a "poison message" scenario, blocking the entire pipeline.
                _logger.LogError(ex,
                    "Failed to persist a batch of {BatchSize} data points after {ElapsedMilliseconds}ms. The batch will be discarded.",
                    batch.Count,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}