using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security.UserRoles
{
    /// <inheritdoc cref="ICreateUserRoleUseCase"/>
    [DefaultServiceImplementation(typeof(ICreateUserRoleUseCase))]
    public class CreateUserRoleUseCase : SaveUseCaseBase<UserRole, IUserRole>, ICreateUserRoleUseCase
    {
        private readonly ICacheProvider _cache;
        private readonly IUserRoleRepository _repository;
        private readonly IPluginProvider _pluginProvider;


        /// <inheritdoc />
        public CreateUserRoleUseCase(IAuthenticationStateProvider authenticationStateProvider,
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal,
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, ILogger logger,
            ICacheProvider cache, IUserRoleRepository repository, IPluginProvider pluginProvider) : base(
            authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _pluginProvider = pluginProvider ?? throw new ArgumentNullException(nameof(pluginProvider));
        }


        protected override async Task<UserRole> DoSaveAsync(IUserRole domainDto)
        {
            var allPermissions = new List<Permission>(DefaultPermissions.AllPermissions);
            allPermissions.AddRange(_pluginProvider.GetPermissions());
            var list = allPermissions.Select(p => new UserRolePermission.UserRolePermissionDto(p))
                .OrderBy(p => p.ModuleName)
                .ThenBy(p => p.GroupName)
                .ThenBy(p => p.Order)
                .ToList();

            var current = new UserRole(list, ValidationProvider);

            current.Merge(domainDto, true);

            var identity = await GetIdentityAsync();

            var result = await _repository.InsertAsync(current.ToDto(), identity.Email());

            _cache.Clear(typeof(List<UserRoleLookup>));

            return new UserRole(result, ValidationProvider);
        }

    }
}
