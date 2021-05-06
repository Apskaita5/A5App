using System.Collections.Generic;
using System.Threading.Tasks;
using A5Soft.A5App.Application.UseCases.Plugins;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.Repositories
{
    /// <summary>
    /// A repository for managing plugins installed for tenants.
    /// </summary>
    [Service(ServiceLifetime.Transient)]
    public interface ITenantPluginRepository
    {
        Task<List<InstalledPluginLookup>> FetchLookupAsync();
    }
}
