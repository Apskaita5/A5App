using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// a user request to reset his password
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_ResetPasswordRequest_DisplayNameForNew))]
    [Serializable]
    public class ResetPasswordRequest
    {

        /// <summary>
        /// A user email that is used as the app account id.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
            Description = nameof(Resources.Security_ResetPasswordRequest_Email_Description),
            Name = nameof(Resources.Security_ResetPasswordRequest_Email_Name),
            ShortName = nameof(Resources.Security_ResetPasswordRequest_Email_ShortName),
            Prompt = nameof(Resources.Security_ResetPasswordRequest_Email_Prompt))]
        [ValueRequired]
        [Email(AllowMultipleAddresses = false)]
        public string Email { get; set; } = string.Empty;

    }
}
