using System;
using System.Collections.Generic;
using A5Soft.A5App.Domain.Security;

namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// base interface for plugin end points
    /// </summary>
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
        /// Gets a list of plugin permissions (that are required to configure user access
        /// to plugin features).
        /// </summary>
        List<Permission> GetPermissions();

        /// <summary>
        /// Gets a list of the app extensions (plugins) installed.
        /// </summary>
        List<ExtensionInfo> GetExtensions();

    }
}
