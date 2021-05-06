using A5Soft.A5App.Domain.Properties;
using System;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// at least one property required rule
    /// </summary>  
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AtLeastOneRequiredAttribute : CARMA.Domain.Rules.DataAnnotations.CommonRules.AtLeastOneRequiredAttributeBase
    {
        /// <inheritdoc />
        public AtLeastOneRequiredAttribute(string referenceProperty) : base(referenceProperty)
        { }

        protected override string GetLocalizedErrorMessageFor(string localizedPropName, string localizedOtherPropName)
        {
            return string.Format(Resources.DataAnnotations_ErrorMessage_AtLeastOneRequired, 
                localizedPropName, localizedOtherPropName);
        }
    }
}
