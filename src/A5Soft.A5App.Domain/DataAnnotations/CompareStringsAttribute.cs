using A5Soft.A5App.Domain.Properties;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// string values equal rule
    /// </summary>
    public class CompareStringsAttribute : A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules.CompareStringsAttribute
    {
        /// <inheritdoc />
        public CompareStringsAttribute(string referenceProperty) : base(referenceProperty)
        {
            ErrorMessageResourceType = typeof(Resources);
            ErrorMessageResourceName = nameof(Resources.DataAnnotations_ErrorMessage_CompareStrings);
        }
    }
}
