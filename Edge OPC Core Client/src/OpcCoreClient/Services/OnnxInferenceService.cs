using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Edge.OpcCoreClient.Models;

namespace System.Edge.OpcCoreClient.Services
{
    /// <summary>
    /// Service for running real-time AI/ML model inference using the ONNX Runtime.
    /// Fulfills requirements REQ-1-049 and REQ-1-056.
    /// </summary>
    public class OnnxInferenceService : IDisposable
    {
        private readonly ILogger<OnnxInferenceService> _logger;
        private InferenceSession _session;
        private IReadOnlyList<string> _inputNames;
        private IReadOnlyList<string> _outputNames;
        private bool _isInitialized = false;

        public OnnxInferenceService(ILogger<OnnxInferenceService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets a value indicating whether the inference service is initialized with a model.
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// Initializes the service by loading an ONNX model from the specified file path.
        /// </summary>
        /// <param name="modelPath">The path to the .onnx model file.</param>
        public Task InitializeAsync(string modelPath)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(modelPath) || !File.Exists(modelPath))
                {
                    _logger.LogError("ONNX model path is invalid or file does not exist: {ModelPath}", modelPath);
                    _isInitialized = false;
                    return;
                }

                try
                {
                    _logger.LogInformation("Initializing ONNX Inference Service with model at {ModelPath}", modelPath);
                    
                    // Dispose previous session if re-initializing
                    _session?.Dispose();

                    // For NVIDIA Jetson support, SessionOptions can be configured to use CUDA or TensorRT providers.
                    // This example uses the default CPU provider for cross-platform compatibility.
                    // For GPU: SessionOptions.MakeSessionOptionWithCudaProvider(0);
                    var sessionOptions = new SessionOptions(); 
                    
                    _session = new InferenceSession(modelPath, sessionOptions);
                    _inputNames = _session.InputNames;
                    _outputNames = _session.OutputNames;

                    _logger.LogInformation("ONNX model loaded successfully. Input names: {InputNames}, Output names: {OutputNames}",
                        string.Join(", ", _inputNames), string.Join(", ", _outputNames));
                    
                    _isInitialized = true;
                }
                catch (OnnxRuntimeException ex)
                {
                    _logger.LogCritical(ex, "Failed to load ONNX model from {ModelPath}. The file may be corrupt or incompatible.", modelPath);
                    _isInitialized = false;
                    _session = null;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "An unexpected error occurred while initializing the ONNX Inference Service with model {ModelPath}", modelPath);
                    _isInitialized = false;
                    _session = null;
                }
            });
        }

        /// <summary>
        /// Processes a single data point to produce an anomaly score.
        /// This is a simplified example assuming a single float input and a single float output (score).
        /// A real implementation would need a more complex mapping from DataPoint to model inputs.
        /// </summary>
        /// <param name="dataPoint">The data point to process.</param>
        /// <returns>The anomaly score as a float, or null if processing fails.</returns>
        public float? GetAnomalyScore(DataPoint dataPoint)
        {
            if (!_isInitialized || _session == null)
            {
                _logger.LogWarning("Inference service is not initialized. Cannot process data point for TagId {TagId}.", dataPoint.TagId);
                return null;
            }

            if (_inputNames.Count != 1)
            {
                _logger.LogWarning("This service currently only supports models with a single input, but the loaded model has {InputCount}.", _inputNames.Count);
                return null;
            }

            try
            {
                // Convert the incoming data point value to a float.
                // This is a major assumption and would need robust type checking/conversion in production.
                if (!float.TryParse(dataPoint.Value?.ToString(), out float floatValue))
                {
                    _logger.LogTrace("Could not convert value '{Value}' for TagId {TagId} to float for inference.", dataPoint.Value, dataPoint.TagId);
                    return null;
                }

                // Assuming the model expects a 1-dimensional tensor with a single value, e.g., shape [1, 1]
                var dimensions = new int[] { 1, 1 };
                var inputTensor = new DenseTensor<float>(new[] { floatValue }, dimensions);
                
                var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor(_inputNames[0], inputTensor)
                };

                using var results = _session.Run(inputs);
                
                var output = results.FirstOrDefault();
                if (output == null)
                {
                    _logger.LogWarning("Inference ran but produced no output.");
                    return null;
                }
                
                // Assuming the output is a float tensor
                if (output.Value is DenseTensor<float> outputTensor)
                {
                    return outputTensor.GetValue(0);
                }
                else
                {
                     _logger.LogWarning("Model output was not of the expected type DenseTensor<float>. Actual type: {OutputType}", output.Value.GetType().Name);
                     return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during model inference for TagId {TagId}.", dataPoint.TagId);
                return null;
            }
        }
        
        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}