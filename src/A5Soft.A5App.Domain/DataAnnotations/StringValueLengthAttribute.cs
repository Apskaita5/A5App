using A5Soft.A5App.Domain.Properties;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    public class StringValueLengthAttribute : A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules.StringLengthAttribute
    {
        /// <inheritdoc />
        public StringValueLengthAttribute(int maximumLength) : base(maximumLength)
        {
            ErrorMessageResourceType = typeof(Resources);
        }

        /// <inheritdoc />
        public StringValueLengthAttribute(int minimumLength, int maximumLength) : base(minimumLength, maximumLength)
        {
            ErrorMessageResourceType = typeof(Resources);
        }

        protected override string ErrorMessageResourceNameForMaxLength =>
            nameof(Resources.DataAnnotations_ErrorMessage_StringLength_ForMax);
        protected override string ErrorMessageResourceNameForRange =>
            nameof(Resources.DataAnnotations_ErrorMessage_StringLength_ForRange);
        protected override string ErrorMessageResourceNameForExact =>
            nameof(Resources.DataAnnotations_ErrorMessage_StringLength_ForExact);

    }
}
