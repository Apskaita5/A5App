using System;
using System.Linq;
using A5Soft.A5App.Application.UseCases;

namespace A5Soft.A5App.Application
{
    /// <summary>
    /// Info about a required service for DI container.
    /// </summary>
    public sealed class ApplicationServiceInfo : A5Soft.CARMA.Application.ApplicationServiceInfo
    {
        /// <summary>
        /// a built in application service
        /// </summary>
        /// <param name="baseServiceInfo"><see cref="CARMA.Application.IoC"/></param>
        internal ApplicationServiceInfo(CARMA.Application.ApplicationServiceInfo baseServiceInfo) 
            : base(baseServiceInfo.InterfaceType, baseServiceInfo.ImplementationType, baseServiceInfo.Lifetime)
        {
            IsTenantScope = InterfaceType.GetCustomAttributes(typeof(TenantScopeAttribute), true).Any();
        }

        /// <summary>
        /// an application service (implementation) that is defined by a plugin
        /// </summary>
        /// <param name="baseServiceInfo"><see cref="CARMA.Application.IoC"/></param>
        /// <param name="pluginId">an id of the plugin that defines the service (implementation)</param>
        public ApplicationServiceInfo(CARMA.Application.ApplicationServiceInfo baseServiceInfo, Guid pluginId) 
            : this(baseServiceInfo)
        {
            PluginId = pluginId;
        }


        /// <summary>
        /// an id of the plugin that defines the service (implementation);
        /// null for a built in implementation
        /// </summary>
        public Guid? PluginId { get; } = null;

        /// <summary>
        /// whether the service is always executed in context of a particular tenant
        /// and requires implementation type resolution subject to the plugins for the tenant
        /// </summary>
        public bool IsTenantScope { get; }

        /// <summary>
        /// whether to inject the service using <see cref="ITenantServiceResolver"/> as a factory
        /// </summary>
        public bool InjectResolver 
            => IsTenantScope && !PluginId.HasValue;

        /// <summary>
        /// whether to inject service implementation only
        /// </summary>
        /// <remarks>plugin service implementations for tenant scoped services need to be
        /// resolved by <see cref="ITenantServiceResolver"/> which in turn needs access
        /// to implementations as there are (could be) multiple per service</remarks>
        public bool InjectImplementationOnly 
            => IsTenantScope && PluginId.HasValue;

    }
}
