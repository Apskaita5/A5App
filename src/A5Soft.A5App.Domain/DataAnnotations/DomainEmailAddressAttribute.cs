

using A5Soft.A5App.Domain.Properties;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// email address rule
    /// </summary>
    public class DomainEmailAddressAttribute : A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules.EmailAddressAttribute
    {
        /// <inheritdoc />
        public DomainEmailAddressAttribute()
        {
            ErrorMessageResourceType = typeof(Resources);
            ErrorMessageResourceName = nameof(Resources.DataAnnotations_ErrorMessage_EmailAddress);
        }
    }
}
