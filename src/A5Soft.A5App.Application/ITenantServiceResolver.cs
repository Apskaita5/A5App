using System;
using System.Collections.Generic;
using System.Text;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application
{
    /// <summary>
    /// Resolves tenant scoped service implementation for a particular tenant.
    /// (subject to the plugins installed for the tenant)
    /// </summary>
    [Service(ServiceLifetime.Transient)]
    public interface ITenantServiceResolver
    {
        Type Resolve(Type interfaceType);
    }
}
