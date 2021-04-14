using System;
using A5Soft.A5App.Domain.Core;
using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Domain
{
    /// <summary>
    /// an exception thrown when a domain entity or singleton data is invalid
    /// </summary>
    [Serializable]
    public class ValidationException : BusinessException
    {
        /// <inheritdoc />
        public ValidationException(BrokenRulesTreeNode brokenRulesTree)
            : base(string.Format(Resources.ValidationException_Message, 
                brokenRulesTree?.EntityDisplayName, brokenRulesTree?.ToString()))
        {
            BrokenRulesTree = brokenRulesTree ?? throw new ArgumentNullException(nameof(brokenRulesTree));
        }

        /// <inheritdoc />
        public ValidationException(BrokenRulesCollection brokenRules)
            : base(brokenRules?.ToString())
        {
            BrokenRules = brokenRules ?? throw new ArgumentNullException(nameof(brokenRules));
        }

        /// <inheritdoc />
        public ValidationException(string errorMessage)
            : base(errorMessage) { }


        public BrokenRulesTreeNode BrokenRulesTree { get; }

        public BrokenRulesCollection BrokenRules { get; }

    }
}
