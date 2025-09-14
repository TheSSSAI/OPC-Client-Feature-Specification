namespace System.Services.IdentityAccess.Domain.Entities
{
    /// <summary>
    /// Represents a role in the Role-Based Access Control (RBAC) system.
    /// Roles define a set of permissions that can be assigned to users.
    /// This is an Aggregate Root.
    /// </summary>
    public class Role
    {
        // Predefined role names based on REQ-1-011
        public const string AdministratorRoleName = "Administrator";
        public const string DataScientistRoleName = "Data Scientist";
        public const string EngineerRoleName = "Engineer";
        public const string OperatorRoleName = "Operator";
        public const string ViewerRoleName = "Viewer";

        /// <summary>
        /// Gets the unique identifier for the role.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the name of the role.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this is a default system role that cannot be modified.
        /// </summary>
        public bool IsDefault { get; private set; }

        /// <summary>
        /// Navigation property for the user-role assignments.
        /// </summary>
        private readonly List<UserRole> _userRoles = new();
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

        /// <summary>
        /// Private constructor for EF Core.
        /// </summary>
        private Role()
        {
            Name = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the role.</param>
        /// <param name="name">The name of the role.</param>
        /// <param name="isDefault">Indicates if this is a non-modifiable system role.</param>
        public Role(Guid id, string name, bool isDefault = false)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Role ID cannot be empty.", nameof(id));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Role name cannot be null or whitespace.", nameof(name));
            }

            Id = id;
            Name = name;
            IsDefault = isDefault;
        }

        /// <summary>
        /// Creates instances of the five default system roles.
        /// </summary>
        /// <returns>A collection of the default roles.</returns>
        public static IEnumerable<Role> CreateDefaultRoles()
        {
            yield return new Role(Guid.NewGuid(), AdministratorRoleName, true);
            yield return new Role(Guid.NewGuid(), DataScientistRoleName, true);
            yield return new Role(Guid.NewGuid(), EngineerRoleName, true);
            yield return new Role(Guid.NewGuid(), OperatorRoleName, true);
            yield return new Role(Guid.NewGuid(), ViewerRoleName, true);
        }
    }
}