using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Repositories;
using A5Soft.A5App.Application.UseCases.Plugins;
using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Repositories.Plugins
{
    /// <summary>
    /// default implementation for <see cref="ITenantPluginRepository"/>
    /// </summary>
    [DefaultServiceImplementation(typeof(ITenantPluginRepository))]
    public class TenantPluginRepository : ITenantPluginRepository
    {
        private readonly ITenantOrmServiceProvider _ormServiceProvider;
        private readonly Guid? _tenantId;


        public TenantPluginRepository(ITenantOrmServiceProvider ormServiceProvider, ClaimsIdentity user)
        {
            if (null == user) throw new ArgumentNullException(nameof(user));

            _ormServiceProvider = ormServiceProvider ?? throw new ArgumentNullException(nameof(ormServiceProvider));
            _tenantId = user.TenantSid();
        }


        public async Task<List<InstalledPluginLookup>> FetchLookupAsync()
        {
            if (!_tenantId.HasValue) throw new InvalidOperationException("No tenant selected.");
            var ormService = await _ormServiceProvider.GetServiceAsync(_tenantId.Value);
            return await ormService.FetchAllEntitiesAsync<InstalledPluginLookup>();
        }

    }
}
