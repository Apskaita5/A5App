using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules;
using System;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// Property values equal rule.
    /// </summary> 
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EqualToAttribute : EqualToAttributeBase
    {
        /// <inheritdoc />
        public EqualToAttribute(string referenceProperty) : base(referenceProperty)
        { }

        protected override string GetLocalizedErrorMessageFor(string localizedPropName, string localizedOtherPropName)
        {
            return string.Format(Resources.DataAnnotations_ErrorMessage_EqualTo, 
                localizedPropName, localizedOtherPropName);
        }
    }
}
