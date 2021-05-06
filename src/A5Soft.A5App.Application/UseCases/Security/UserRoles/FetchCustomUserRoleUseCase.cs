using System;
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

namespace A5Soft.A5App.Application.UseCases.Security.UserRoles
{
    /// <inheritdoc cref="IFetchCustomUserRoleUseCase"/>
    [DefaultServiceImplementation(typeof(IFetchCustomUserRoleUseCase))]
    public class FetchCustomUserRoleUseCase : FetchDomainEntityUseCaseBase<CustomUserRole>, IFetchCustomUserRoleUseCase
    {
        private readonly ICustomUserRoleRepository _repository;


        /// <inheritdoc />
        public FetchCustomUserRoleUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, ICustomUserRoleRepository repository)
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override async Task<CustomUserRole> DoFetchAsync(IDomainEntityIdentity id, CancellationToken ct)
        {
            if (!(id is CustomUserRoleIdentity rid)) throw new InvalidOperationException(
                $"Identity {id} is not assignable to CustomUserRoleIdentity.");

            var identity = await GetIdentityAsync();

            if (rid.UserId.IsSameIdentityAs(identity.Sid().ToIdentity<User>()))
            {
                Logger.LogSecurityIssue($"User {identity.Name} attempted to fetch a custom role for self.");
                throw new AuthorizationException(Resources.Security_FetchCustomUserRoleUseCase_No_Self_Fetch);
            }

            var dto = await _repository.FetchAsync(rid.UserId, rid.TenantId, ct);

            if (null == dto) throw new NotFoundException(typeof(CustomUserRole),
                id.ToString(), MetadataProvider);

            if (identity.IsGroupAdmin())
            {
                if (!identity.GroupSid().ToIdentity<UserGroup>().IsSameIdentityAs(dto.UserGroupId))
                {
                    Logger.LogSecurityIssue($"User {identity.Name} attempted to fetch a custom role for the user " +
                        $"outside of his group - {dto.UserName}.");
                    throw new AuthorizationException(Resources.Security_FetchCustomUserRoleUseCase_No_Fetch_For_Other_UserGroups);
                }
            }

            return new CustomUserRole(dto, ValidationProvider);
        }

    }
}
