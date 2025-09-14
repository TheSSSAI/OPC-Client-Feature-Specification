using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace System.Edge.OpcCoreClient.Services
{
    /// <summary>
    /// Handles the one-time, secure provisioning of a new OPC Core Client instance.
    /// Implements the client-side logic for REQ-1-082 and sequence diagram #74.
    /// </summary>
    public class ProvisioningService
    {
        private readonly ILogger<ProvisioningService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _certPath;
        private readonly string _keyPath;
        private readonly string _provisioningUrl;

        // In a real application, these would come from IOptions<T>
        public ProvisioningService(ILogger<ProvisioningService> logger, IHttpClientFactory httpClientFactory, string provisioningUrl, string certificatePath, string privateKeyPath)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _provisioningUrl = provisioningUrl ?? throw new ArgumentNullException(nameof(provisioningUrl));
            _certPath = certificatePath ?? throw new ArgumentNullException(nameof(certificatePath));
            _keyPath = privateKeyPath ?? throw new ArgumentNullException(nameof(privateKeyPath));
        }

        /// <summary>
        /// Checks if the client is already provisioned. If not, uses the registration token to perform provisioning.
        /// </summary>
        /// <param name="registrationToken">The one-time registration token.</param>
        /// <returns>True if the client is provisioned and ready, false otherwise.</returns>
        public async Task<bool> EnsureProvisionedAsync(string registrationToken)
        {
            if (IsProvisioned())
            {
                _logger.LogInformation("Client is already provisioned. Certificate found at {CertificatePath}.", _certPath);
                return true;
            }

            _logger.LogInformation("Client is not provisioned. Attempting to register using a one-time token.");

            if (string.IsNullOrWhiteSpace(registrationToken))
            {
                _logger.LogCritical("Registration token is missing. Cannot provision the client. Please provide a registration token via configuration.");
                return false;
            }

            try
            {
                using var rsa = RSA.Create(4096);
                var csr = CreateCertificateSigningRequest(rsa);
                string csrPem = ExportCsrToPem(csr);
                
                _logger.LogDebug("Generated CSR. Submitting to provisioning service at {ProvisioningUrl}.", _provisioningUrl);
                
                string signedCertPem = await RequestSignedCertificateAsync(registrationToken, csrPem);

                await StoreCredentialsAsync(rsa, signedCertPem);
                
                _logger.LogInformation("Provisioning successful. Client certificate and private key have been stored.");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "A critical error occurred during the client provisioning process. The client cannot start.");
                return false;
            }
        }
        
        public bool IsProvisioned() => File.Exists(_certPath) && File.Exists(_keyPath);

        private CertificateRequest CreateCertificateSigningRequest(RSA rsa)
        {
            var subjectName = $"CN={Environment.MachineName}-{Guid.NewGuid()}";
            var request = new CertificateRequest(subjectName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            
            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, true));
            request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new Oid("1.3.6.1.5.5.7.3.2") }, false)); // TLS Client auth
            request.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(request.PublicKey, false));
            
            return request;
        }

        private string ExportCsrToPem(CertificateRequest csr)
        {
            byte[] csrBytes = csr.CreateSigningRequest();
            return PemEncoding.Write("CERTIFICATE REQUEST", csrBytes);
        }

        private async Task<string> RequestSignedCertificateAsync(string token, string csrPem)
        {
            var httpClient = _httpClientFactory.CreateClient("ProvisioningClient");
            var requestPayload = new { Token = token, Csr = csrPem };

            HttpResponseMessage response = await httpClient.PostAsJsonAsync(_provisioningUrl, requestPayload);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Provisioning request failed with status code {StatusCode}. Response: {Response}", response.StatusCode, errorContent);
                throw new HttpRequestException($"Provisioning failed: {response.StatusCode}");
            }

            var responsePayload = await response.Content.ReadFromJsonAsync<SignedCertificateResponse>();
            if (responsePayload == null || string.IsNullOrWhiteSpace(responsePayload.Certificate))
            {
                throw new InvalidOperationException("Provisioning service returned a success status but an invalid payload.");
            }
            return responsePayload.Certificate;
        }

        private async Task StoreCredentialsAsync(RSA rsa, string signedCertPem)
        {
            string privateKeyPem = rsa.ExportRSAPrivateKeyPem();

            var directory = Path.GetDirectoryName(_certPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // In production on Linux, we should set file permissions to be readable only by the owner (e.g., 0600).
            await File.WriteAllTextAsync(_keyPath, privateKeyPem, Encoding.UTF8);
            await File.WriteAllTextAsync(_certPath, signedCertPem, Encoding.UTF8);
        }

        private sealed class SignedCertificateResponse
        {
            public string Certificate { get; set; }
        }
    }
}