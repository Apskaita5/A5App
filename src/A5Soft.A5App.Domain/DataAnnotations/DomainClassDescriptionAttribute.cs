using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain.Metadata.DataAnnotations;
using System;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// Allows to decorate a domain class with localizable values that could be used
    /// for entity (edit) form captions, headers and menu items for create a new instance action
    /// as well as help uri for the respective help file/resource topic.
    /// </summary>
    /// <remarks>Inherited class is obviously a different entity,
    /// therefore this attribute can only be used either on concrete (final) business entity (class)
    /// or on business entity interface used for business metadata
    /// (that inherits <see cref="A5Soft.CARMA.Domain.IDomainObject"/>).</remarks>  
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface,
        AllowMultiple = false, Inherited = false)]
    public class DomainClassDescriptionAttribute : ClassDescriptionAttribute
    {
        /// <inheritdoc />
        public DomainClassDescriptionAttribute()
        {
            ResourceType = typeof(Resources);
        }
    }
}
