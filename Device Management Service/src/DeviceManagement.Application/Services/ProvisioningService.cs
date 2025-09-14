using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Services.DeviceManagement.Domain.Entities;
using System.Services.DeviceManagement.Domain.Enums;
using System.Services.DeviceManagement.Domain.Interfaces;
using System.Services.Shared.Application.Exceptions;
using System.Services.Shared.Domain.Services; // Assuming a shared ICertificateService

namespace System.Services.DeviceManagement.Application.Services
{
    /// <summary>
    /// Implements the business logic for secure client bootstrapping and provisioning.
    /// </summary>
    public class ProvisioningService
    {
        private readonly IProvisioningTokenRepository _provisioningTokenRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICertificateService _certificateService; // Abstracted infrastructure dependency
        private readonly ILogger<ProvisioningService> _logger;
        private const int TokenLifetimeHours = 24;

        public ProvisioningService(
            IProvisioningTokenRepository provisioningTokenRepository,
            IClientRepository clientRepository,
            ICertificateService certificateService,
            ILogger<ProvisioningService> logger)
        {
            _provisioningTokenRepository = provisioningTokenRepository ?? throw new ArgumentNullException(nameof(provisioningTokenRepository));
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _certificateService = certificateService ?? throw new ArgumentNullException(nameof(certificateService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Generates a single-use provisioning token for a new client.
        /// </summary>
        /// <param name="tenantId">The tenant ID the new client will belong to.</param>
        /// <param name="clientName">A user-friendly name for the new client.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The plaintext, single-use token.</returns>
        public async Task<string> GenerateProvisioningTokenAsync(Guid tenantId, string clientName, CancellationToken cancellationToken = default)
        {
            if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
            if (string.IsNullOrWhiteSpace(clientName)) throw new ArgumentException("Client name cannot be empty.", nameof(clientName));
            
            var newClient = new OpcCoreClient
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Name = clientName,
                Status = ClientStatus.Pending,
                SoftwareVersion = "N/A"
            };

            await _clientRepository.AddAsync(newClient, cancellationToken);
            _logger.LogInformation("Created new client record {ClientId} in Pending state for tenant {TenantId}.", newClient.Id, tenantId);

            var plainTextToken = GenerateSecureToken();
            var tokenHash = HashToken(plainTextToken);
            
            var provisioningToken = new ProvisioningToken
            {
                TokenHash = tokenHash,
                ClientId = newClient.Id,
                ExpiryUtc = DateTime.UtcNow.AddHours(TokenLifetimeHours),
                IsUsed = false
            };

            await _provisioningTokenRepository.AddAsync(provisioningToken, cancellationToken);
            _logger.LogInformation("Generated and stored provisioning token for client {ClientId}.", newClient.Id);

            return plainTextToken;
        }

        /// <summary>
        /// Registers a new client by validating its token and signing its Certificate Signing Request (CSR).
        /// </summary>
        /// <param name="token">The single-use provisioning token.</param>
        /// <param name="csrPem">The client's CSR in PEM format.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The signed client certificate in PEM format.</returns>
        public async Task<string> RegisterClientAsync(string token, string csrPem, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException("Token cannot be empty.", nameof(token));
            if (string.IsNullOrWhiteSpace(csrPem)) throw new ArgumentException("CSR cannot be empty.", nameof(csrPem));

            var tokenHash = HashToken(token);
            var provisioningToken = await _provisioningTokenRepository.FindByHashAsync(tokenHash, cancellationToken);

            if (provisioningToken == null)
            {
                _logger.LogWarning("Registration attempt with an unknown token hash.");
                throw new SecurityException("Invalid provisioning token.");
            }

            if (provisioningToken.IsUsed)
            {
                _logger.LogWarning("Registration attempt with an already used token for client {ClientId}.", provisioningToken.ClientId);
                throw new SecurityException("Provisioning token has already been used.");
            }

            if (provisioningToken.ExpiryUtc < DateTime.UtcNow)
            {
                _logger.LogWarning("Registration attempt with an expired token for client {ClientId}.", provisioningToken.ClientId);
                throw new SecurityException("Provisioning token has expired.");
            }

            var client = await _clientRepository.GetByIdAsync(provisioningToken.ClientId, cancellationToken);
            if (client == null)
            {
                _logger.LogError("Could not find client {ClientId} associated with a valid token.", provisioningToken.ClientId);
                throw new InvalidOperationException("Client record not found for a valid token.");
            }

            // The client's identity for mTLS will be its unique ID. This is more secure than using a user-provided name.
            string commonName = client.Id.ToString();
            string signedCertificatePem;

            try
            {
                signedCertificatePem = await _certificateService.SignCsrAsync(csrPem, commonName, 365 * 2); // 2-year validity
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sign CSR for client {ClientId}.", client.Id);
                throw new InvalidOperationException("Certificate signing failed.", ex);
            }
            
            // Atomically update state
            provisioningToken.IsUsed = true;
            await _provisioningTokenRepository.UpdateAsync(provisioningToken, cancellationToken);

            client.Status = ClientStatus.Offline;
            client.CertificateCommonName = commonName;
            await _clientRepository.UpdateAsync(client, cancellationToken);

            _logger.LogInformation("Successfully registered client {ClientId}. Issued certificate with CN {CommonName}.", client.Id, commonName);

            return signedCertificatePem;
        }

        private string GenerateSecureToken(int length = 32)
        {
            var bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes)
                .Replace("/", "_")
                .Replace("+", "-")
                .TrimEnd('=');
        }

        private string HashToken(string token)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}