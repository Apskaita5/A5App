using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules;
using System;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// required rule
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ValueRequiredAttribute : RequiredAttributeBase
    {
        /// <inheritdoc />
        public ValueRequiredAttribute() { }

        protected override string GetLocalizedErrorMessageFor(string localizedPropName)
        {
            return string.Format(Resources.DataAnnotations_ErrorMessage_Required, localizedPropName);
        }
    }
}
