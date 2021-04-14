using A5Soft.A5App.Domain.Properties;

namespace A5Soft.A5App.Domain.DataAnnotations
{
    /// <summary>
    /// numeric range (double of int only) validation
    /// </summary>
    public class ValueRangeAttribute : A5Soft.CARMA.Domain.Rules.DataAnnotations.CommonRules.RangeAttribute
    {
        /// <inheritdoc />
        public ValueRangeAttribute(double minimum, double maximum) : base(minimum, maximum)
        {
            ErrorMessageResourceType = typeof(Resources);
        }

        /// <inheritdoc />
        public ValueRangeAttribute(int minimum, int maximum) : base(minimum, maximum)
        {
            ErrorMessageResourceType = typeof(Resources);
        }

        /// <inheritdoc />
        public ValueRangeAttribute(double threshold, bool isMin) : base(threshold, isMin)
        {
            ErrorMessageResourceType = typeof(Resources);
        }

        /// <inheritdoc />
        public ValueRangeAttribute(int threshold, bool isMin) : base(threshold, isMin)
        {
            ErrorMessageResourceType = typeof(Resources);
        }

        protected override string ErrorMessageResourceNameForRange 
            => nameof(Resources.DataAnnotations_ErrorMessage_Range_ForRange);
        protected override string ErrorMessageResourceNameForMin 
            => nameof(Resources.DataAnnotations_ErrorMessage_Range_ForMin);
        protected override string ErrorMessageResourceNameForMax 
            => nameof(Resources.DataAnnotations_ErrorMessage_Range_ForMax);

        
    }
}
