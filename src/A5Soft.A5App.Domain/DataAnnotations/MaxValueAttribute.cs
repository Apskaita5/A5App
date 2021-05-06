using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules;
using System;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// Maximum value rule for numeric values.
    /// </summary> 
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MaxValueAttribute : MaxValueAttributeBase
    {
        /// <inheritdoc />
        public MaxValueAttribute(double maxValue, int digits = 2, RuleSeverity severity = RuleSeverity.Error) 
            : base(maxValue, digits, severity) { }

        /// <inheritdoc />
        public MaxValueAttribute(int maxValue, RuleSeverity severity = RuleSeverity.Error) 
            : base(maxValue, severity) { }

        protected override string GetLocalizedErrorMessageFor(string localizedPropName)
        {
            string value;
            if (this.Maximum is double dblValue)
            {
                value = dblValue.ToString("#.".PadRight(this.Digits, '0'));
            }
            else value = this.Maximum.ToString();

            return string.Format(Resources.DataAnnotations_ErrorMessage_MaxValueAttribute, 
                localizedPropName, value);
        }
    }
}
