using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules;
using System;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// Minimum string length rule.
    /// </summary> 
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MinLengthAttribute : MinLengthAttributeBase
    {
        /// <inheritdoc />
        public MinLengthAttribute(int minimumLength, RuleSeverity severity = RuleSeverity.Error) 
            : base(minimumLength, severity) { }

        protected override string GetLocalizedErrorMessageFor(string localizedPropName)
        {
            return string.Format(Resources.DataAnnotations_ErrorMessage_MinLengthAttribute, 
                localizedPropName, MinimumLength);
        }
    }
}
