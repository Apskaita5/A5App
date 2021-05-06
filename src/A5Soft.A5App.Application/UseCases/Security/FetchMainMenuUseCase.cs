using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Repositories;
using A5Soft.A5App.Application.UseCases.Plugins;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Application.Navigation;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IFetchMainMenuUseCase"/>
    [DefaultServiceImplementation(typeof(IFetchMainMenuUseCase))]
    public class FetchMainMenuUseCase : FetchMainMenuUseCaseBase, IFetchMainMenuUseCase
    {
        private readonly ITenantPluginRepository _repository;
        private readonly IPluginProvider _pluginProvider;
        private readonly ICacheProvider _cache;


        /// <inheritdoc />
        public FetchMainMenuUseCase(IClientDataPortal dataPortal, IAuthorizationProvider authorizationProvider,
            IAuthenticationStateProvider authenticationStateProvider, ILogger logger, 
            ITenantPluginRepository repository, IPluginProvider pluginProvider, ICacheProvider cache) 
            : base(authenticationStateProvider, dataPortal, authorizationProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _pluginProvider = pluginProvider ?? throw new ArgumentNullException(nameof(pluginProvider));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        protected override MainMenu GetBaseMainMenu()
        {
            return ApplicationMenu.GetMainMenu();
        }
         
        protected override async Task<List<List<(string MenuGroupName, MenuItem MenuItem)>>> GetPluginMenuItemsAsync()
        {
            var result = _pluginProvider.GetApplicationMenuItems()
                .Select(e => e.MenuItems).ToList();

            var identity = await GetIdentityAsync();

            var tenantId = identity.TenantSid();
            if (tenantId.HasValue)
            {
                var installed = await _cache.GetOrCreate<List<InstalledPluginLookup>>(
                    tenantId.Value, async () => await _repository.FetchLookupAsync());

                var installedGuids = installed.Select(v => v.Id).ToList();

                result.AddRange(_pluginProvider.GetApplicationMenuItemsForTenant()
                    .Where(v => installedGuids.Contains(v.PluginId))
                    .Select(v => v.MenuItems));
            }

            return result;
        }

        protected override async Task<Dictionary<Type, Type>> GetReplacedUseCaseInterfaceTypesAsync()
        {
            var resultList = _pluginProvider.GetReplacedUseCaseInterfaceTypes()
                .Select(v => v.ReplacedTypes).ToList();

            var identity = await GetIdentityAsync();

            var tenantId = identity.TenantSid();
            if (tenantId.HasValue)
            {
                var installed = await _cache.GetOrCreate<List<InstalledPluginLookup>>(
                    tenantId.Value, async () => await _repository.FetchLookupAsync());

                var installedGuids = installed.Select(v => v.Id).ToList();

                resultList.AddRange(_pluginProvider.GetReplacedUseCaseInterfaceTypesForTenant()
                    .Where(v => installedGuids.Contains(v.PluginId))
                    .Select(v => v.ReplacedTypes));
            }

            var result = new Dictionary<Type, Type>();
            foreach (var dic in resultList)
            {
                foreach (var keyValuePair in dic)
                {
                    result.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }

            return result;
        }

    }
}
