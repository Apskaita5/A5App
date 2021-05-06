using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules;
using System;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// date sanity rule, i.e. not in future and not too far in past (20 years or more)
    /// </summary> 
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateSanityAttribute : DateSanityAttributeBase
    {
        /// <inheritdoc />
        public DateSanityAttribute(RuleSeverity severity = RuleSeverity.Warning) : base(severity)
        { }

        protected override string GetInFutureLocalizedErrorMessageFor(string localizedPropName)
        {
            return string.Format(Resources.DataAnnotations_ErrorMessage_DateSanity_InFuture, localizedPropName);
        }

        protected override string GetTooOldLocalizedErrorMessageFor(string localizedPropName)
        {
            return string.Format(Resources.DataAnnotations_ErrorMessage_DateSanity_TooOld, localizedPropName);
        }
    }
}
