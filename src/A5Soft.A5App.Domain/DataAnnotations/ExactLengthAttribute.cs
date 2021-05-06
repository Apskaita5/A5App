using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules;
using System;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// Exact string length rule.
    /// </summary>  
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExactLengthAttribute : ExactLengthAttributeBase
    {
        /// <inheritdoc />
        public ExactLengthAttribute(int length, RuleSeverity severity = RuleSeverity.Error) 
            : base(length, severity) { }

        protected override string GetLocalizedErrorMessageFor(string localizedPropName)
        {
            return string.Format(Resources.DataAnnotations_ErrorMessage_ExactLengthAttribute, 
                localizedPropName, MinimumLength);
        }
    }
}
