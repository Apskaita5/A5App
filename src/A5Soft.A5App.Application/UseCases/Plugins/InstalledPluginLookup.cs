using System;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Application.UseCases.Plugins
{
    /// <summary>
    /// a lookup for basic info about a plugin installed for a particular tenant
    /// </summary>
    [Serializable]
    public class InstalledPluginLookup
    {
        protected Guid _id;
        protected string _name;
        protected string _version;

        
        protected InstalledPluginLookup() { }


        /// <summary>
        /// <see cref="PluginInfo.Id"/>
        /// </summary>
        [Key]
        public Guid Id => _id;

        /// <summary>
        /// <see cref="PluginInfo.Name"/>
        /// </summary>
        public string Name => _name;
        
        /// <summary>
        /// <see cref="PluginInfo.Version"/>
        /// </summary>
        public string Version => _version;


        public override string ToString()
        {
            return $"{_id: N}: {_name} (v. {_version})";
        }

    }
}
