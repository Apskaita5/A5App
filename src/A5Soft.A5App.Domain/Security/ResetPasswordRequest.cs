using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// a user request to reset his password
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_ResetPasswordRequestQueryCriteria_DisplayNameForNew))]
    [Serializable]
    public class ResetPasswordRequest
    {

        /// <summary>
        /// A user email that is used as the app account id.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
            Description = nameof(Resources.Security_ResetPasswordRequestQueryCriteria_Email_Description),
            Name = nameof(Resources.Security_ResetPasswordRequestQueryCriteria_Email_Name),
            ShortName = nameof(Resources.Security_ResetPasswordRequestQueryCriteria_Email_ShortName),
            Prompt = nameof(Resources.Security_ResetPasswordRequestQueryCriteria_Email_Prompt))]
        [ValueRequired]
        [DomainEmailAddress(AllowMultipleAddresses = false)]
        public string Email { get; set; } = string.Empty;

    }
}
