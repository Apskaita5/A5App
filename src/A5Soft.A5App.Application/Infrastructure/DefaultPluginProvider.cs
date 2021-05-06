using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application.Navigation;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// Default <see cref="IPluginProvider"/> implementation.
    /// </summary>
    public class DefaultPluginProvider : IPluginProvider
    {
        private readonly List<(string FolderPath, IPlugin Plugin)> _plugins;


        public DefaultPluginProvider(List<(string FolderPath, IPlugin Plugin)> plugins)
        {
            _plugins = plugins ?? throw new ArgumentNullException(nameof(plugins));
            //TODO: validate possible conflicts
        }


        /// <inheritdoc cref="IPluginProvider.GetApplicationMenuItems"/>
        public List<(Guid PluginId, List<(string MenuGroupName, MenuItem MenuItem)> MenuItems)> GetApplicationMenuItems()
        {
            return _plugins.Select(p => (PluginId: p.Plugin.Id, MenuItems: p.Plugin.GetApplicationMenuItems()))
                .Where(i => null != i.MenuItems && i.MenuItems.Any())
                .ToList();
        }

        /// <inheritdoc cref="IPluginProvider.GetApplicationMenuItemsForTenant"/>
        public List<(Guid PluginId, List<(string MenuGroupName, MenuItem MenuItem)> MenuItems)> GetApplicationMenuItemsForTenant()
        {
            return _plugins.Select(p => (PluginId: p.Plugin.Id, MenuItems: p.Plugin.GetApplicationMenuItemsForTenant()))
                .Where(i => null != i.MenuItems && i.MenuItems.Any())
                .ToList();
        }

        /// <inheritdoc cref="IPluginProvider.GetApplicationServices"/>
        public List<ApplicationServiceInfo> GetApplicationServices()
        {
            return _plugins.Select(p => p.Plugin)
                .SelectMany(p => p.GetApplicationServices() ?? new List<ApplicationServiceInfo>())
                .ToList();
        }

        /// <inheritdoc cref="IPluginProvider.GetDbStructurePaths"/>
        public Dictionary<Guid, string> GetDbStructurePaths()
        {
            return _plugins.Where(p => !p.Plugin.TenantDbStructureFile.IsNullOrWhiteSpace())
                .ToDictionary(p => p.Plugin.Id, 
                    p => Path.Combine(p.FolderPath, p.Plugin.TenantDbStructureFile));
        }

        /// <inheritdoc cref="IPluginProvider.GetPermissions"/>
        public List<Permission> GetPermissions()
        {
            return _plugins.Select(p => p.Plugin)
                .SelectMany(p => p.GetPermissions() ?? new List<Permission>())
                .ToList();
        }

        /// <inheritdoc cref="IPluginProvider.GetReplacedUseCaseInterfaceTypes"/>
        public List<(Guid PluginId, Dictionary<Type, Type> ReplacedTypes)> GetReplacedUseCaseInterfaceTypes()
        {
            return _plugins.Select(p => (PluginId: p.Plugin.Id, ReplacedTypes: p.Plugin.GetReplacedUseCaseInterfaceTypes()))
                .Where(v => null != v.ReplacedTypes && v.ReplacedTypes.Any())
                .ToList();
        }

        /// <inheritdoc cref="IPluginProvider.GetReplacedUseCaseInterfaceTypesForTenant"/>
        public List<(Guid PluginId, Dictionary<Type, Type> ReplacedTypes)> GetReplacedUseCaseInterfaceTypesForTenant()
        {
            return _plugins.Select(p => (PluginId: p.Plugin.Id, ReplacedTypes: p.Plugin.GetReplacedUseCaseInterfaceTypesForTenant()))
                .Where(v => null != v.ReplacedTypes && v.ReplacedTypes.Any())
                .ToList();
        }

        /// <inheritdoc cref="IPluginProvider.GetSecurityDbStructurePaths"/>
        public Dictionary<Guid, string> GetSecurityDbStructurePaths()
        {
            return _plugins.Where(p => !p.Plugin.SecurityDbStructureFile.IsNullOrWhiteSpace())
                .ToDictionary(p => p.Plugin.Id,
                    p => Path.Combine(p.FolderPath, p.Plugin.SecurityDbStructureFile));
        }

        /// <inheritdoc cref="IPluginProvider.GetSqlRepositoryPaths"/>
        public Dictionary<Guid, string> GetSqlRepositoryPaths()
        {
            return _plugins.Where(p => !p.Plugin.SqlRepositoryFolder.IsNullOrWhiteSpace())
                .ToDictionary(p => p.Plugin.Id,
                    p => Path.Combine(p.FolderPath, p.Plugin.SqlRepositoryFolder));
        }

    }
}
