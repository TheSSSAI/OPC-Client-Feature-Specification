namespace System.Services.IdentityAccess.Domain.Entities
{
    /// <summary>
    /// Represents a user within the system. A user is an aggregate root within the context of a Tenant.
    /// It stores application-specific data and role assignments.
    /// Core identity is managed by Keycloak, this entity stores related metadata.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets the unique identifier for the user. This should correspond to the 'sub' claim in the JWT from Keycloak.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the identifier of the tenant to which this user belongs.
        /// </summary>
        public Guid TenantId { get; private set; }

        /// <summary>
        /// Gets the user's email address. This is considered Personally Identifiable Information (PII).
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the user's first name.
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the user's last name.
        /// </summary>
        public string LastName { get; private set; }
        
        /// <summary>
        /// Navigation property to the parent Tenant.
        /// </summary>
        public Tenant? Tenant { get; private set; }

        /// <summary>
        /// Navigation property for the user-role assignments.
        /// </summary>
        private readonly List<UserRole> _userRoles = new();
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
        
        /// <summary>
        /// Private constructor for EF Core.
        /// </summary>
        private User()
        {
            Email = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The user's unique identifier (from IdP).</param>
        /// <param name="tenantId">The identifier of the tenant the user belongs to.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        public User(Guid id, Guid tenantId, string email, string firstName, string lastName)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(id));
            if (tenantId == Guid.Empty)
                throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or whitespace.", nameof(email));

            Id = id;
            TenantId = tenantId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        /// <summary>
        /// Assigns a role to the user, optionally scoped to a specific asset.
        /// </summary>
        /// <param name="role">The role to assign.</param>
        /// <param name="assetScopeId">Optional. The identifier of an asset to scope this role assignment to.</param>
        public void AssignRole(Role role, Guid? assetScopeId)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (_userRoles.Any(ur => ur.RoleId == role.Id && ur.AssetScopeId == assetScopeId))
            {
                // Role is already assigned with the same scope, do nothing.
                return;
            }

            var userRole = new UserRole(Id, role.Id, assetScopeId);
            _userRoles.Add(userRole);
        }

        /// <summary>
        /// Removes a role assignment from the user.
        /// </summary>
        /// <param name="roleId">The ID of the role to remove.</param>
        /// <param name="assetScopeId">The specific scope of the role assignment to remove.</param>
        public void RemoveRole(Guid roleId, Guid? assetScopeId)
        {
            var userRoleToRemove = _userRoles.FirstOrDefault(ur => ur.RoleId == roleId && ur.AssetScopeId == assetScopeId);
            if (userRoleToRemove != null)
            {
                _userRoles.Remove(userRoleToRemove);
            }
        }

        /// <summary>
        /// Clears all role assignments for this user.
        /// </summary>
        public void ClearRoles()
        {
            _userRoles.Clear();
        }

        /// <summary>
        /// Updates the user's profile information.
        /// </summary>
        /// <param name="firstName">The new first name.</param>
        /// <param name="lastName">The new last name.</param>
        public void UpdateProfile(string firstName, string lastName)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                FirstName = firstName;
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                LastName = lastName;
            }
        }
    }
}