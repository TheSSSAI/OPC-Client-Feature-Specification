namespace System.Services.IdentityAccess.Domain.Entities
{
    /// <summary>
    /// Represents the junction entity for the many-to-many relationship between User and Role.
    /// It includes an optional scope to apply the role to a specific part of the asset hierarchy.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Gets the identifier of the user.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the identifier of the role.
        /// </summary>
        public Guid RoleId { get; private set; }

        /// <summary>
        /// Gets the optional identifier for an asset, scoping this role assignment to a specific
        /// part of the asset hierarchy. If null, the role is global for the user within the tenant.
        /// </summary>
        public Guid? AssetScopeId { get; private set; }

        /// <summary>
        /// Navigation property to the User.
        /// </summary>
        public User? User { get; private set; }

        /// <summary>
        /// Navigation property to the Role.
        /// </summary>
        public Role? Role { get; private set; }

        /// <summary>
        /// Private constructor for EF Core.
        /// </summary>
        private UserRole() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRole"/> class.
        /// </summary>
        /// <param name="userId">The user's identifier.</param>
        /// <param name="roleId">The role's identifier.</param>
        /// <param name="assetScopeId">Optional. The asset scope identifier.</param>
        public UserRole(Guid userId, Guid roleId, Guid? assetScopeId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }
            if (roleId == Guid.Empty)
            {
                throw new ArgumentException("Role ID cannot be empty.", nameof(roleId));
            }
            if (assetScopeId == Guid.Empty)
            {
                throw new ArgumentException("Asset scope ID cannot be an empty GUID. Use null for a global scope.", nameof(assetScopeId));
            }

            UserId = userId;
            RoleId = roleId;
            AssetScopeId = assetScopeId;
        }
    }
}