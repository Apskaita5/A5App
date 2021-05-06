using A5Soft.CARMA.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using A5Soft.A5App.Domain.Security;
using System.Threading.Tasks;
using A5Soft.CARMA.Domain;
using System.Threading;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security.UserRoles
{
    /// <inheritdoc cref="IInitializeNewUserRoleUseCase"/>
    [DefaultServiceImplementation(typeof(IInitializeNewUserRoleUseCase))]
    public class InitializeNewUserRoleUseCase : FetchDomainSingletonUseCaseBase<UserRole>, IInitializeNewUserRoleUseCase
    {
        private readonly IPluginProvider _pluginProvider;


        /// <inheritdoc />
        public InitializeNewUserRoleUseCase(IAuthenticationStateProvider authenticationStateProvider,
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal,
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, ILogger logger,
            IPluginProvider pluginProvider) : base(authenticationStateProvider, authorizer, dataPortal,
            validationProvider, metadataProvider, logger)
        {
            _pluginProvider = pluginProvider ?? throw new ArgumentNullException(nameof(pluginProvider));
        }

        protected override Task<UserRole> DoFetchAsync(CancellationToken ct)
        {
            var allPermissions = new List<Permission>(DefaultPermissions.AllPermissions);
            allPermissions.AddRange(_pluginProvider.GetPermissions());
            var list = allPermissions.Select(p => new UserRolePermission.UserRolePermissionDto(p))
                .OrderBy(p => p.ModuleName)
                .ThenBy(p => p.GroupName)
                .ThenBy(p => p.Order)
                .ToList();

            return Task.FromResult(new UserRole(list, ValidationProvider));
        }

    }
}
