

using A5Soft.A5App.Domain.Properties;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// date sanity rule, i.e. not in future and not too far in past (20 years or more)
    /// </summary>
    public class DateSanityAttribute : A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules.DateSanityAttribute
    {
        /// <inheritdoc />
        public DateSanityAttribute()
        {
            ErrorMessageResourceType = typeof(Resources);
            ErrorMessageResourceName = nameof(Resources.DataAnnotations_ErrorMessage_DateSanity);
        }
    }
}
