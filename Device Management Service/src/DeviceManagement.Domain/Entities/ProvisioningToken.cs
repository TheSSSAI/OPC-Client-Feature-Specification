using System;
using System.Security.Cryptography;
using System.Text;

namespace System.Services.DeviceManagement.Domain.Entities
{
    /// <summary>
    /// Represents a single-use, time-limited token used to securely provision a new OPC Core Client.
    /// This entity is central to the secure bootstrapping process defined in REQ-1-082 and SEQ-74.
    /// </summary>
    public class ProvisioningToken
    {
        /// <summary>
        /// The SHA256 hash of the plaintext token. The plaintext token is only shown once to the administrator
        /// and is never stored in the system.
        /// </summary>
        public string TokenHash { get; private set; }

        /// <summary>
        /// The unique identifier of the OpcCoreClient entity this token is associated with.
        /// Foreign key to the OpcCoreClients table.
        /// </summary>
        public Guid ClientId { get; private set; }

        /// <summary>
        /// The UTC timestamp after which this token is no longer valid for registration.
        /// </summary>
        public DateTime ExpiryUtc { get; private set; }

        /// <summary>
        /// A flag indicating whether the token has already been successfully used for registration.
        /// This prevents token replay attacks.
        /// </summary>
        public bool IsUsed { get; private set; }

        /// <summary>
        /// EF Core constructor.
        /// </summary>
        private ProvisioningToken() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvisioningToken"/> class.
        /// </summary>
        /// <param name="plaintextToken">The secure, plaintext token generated for the client.</param>
        /// <param name="clientId">The unique identifier of the client being provisioned.</param>
        /// <param name="lifetime">The duration for which the token will be valid.</param>
        public ProvisioningToken(string plaintextToken, Guid clientId, TimeSpan lifetime)
        {
            if (string.IsNullOrWhiteSpace(plaintextToken))
            {
                throw new ArgumentException("Plaintext token cannot be null or whitespace.", nameof(plaintextToken));
            }
            
            if (clientId == Guid.Empty)
            {
                throw new ArgumentException("Client ID cannot be empty.", nameof(clientId));
            }

            if (lifetime <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(lifetime), "Token lifetime must be a positive duration.");
            }

            TokenHash = HashToken(plaintextToken);
            ClientId = clientId;
            ExpiryUtc = DateTime.UtcNow.Add(lifetime);
            IsUsed = false;
        }

        /// <summary>
        /// Determines if the token is currently valid for use in registration.
        /// A valid token is one that has not been used and has not expired.
        /// </summary>
        /// <returns>True if the token is valid; otherwise, false.</returns>
        public bool IsValid()
        {
            return !IsUsed && DateTime.UtcNow < ExpiryUtc;
        }

        /// <summary>
        /// Marks the token as used, invalidating it for any future registration attempts.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the token has already been used or has expired.
        /// </exception>
        public void MarkAsUsed()
        {
            if (IsUsed)
            {
                throw new InvalidOperationException("This provisioning token has already been used.");
            }

            if (DateTime.UtcNow >= ExpiryUtc)
            {
                throw new InvalidOperationException("This provisioning token has expired.");
            }

            IsUsed = true;
        }

        /// <summary>
        /// Hashes the provided plaintext token using SHA256 for secure storage.
        /// </summary>
        /// <param name="plaintextToken">The token to hash.</param>
        /// <returns>A SHA256 hash string of the token.</returns>
        public static string HashToken(string plaintextToken)
        {
            if (string.IsNullOrEmpty(plaintextToken)) return string.Empty;

            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plaintextToken));
            return Convert.ToBase64String(bytes);
        }
    }
}