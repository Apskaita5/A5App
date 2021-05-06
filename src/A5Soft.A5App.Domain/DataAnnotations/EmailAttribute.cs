using A5Soft.A5App.Domain.Properties;
using System;
using A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// email address rule
    /// </summary> 
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EmailAttribute : EmailAddressAttributeBase
    {
        /// <inheritdoc />
        public EmailAttribute() : base()
        { }

        protected override string GetLocalizedErrorMessageFor(string localizedPropName)
        {
            return string.Format(Resources.DataAnnotations_ErrorMessage_EmailAddress, localizedPropName);
        }
    }
}
