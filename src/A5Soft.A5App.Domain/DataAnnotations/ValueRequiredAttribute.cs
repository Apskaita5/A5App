using A5Soft.A5App.Domain.Properties;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// required rule
    /// </summary>
    public class ValueRequiredAttribute : A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules.RequiredAttribute
    {
        /// <inheritdoc />
        public ValueRequiredAttribute()
        {
            ErrorMessageResourceType = typeof(Resources);
            ErrorMessageResourceName = nameof(Resources.DataAnnotations_ErrorMessage_Required);
        }
    }
}
