using System;
using System.Collections.Generic;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application.Navigation;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// base interface for plugin end points
    /// </summary>    
    [Service(ServiceLifetime.Singleton)]
    public interface IPluginProvider
    {
        /// <summary>
        /// Gets a plugin dictionary for plugin folders that contain SQL repositories.
        /// (key - plugin id; value - path to the plugin SQL repositories folder)
        /// </summary>
        Dictionary<Guid, string> GetSqlRepositoryPaths();

        /// <summary>
        /// Gets a plugin dictionary for plugin files that contain database structure.
        /// (key - plugin id; value - path to the plugin database structure file)
        /// </summary>
        Dictionary<Guid, string> GetDbStructurePaths();

        /// <summary>
        /// Gets a plugin dictionary for plugin files that contain security database structure.
        /// (key - plugin id; value - path to the plugin security database structure file)
        /// </summary>
        Dictionary<Guid, string> GetSecurityDbStructurePaths();

        /// <summary>
        /// Gets a list of the application services implemented by the plugins.
        /// </summary>
        List<ApplicationServiceInfo> GetApplicationServices();

        /// <summary>
        /// Gets a list of plugin permissions (that are required to configure user access
        /// to plugin features).
        /// </summary>
        List<Permission> GetPermissions();
                
        /// <summary>
        /// Gets a list of non tenant related menu items that are added by plugins to the application main menu.
        /// </summary>
        List<(Guid PluginId, List<(string MenuGroupName, MenuItem MenuItem)> MenuItems)> GetApplicationMenuItems();

        /// <summary>
        /// Gets a list of tenant related menu items that are added by plugins to the application main menu.
        /// </summary>
        List<(Guid PluginId, List<(string MenuGroupName, MenuItem MenuItem)> MenuItems)> GetApplicationMenuItemsForTenant();

        /// <summary>
        /// Gets a dictionary of non tenant related use case interface replacements for main menu.
        /// E.g. if plugin implements custom user management and wants to "reuse" builtin menu items
        /// it can replace menu item use case IQueryUsersUseCase with ICustomQueryUsersUseCase.
        /// </summary>
        List<(Guid PluginId, Dictionary<Type, Type> ReplacedTypes)> GetReplacedUseCaseInterfaceTypes();

        /// <summary>
        /// Gets a dictionary of tenant related use case interface replacements for main menu.
        /// E.g. if plugin implements custom invoices and wants to "reuse" builtin menu items
        /// it can replace menu item use case IQueryInvoicesUseCase with ICustomQueryInvoicesUseCase.
        /// </summary>
        List<(Guid PluginId, Dictionary<Type, Type> ReplacedTypes)> GetReplacedUseCaseInterfaceTypesForTenant();

    }
}
