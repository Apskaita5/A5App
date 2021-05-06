using A5Soft.CARMA.Domain.Metadata.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using System;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// Allows to decorate a class method with localizable values that could be used
    /// for action button tooltip.
    /// </summary>
    /// <remarks>Methods on business entities are used for (internal) business calculations
    /// only, as any interaction with "external world" is managed by use cases.
    /// Therefore method descriptions can only be applied to class methods (incl. inherited).
    /// They cannot be defined on business interfaces as their implementation
    /// are subject to the platform used (in rich entity models for winforms vs. javascript for web).</remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DomainMethodDescriptionAttribute : MethodDescriptionAttribute
    {
        /// <inheritdoc />
        public DomainMethodDescriptionAttribute()
        {
            ResourceType = typeof(Resources);
        }
    }
}
