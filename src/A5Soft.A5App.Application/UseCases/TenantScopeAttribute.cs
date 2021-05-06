using System;

namespace A5Soft.A5App.Application.UseCases
{
    /// <summary>
    /// Marks a service that is always executed in context of a particular tenant.
    /// </summary>
    /// <remarks>Used to identify services that could be overriden by plugins
    /// that are installed for a particular tenant.</remarks>
    internal class TenantScopeAttribute : Attribute
    {
    }
}
