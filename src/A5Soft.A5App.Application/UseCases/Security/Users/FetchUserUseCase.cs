using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security.Users
{
    /// <inheritdoc cref="IFetchUserUseCase"/>
    [DefaultServiceImplementation(typeof(IFetchUserUseCase))]
    public class FetchUserUseCase : FetchDomainEntityUseCaseBase<User>, IFetchUserUseCase
    {
        private readonly IUserRepository _repository;
        private readonly IFetchTenantLookupUseCase _tenantLookup;
        private readonly IUserGroupLookupService _userGroupLookup;
        private readonly IUserRoleLookupService _userRoleLookup;


        /// <inheritdoc />
        public FetchUserUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal,
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, ILogger logger,
            IUserRepository repository, IFetchTenantLookupUseCase tenantLookup, IUserGroupLookupService userGroupLookup,
            IUserRoleLookupService userRoleLookup) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _tenantLookup = tenantLookup ?? throw new ArgumentNullException(nameof(tenantLookup));
            _userGroupLookup = userGroupLookup ?? throw new ArgumentNullException(nameof(userGroupLookup));
            _userRoleLookup = userRoleLookup ?? throw new ArgumentNullException(nameof(userRoleLookup));
        }


        /// <inheritdoc cref="IFetchUserUseCase.FetchUserGroupLookupAsync"/>
        public async Task<List<UserGroupLookup>> FetchUserGroupLookupAsync()
        {
            return await _userGroupLookup.FetchAsync(this.GetType());
        }

        /// <inheritdoc cref="IFetchUserUseCase.FetchUserRoleLookupAsync"/>
        public async Task<List<UserRoleLookup>> FetchUserRoleLookupAsync(IUserTenant forUserTenant)
        {
            if (null == forUserTenant) throw new ArgumentNullException(nameof(forUserTenant));

            var result = (await _userRoleLookup.FetchAsync(this.GetType())).AsReadOnly();

            if (UserRoleLookup.CustomRole.Id.IsSameIdentityAs(forUserTenant.RoleId))
            {
                var res = result.ToList();
                res.Insert(0, UserRoleLookup.CustomRole);
                return res;
            }

            return result.ToList();
        }

        /// <inheritdoc cref="IFetchUserUseCase.FetchAdministrativeRoleLookupAsync"/>
        public async Task<AdministrativeRole[]> FetchAdministrativeRoleLookupAsync()
        {
            var identity = await GetIdentityAsync();
            if (identity.IsAdmin()) return new AdministrativeRole[] 
                {AdministrativeRole.Admin, AdministrativeRole.GroupAdmin, AdministrativeRole.None};
            return new AdministrativeRole[] { AdministrativeRole.None };
        }

        protected override async Task<User> DoFetchAsync(IDomainEntityIdentity id, CancellationToken ct)
        {
            var identity = await GetIdentityAsync();

            if (id.IsSameIdentityAs(identity.Sid().ToIdentity<User>()))
            {
                Logger.LogSecurityIssue($"User {identity.Name} attempted to access data about self.");
                throw new AuthorizationException(Resources.Security_FetchUserUseCase_No_Self_Fetch);
            }

            var tenantsLookup = await _tenantLookup.QueryAsync(ct);

            var dto = await _repository.FetchAsync(id, tenantsLookup, ct);

            if (null == dto) throw new NotFoundException(typeof(User),
                id.ToString(), MetadataProvider);

            var userGroupLookup = await _userGroupLookup.FetchAsync(this.GetType());
            var roleLookup = await _userRoleLookup.FetchAsync(this.GetType());

            var result = new User(dto, tenantsLookup, roleLookup, userGroupLookup, ValidationProvider);

            if (identity.IsGroupAdmin())
            {
                if (null == result.UserGroup ||
                    !identity.GroupSid().ToIdentity<UserGroup>().IsSameIdentityAs(result.UserGroup.Id))
                {
                    Logger.LogSecurityIssue($"User {identity.Name} attempted to access data of the user " +
                        $"outside of his group - {result.Email}.");
                    throw new AuthorizationException(Resources.Security_FetchUserUseCase_Unauthorized_For_GroupAdmin);
                }

                if (result.AdminRole != AdministrativeRole.None)
                {
                    Logger.LogSecurityIssue($"User {identity.Name} attempted to access data of the admin user " +
                        $"- {result.Email}.");
                    throw new AuthorizationException(Resources.Security_FetchUserUseCase_Unauthorized_Admin_Fetch_By_GroupAdmin);
                }
            }

            return result;
        }

    }
}
