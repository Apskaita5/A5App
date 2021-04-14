using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="ILoginTenantUseCase"/>
    [DefaultServiceImplementation(typeof(ILoginTenantUseCase))]
    public class LoginTenantUseCase : QueryWithCriteriaUseCaseBase<LoginResponse, Guid>, ILoginTenantUseCase
    {
        private readonly IUserRepository _repository;
        private readonly ISecurityPolicy _policy;


        /// <inheritdoc />
        public LoginTenantUseCase(IUserRepository repository, ISecurityPolicy policy, ClaimsIdentity user, 
            IUseCaseAuthorizer authorizer, IClientDataPortal dataPortal,
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger) : base(user, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }


        protected override async Task<LoginResponse> DoQueryAsync(Guid tenantId, CancellationToken ct)
        {
            if (!tenantId.IsValidKey()) throw new ArgumentNullException(nameof(tenantId));

            var userDetails = await _repository.FetchUserIdentityAsync(User.Sid(), tenantId, ct);

            if (null == userDetails) throw new Exception(
                $"Failed to fetch user details (id = {tenantId}).");

            if (userDetails.IsDisabled)
            {
                Logger.LogSecurityIssue($"User {userDetails.Email} somehow " +
                    $"attempted to login a tenant even though his account is disabled.");
                return new LoginResponse(Resources.Security_LoginUseCase_Account_Disabled);
            }

            if (userDetails.AdminRole == AdministrativeRole.None && !userDetails.Permissions.Any())
            {
                Logger.LogSecurityIssue($"User {userDetails.Email} " +
                    $"attempted to login a tenant that he have no access to.");
                return new LoginResponse(Resources.Security_LoginTenantUseCase_NotAuthorized);
            }

            Logger.LogSecurityIssue($"User {userDetails.Email} has successfully logged in " +
                $"to tenant database (id = {tenantId}).");

            return new LoginResponse(userDetails.ToClaimsIdentity(_policy));
        }
    }
}
