using A5Soft.A5App.Domain.Security;
using System;
using System.Collections.Generic;
using A5Soft.CARMA.Application.Navigation;

namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// An interface that shall be implemented by a plugin class that provides integration interface with the app.
    /// </summary>
    public interface IPlugin
    {

        /// <summary>
        /// an id of the plugin 
        /// </summary> 
        /// <remarks>required</remarks>
        Guid Id { get; }

        /// <summary>
        /// a name of the plugin
        /// </summary> 
        /// <remarks>required</remarks>
        string Name { get; }

        /// <summary>
        /// a version of the plugin
        /// </summary>
        /// <remarks>required</remarks>
        string Version { get; }
            
        /// <summary>
        /// path to the folder where SQL repository files are located (relative to the plugin root folder)
        /// </summary>
        /// <remarks>could be null or empty if the plugin does not require SQL queries</remarks>
        string SqlRepositoryFolder { get; }

        /// <summary>
        /// path to the file for tenant database structure extension data (relative to the plugin root folder)
        /// </summary> 
        /// <remarks>could be null or empty if the plugin does not require tenant database extension (extra tables)</remarks>
        string TenantDbStructureFile { get; }

        /// <summary>
        /// path to the file for security database structure extension data (relative to the plugin root folder)
        /// </summary> 
        /// <remarks>could be null or empty if the plugin does not require security database extension (extra tables)</remarks>
        string SecurityDbStructureFile { get; }


        /// <summary>
        /// Gets a list of the application services implemented by the plugin.
        /// </summary>
        List<ApplicationServiceInfo> GetApplicationServices();

        /// <summary>
        /// Gets a list of plugin permissions (that are required to configure user access to plugin features).
        /// </summary> 
        /// <remarks>could be null or empty if the plugin does not require extra security permissions</remarks>
        List<Permission> GetPermissions();

        /// <summary>
        /// Gets a list of non tenant related menu items that are added by the plugin to the application main menu.
        /// </summary> 
        /// <remarks>could be null or empty if the plugin does not add any non tenant related menu items</remarks>
        List<(string MenuGroupName, MenuItem MenuItem)> GetApplicationMenuItems();

        /// <summary>
        /// Gets a list of tenant related menu items that are added by the plugin to the application main menu.
        /// </summary> 
        /// <remarks>could be null or empty if the plugin does not add any tenant related menu items</remarks>
        List<(string MenuGroupName, MenuItem MenuItem)> GetApplicationMenuItemsForTenant();

        /// <summary>
        /// Gets a dictionary of non tenant related use case interface replacements for main menu.
        /// E.g. if plugin implements custom user management and wants to "reuse" builtin menu items
        /// it can replace menu item use case IQueryUsersUseCase with ICustomQueryUsersUseCase.
        /// </summary>  
        /// <remarks>could be null or empty if the plugin does not replace non tenant related use cases for main menu items</remarks>
        Dictionary<Type, Type> GetReplacedUseCaseInterfaceTypes();

        /// <summary>
        /// Gets a dictionary of tenant related use case interface replacements for main menu.
        /// E.g. if plugin implements custom invoices and wants to "reuse" builtin menu items
        /// it can replace menu item use case IQueryInvoicesUseCase with ICustomQueryInvoicesUseCase.
        /// </summary>  
        /// <remarks>could be null or empty if the plugin does not replace tenant related use cases for main menu items</remarks>
        Dictionary<Type, Type> GetReplacedUseCaseInterfaceTypesForTenant();

    }
}
