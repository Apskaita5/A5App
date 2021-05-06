using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules;
using System;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// Maximum string length rule.
    /// </summary>  
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MaxLengthAttribute : MaxLengthAttributeBase
    {
        /// <inheritdoc />
        public MaxLengthAttribute(int maximumLength, RuleSeverity severity = RuleSeverity.Error) 
            : base(maximumLength, severity) { }

        protected override string GetLocalizedErrorMessageFor(string localizedPropName)
        {
            return string.Format(Resources.DataAnnotations_ErrorMessage_MaxLengthAttribute, 
                localizedPropName, MaximumLength);
        }
    }
}
