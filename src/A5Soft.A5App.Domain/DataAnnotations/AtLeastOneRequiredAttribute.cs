using A5Soft.A5App.Domain.Properties;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// at least one property required rule
    /// </summary>
    public class AtLeastOneRequiredAttribute : CARMA.Domain.Rules.DataAnnotations.CommonRules.AtLeastOneRequiredAttribute
    {
        /// <inheritdoc />
        public AtLeastOneRequiredAttribute(string referenceProperty) : base(referenceProperty)
        {
            ErrorMessageResourceType = typeof(Resources);
            ErrorMessageResourceName = nameof(Resources.DataAnnotations_ErrorMessage_AtLeastOneRequired);
        }
    }
}
