using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace System.Edge.OpcCoreClient.Services
{
    #region Placeholder DTOs (Would be in REPO-LIB-SHARED)

    /// <summary>
    /// Represents a generic command received from the MQTT broker.
    /// </summary>
    public class MqttCommand
    {
        public string CommandType { get; set; }
        public JsonElement Payload { get; set; }
    }

    /// <summary>
    /// Payload for the SOFTWARE_UPDATE command.
    /// </summary>
    public class SoftwareUpdatePayload
    {
        public string ImageTag { get; set; }
    }

    /// <summary>
    /// Payload for the CONFIGURATION_UPDATE command.
    /// </summary>
    public class ConfigurationUpdatePayload
    {
        public string ConfigurationJson { get; set; }
    }

    #endregion

    /// <summary>
    /// Defines the contract for a service that processes commands received from MQTT.
    /// </summary>
    public interface IMqttCommandHandler
    {
        /// <summary>
        /// Handles a deserialized command from the MQTT broker.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <returns>A task representing the asynchronous handling operation.</returns>
        Task HandleCommandAsync(MqttCommand command);
    }

    /// <summary>
    /// Processes incoming command messages received from the MQTT broker, such as configuration or software update requests.
    /// </summary>
    public class MqttCommandHandler : IMqttCommandHandler
    {
        private readonly ILogger<MqttCommandHandler> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IConfiguration _configuration;
        
        // Simple regex to validate image tags. A production system might have stricter rules.
        private static readonly Regex ValidImageTagRegex = new Regex(@"^[a-zA-Z0-9][a-zA-Z0-9_.-]{0,127}$", RegexOptions.Compiled);

        public MqttCommandHandler(ILogger<MqttCommandHandler> logger, IHostApplicationLifetime appLifetime, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        public async Task HandleCommandAsync(MqttCommand command)
        {
            if (command == null)
            {
                _logger.LogWarning("Received a null MQTT command.");
                return;
            }

            _logger.LogInformation("Handling MQTT command of type: {commandType}", command.CommandType);

            try
            {
                switch (command.CommandType?.ToUpperInvariant())
                {
                    case "SOFTWARE_UPDATE":
                        var updatePayload = command.Payload.Deserialize<SoftwareUpdatePayload>();
                        await HandleSoftwareUpdateAsync(updatePayload);
                        break;

                    case "CONFIGURATION_UPDATE":
                        var configPayload = command.Payload.Deserialize<ConfigurationUpdatePayload>();
                        await HandleConfigurationUpdateAsync(configPayload);
                        break;

                    default:
                        _logger.LogWarning("Received unhandled MQTT command type: {commandType}", command.CommandType);
                        break;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize payload for command type {commandType}", command.CommandType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while handling command type {commandType}", command.CommandType);
            }
        }
        
        private Task HandleSoftwareUpdateAsync(SoftwareUpdatePayload payload)
        {
            if (payload == null || string.IsNullOrWhiteSpace(payload.ImageTag))
            {
                _logger.LogError("Received SOFTWARE_UPDATE command with invalid or missing payload.");
                return Task.CompletedTask;
            }

            if (!ValidImageTagRegex.IsMatch(payload.ImageTag))
            {
                _logger.LogError("Received SOFTWARE_UPDATE command with an invalid image tag format: {imageTag}. Aborting update.", payload.ImageTag);
                return Task.CompletedTask;
            }

            _logger.LogInformation("Initiating software update to version: {imageTag}", payload.ImageTag);

            try
            {
                // This script is expected to be in the container's path.
                // It will be responsible for pulling the new image and restarting the container.
                // The current application process will be terminated by this script.
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash", // Assuming Linux container; use "cmd.exe" or "powershell.exe" for Windows
                    Arguments = $"-c \"/app/update.sh {payload.ImageTag}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                // On Windows, the command would be different:
                // FileName = "cmd.exe", Arguments = $"/C update.cmd {payload.ImageTag}"

                // We start the process but do not wait for it to exit,
                // as it is responsible for terminating this very process.
                using var process = Process.Start(processStartInfo);

                if (process == null)
                {
                    _logger.LogError("Failed to start the update process.");
                }
                else
                {
                    _logger.LogInformation("Update process started with PID {pid}. The application will now shut down as part of the update.", process.Id);
                    // We don't need to explicitly stop the application; the script will handle it.
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to initiate the software update process. A critical error occurred.");
            }
            
            return Task.CompletedTask;
        }

        private async Task HandleConfigurationUpdateAsync(ConfigurationUpdatePayload payload)
        {
            if (payload == null || string.IsNullOrWhiteSpace(payload.ConfigurationJson))
            {
                 _logger.LogError("Received CONFIGURATION_UPDATE command with invalid or missing payload.");
                 return;
            }

            _logger.LogInformation("Applying new configuration received via MQTT.");

            try
            {
                // Validate the JSON before writing
                using (JsonDocument.Parse(payload.ConfigurationJson)) { }

                var configFilePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                var tempFilePath = configFilePath + ".tmp";

                await File.WriteAllTextAsync(tempFilePath, payload.ConfigurationJson);

                // Perform an atomic replace to avoid corruption
                File.Move(tempFilePath, configFilePath, overwrite: true);

                _logger.LogInformation("New configuration file written successfully to {path}. Triggering configuration reload.", configFilePath);
                
                // This is a complex operation in a running app. The simplest way in .NET Generic Host
                // is to trigger a graceful shutdown, and let the container orchestrator restart the app.
                // This ensures all services get the new configuration from scratch.
                // A more advanced implementation might use IOptionsMonitor to reload, but that can
                // be complex if services cache configuration at startup.
                _logger.LogWarning("Configuration updated. Requesting application shutdown to apply new settings.");
                _appLifetime.StopApplication();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Received configuration is not valid JSON. Update aborted.");
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "Failed to write new configuration file. Update aborted.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during configuration update. Update aborted.");
            }
        }
    }
}