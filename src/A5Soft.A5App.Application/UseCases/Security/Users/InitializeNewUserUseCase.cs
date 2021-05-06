using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    /// <inheritdoc cref="IInitializeNewUserUseCase"/>
    [DefaultServiceImplementation(typeof(IInitializeNewUserUseCase))]
    public class InitializeNewUserUseCase : FetchDomainSingletonUseCaseBase<User>, IInitializeNewUserUseCase
    {
        private readonly IFetchTenantLookupUseCase _tenantLookup;
        private readonly IUserGroupLookupService _userGroupLookup;
        private readonly IUserRoleLookupService _userRoleLookup;


        /// <inheritdoc />
        public InitializeNewUserUseCase(IAuthenticationStateProvider authenticationStateProvider,
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal,
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, ILogger logger,
            IFetchTenantLookupUseCase tenantLookup, IUserGroupLookupService userGroupLookup,
            IUserRoleLookupService userRoleLookup) : base(authenticationStateProvider, authorizer, dataPortal,
            validationProvider, metadataProvider, logger)
        {
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
        public async Task<List<UserRoleLookup>> FetchUserRoleLookup(IUserTenant forUserTenant)
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


        protected override async Task<User> DoFetchAsync(CancellationToken ct)
        {
            var tenantsLookup = await _tenantLookup.QueryAsync(ct);
            return new User(tenantsLookup, ValidationProvider);
        }

    }
}
