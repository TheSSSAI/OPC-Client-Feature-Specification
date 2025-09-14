namespace System.Services.IdentityAccess.Domain.Entities
{
    /// <summary>
    /// Represents the software license for a specific tenant.
    /// This entity is part of the Tenant aggregate.
    /// It defines the limits and features available to the tenant.
    /// </summary>
    public class License
    {
        /// <summary>
        /// Gets the unique identifier for the license.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the identifier of the tenant this license applies to.
        /// </summary>
        public Guid TenantId { get; private set; }

        /// <summary>
        /// Gets the name of the license plan or tier (e.g., 'Silver', 'Gold').
        /// </summary>
        public string PlanName { get; private set; }

        /// <summary>
        /// Gets the maximum number of user accounts allowed under this license.
        /// </summary>
        public int MaxUsers { get; private set; }

        /// <summary>
        /// Gets the maximum number of OPC Core Client instances allowed.
        /// </summary>
        public int MaxClients { get; private set; }

        /// <summary>
        /// Gets a value indicating whether AI/ML features are enabled by this license.
        /// </summary>
        public bool IsAiEnabled { get; private set; }

        /// <summary>
        /// Gets the expiration date of the license. Null for perpetual or subscription-based licenses
        /// where status is managed externally.
        /// </summary>
        public DateTime? ExpirationDate { get; private set; }

        /// <summary>
        /// Navigation property to the parent Tenant.
        /// </summary>
        public Tenant? Tenant { get; private set; }

        /// <summary>
        /// Private constructor for EF Core.
        /// </summary>
        private License()
        {
            PlanName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="License"/> class.
        /// </summary>
        /// <param name="id">The license's unique identifier.</param>
        /// <param name="tenantId">The identifier of the tenant this license is for.</param>
        /// <param name="planName">The name of the license plan.</param>
        /// <param name="maxUsers">The maximum number of users.</param>
        /// <param name="maxClients">The maximum number of clients.</param>
        /// <param name="isAiEnabled">Whether AI features are enabled.</param>
        /// <param name="expirationDate">The optional expiration date.</param>
        public License(Guid id, Guid tenantId, string planName, int maxUsers, int maxClients, bool isAiEnabled, DateTime? expirationDate)
        {
            if (id == Guid.Empty) throw new ArgumentException("License ID cannot be empty.", nameof(id));
            if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
            if (string.IsNullOrWhiteSpace(planName)) throw new ArgumentException("Plan name cannot be empty.", nameof(planName));
            if (maxUsers < 1) throw new ArgumentOutOfRangeException(nameof(maxUsers), "Max users must be at least 1.");
            if (maxClients < 0) throw new ArgumentOutOfRangeException(nameof(maxClients), "Max clients cannot be negative.");

            Id = id;
            TenantId = tenantId;
            PlanName = planName;
            MaxUsers = maxUsers;
            MaxClients = maxClients;
            IsAiEnabled = isAiEnabled;
            ExpirationDate = expirationDate;
        }

        /// <summary>
        /// Determines if a new user can be added based on the license limit.
        /// </summary>
        /// <param name="currentUserCount">The current number of users in the tenant.</param>
        /// <returns>True if the user limit has not been reached; otherwise, false.</returns>
        public bool CanAddUser(int currentUserCount)
        {
            return currentUserCount < MaxUsers;
        }

        /// <summary>
        /// Determines if a new client can be added based on the license limit.
        /// </summary>
        /// <param name="currentClientCount">The current number of clients in the tenant.</param>
        /// <returns>True if the client limit has not been reached; otherwise, false.</returns>
        public bool CanAddClient(int currentClientCount)
        {
            return currentClientCount < MaxClients;
        }

        /// <summary>
        /// Checks if the license is currently active (not expired).
        /// </summary>
        /// <returns>True if the license is active; otherwise, false.</returns>
        public bool IsActive()
        {
            return ExpirationDate == null || ExpirationDate.Value.ToUniversalTime() > DateTime.UtcNow;
        }
    }
}