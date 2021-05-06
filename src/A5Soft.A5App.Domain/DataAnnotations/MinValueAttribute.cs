using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules;
using System;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// Minimum value rule for numeric values.
    /// </summary> 
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MinValueAttribute : MinValueAttributeBase
    {
        /// <inheritdoc />
        public MinValueAttribute(double minValue, int digits = 2, RuleSeverity severity = RuleSeverity.Error) 
            : base(minValue, digits, severity) { }

        /// <inheritdoc />
        public MinValueAttribute(int minValue, RuleSeverity severity = RuleSeverity.Error) 
            : base(minValue, severity) { }

        protected override string GetLocalizedErrorMessageFor(string localizedPropName)
        {
            string value;
            if (this.Minimum is double dblValue)
            {
                value = dblValue.ToString("#.".PadRight(this.Digits, '0'));
            }
            else value = this.Minimum.ToString();

            return string.Format(Resources.DataAnnotations_ErrorMessage_MinValueAttribute,
                localizedPropName, value);
        }
    }
}
