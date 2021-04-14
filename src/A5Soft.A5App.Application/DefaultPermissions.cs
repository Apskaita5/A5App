using A5Soft.A5App.Domain.Security;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using A5Soft.CARMA.Domain.Reflection;

namespace A5Soft.A5App.Application
{
    /// <summary>
    /// Manages permissions for default use case implementations.
    /// </summary>
    public static class DefaultPermissions
    {
        private static readonly List<Permission> _permissions = InitPermissions();

        private static List<Permission> InitPermissions()
        {
            return typeof(DefaultPermissions).Assembly.GetTypes()
                .Where(t => typeof(Permission).IsAssignableFrom(t))
                .Select(t => (Permission)ObjectActivator.CreateInstance(t))
                .ToList();
        }


        public static ReadOnlyCollection<Permission> AllPermissions 
            => _permissions.AsReadOnly();

        
        

    }
}
