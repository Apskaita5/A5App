using System;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// Abstracts permission claim required for either base or plugin functionality and provides localization point.
    /// Every actual permission shall be a descendant of this type, i.e. one type per permission.
    /// Every actual permission shall have a public parameterless constructor. 
    /// </summary>
    public abstract class Permission
    {

        /// <summary>
        /// an ID of the permission
        /// </summary>
        public abstract Guid Id { get; }

        /// <summary>
        /// an ID of the plugin, if the permission is defined by a plugin
        /// </summary>
        public abstract Guid? PluginId { get; }

        /// <summary>
        /// a display order of the permission within total list or a group (if any)
        /// </summary>
        public abstract int Order { get; }

        /// <summary>
        /// a value indicating whether a suspended user can still use this permission
        /// </summary>
        public abstract bool AllowForSuspendedUser { get; }
               
        /// <summary>
        /// Gets a localized name of the permission for use in GUI.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets a localized group name of the permission for use in GUI.
        /// </summary>
        public abstract string GroupName { get; }

        /// <summary>
        /// Gets a localized module name of the permission for use in GUI.
        /// </summary>
        public abstract string ModuleName { get; }

        /// <summary>
        /// Gets a localized description of the permission for use in GUI.
        /// </summary>
        public abstract string Description { get; }
         
    }
}
