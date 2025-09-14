using MediatR;
using Microsoft.Extensions.Logging;
using System.Services.IdentityAccess.Application.Contracts;
using System.Services.IdentityAccess.Application.Contracts.External;
using System.Services.IdentityAccess.Application.DTOs;
using System.Services.IdentityAccess.Domain.Common;
using System.Services.IdentityAccess.Domain.Entities;
using System.Services.IdentityAccess.Domain.Errors;
using System.Services.IdentityAccess.Domain.Interfaces;

namespace System.Services.IdentityAccess.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Handles the command for creating a new user. This is a critical workflow that involves
    /// business rule validation (license limits), external service integration (Keycloak),
    /// and transactional data persistence.
    /// </summary>
    public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly IKeycloakAdminClient _keycloakAdminClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTenantService _currentTenantService;
        private readonly IAuditService _auditService;
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ILicenseRepository licenseRepository,
            IKeycloakAdminClient keycloakAdminClient,
            IUnitOfWork unitOfWork,
            ICurrentTenantService currentTenantService,
            IAuditService auditService,
            ILogger<CreateUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _licenseRepository = licenseRepository;
            _keycloakAdminClient = keycloakAdminClient;
            _unitOfWork = unitOfWork;
            _currentTenantService = currentTenantService;
            _auditService = auditService;
            _logger = logger;
        }

        /// <summary>
        /// Orchestrates the creation of a new user.
        /// </summary>
        /// <param name="request">The command containing user details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Result object containing either the new user's DTO or an error.</returns>
        public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentTenantService.TenantId;
            if (tenantId == Guid.Empty)
            {
                return Result.Failure<UserDto>(DomainErrors.Tenant.NotFound);
            }
            
            _logger.LogInformation("Handling CreateUserCommand for email {Email} in tenant {TenantId}", request.Email, tenantId);

            // 1. Validate email uniqueness within the tenant
            if (!await _userRepository.IsEmailUniqueAsync(request.Email, tenantId, cancellationToken))
            {
                _logger.LogWarning("User creation failed for email {Email} in tenant {TenantId}: Email is not unique.", request.Email, tenantId);
                return Result.Failure<UserDto>(DomainErrors.User.EmailNotUnique);
            }

            // 2. Check license limits for the tenant
            var licenseResult = await CheckLicenseLimits(tenantId, cancellationToken);
            if (licenseResult.IsFailure)
            {
                return Result.Failure<UserDto>(licenseResult.Error);
            }
            
            // 3. Validate provided roles
            var roles = await _roleRepository.GetRolesByIdsAsync(request.RoleIds, tenantId, cancellationToken);
            if (roles.Count != request.RoleIds.Count)
            {
                _logger.LogWarning("User creation failed for email {Email} in tenant {TenantId}: One or more role IDs are invalid.", request.Email, tenantId);
                return Result.Failure<UserDto>(DomainErrors.Role.NotFound);
            }

            // 4. Create user in the external Identity Provider (Keycloak)
            // This is the primary point of failure for external systems.
            var keycloakUserResult = await _keycloakAdminClient.CreateUserAsync(
                tenantId.ToString(),
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password,
                cancellationToken);

            if (keycloakUserResult.IsFailure)
            {
                _logger.LogError("Failed to create user {Email} in Keycloak for tenant {TenantId}. Error: {Error}", 
                    request.Email, tenantId, keycloakUserResult.Error);
                return Result.Failure<UserDto>(keycloakUserResult.Error);
            }

            // 5. Create local user entity if Keycloak creation was successful
            var keycloakUserId = keycloakUserResult.Value;
            var user = User.Create(
                new Guid(keycloakUserId), 
                tenantId,
                request.Email,
                request.FirstName,
                request.LastName);

            // 6. Assign roles
            foreach (var role in roles)
            {
                user.AddRole(role.Id);
            }
            
            _userRepository.Add(user);

            // 7. Persist changes to the local database
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Successfully created local user record {UserId} for email {Email} in tenant {TenantId}", user.Id, user.Email, tenantId);

            // 8. Send invitation email and assign roles in Keycloak
            // These are fire-and-forget or handled by a separate process to avoid failing the entire transaction
            // if email sending fails.
            await PostCreationTasks(user.Id.ToString(), tenantId.ToString(), roles, cancellationToken);
            
            // 9. Log audit event
            var auditLog = new AuditLogEntry(
                Guid.NewGuid(),
                tenantId,
                _currentTenantService.UserId, // The administrator performing the action
                DateTime.UtcNow,
                "User Created",
                nameof(User),
                user.Id.ToString(),
                $"New user '{user.Email}' created.",
                null,
                new { user.Email, user.FirstName, user.LastName, Roles = roles.Select(r => r.Name) });

            await _auditService.LogAsync(auditLog);

            // 10. Map to DTO and return success
            var userDto = new UserDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                roles.Select(r => r.Name).ToList());

            return Result.Success(userDto);
        }

        private async Task<Result> CheckLicenseLimits(Guid tenantId, CancellationToken cancellationToken)
        {
            var license = await _licenseRepository.GetByTenantIdAsync(tenantId, cancellationToken);
            if (license == null)
            {
                _logger.LogError("Tenant {TenantId} does not have a valid license.", tenantId);
                return Result.Failure(DomainErrors.License.NotFound);
            }

            if (license.MaxUsers > 0) // 0 or less might mean unlimited
            {
                var currentUserCount = await _userRepository.CountUsersInTenantAsync(tenantId, cancellationToken);
                if (currentUserCount >= license.MaxUsers)
                {
                    _logger.LogWarning("User creation failed for tenant {TenantId}: User limit of {MaxUsers} reached.", tenantId, license.MaxUsers);
                    return Result.Failure(DomainErrors.License.UserLimitExceeded);
                }
            }

            return Result.Success();
        }

        private async Task PostCreationTasks(string keycloakUserId, string tenantId, List<Role> roles, CancellationToken cancellationToken)
        {
            try
            {
                // Assign roles in Keycloak so they appear in the JWT
                var assignRolesResult = await _keycloakAdminClient.AssignRolesToUserAsync(keycloakUserId, roles.Select(r => r.Name).ToList(), cancellationToken);
                if(assignRolesResult.IsFailure)
                {
                    _logger.LogError("Failed to assign roles in Keycloak for user {UserId}: {Error}", keycloakUserId, assignRolesResult.Error);
                    // This is a compensating action scenario. The user is created but roles are not synced.
                    // A background job might be needed for reconciliation. For now, we log the error.
                }

                // Send invitation email
                var sendInviteResult = await _keycloakAdminClient.SendInvitationEmailAsync(keycloakUserId, cancellationToken);
                if(sendInviteResult.IsFailure)
                {
                     _logger.LogError("Failed to send invitation email from Keycloak for user {UserId}: {Error}", keycloakUserId, sendInviteResult.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during post-creation tasks for user {UserId}", keycloakUserId);
            }
        }
    }
}