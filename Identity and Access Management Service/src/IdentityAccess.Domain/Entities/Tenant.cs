namespace System.Services.IdentityAccess.Domain.Entities
{
    /// <summary>
    /// Represents a Tenant in the multi-tenant architecture. A tenant is the root aggregate for data isolation.
    /// It logically separates the data and configurations of different customers.
    /// </summary>
    public class Tenant
    {
        /// <summary>
        /// Gets the unique identifier for the tenant.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the name of the tenant.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Navigation property for the users belonging to this tenant.
        /// </summary>
        private readonly List<User> _users = new();
        public IReadOnlyCollection<User> Users => _users.AsReadOnly();

        /// <summary>
        /// Navigation property for the license associated with this tenant.
        /// </summary>
        public License? License { get; private set; }

        /// <summary>
        /// Private constructor for EF Core.
        /// </summary>
        private Tenant()
        {
            Name = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tenant"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the tenant.</param>
        /// <param name="name">The name of the tenant.</param>
        public Tenant(Guid id, string name)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Tenant ID cannot be empty.", nameof(id));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Tenant name cannot be null or whitespace.", nameof(name));
            }

            Id = id;
            Name = name;
        }

        /// <summary>
        /// Updates the name of the tenant.
        /// </summary>
        /// <param name="name">The new name for the tenant.</param>
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Tenant name cannot be null or whitespace.", nameof(name));
            }
            Name = name;
        }

        /// <summary>
        /// Assigns a license to this tenant.
        /// </summary>
        /// <param name="license">The license to assign.</param>
        public void AssignLicense(License license)
        {
            if (license == null)
            {
                throw new ArgumentNullException(nameof(license));
            }
            if (license.TenantId != Id)
            {
                throw new InvalidOperationException("Cannot assign a license belonging to a different tenant.");
            }
            License = license;
        }

        /// <summary>
        /// Checks if a new user can be added based on the current license.
        /// </summary>
        /// <returns>True if a user can be added, otherwise false.</returns>
        public bool CanAddUser()
        {
            return License?.CanAddUser(_users.Count) ?? false;
        }
    }
}