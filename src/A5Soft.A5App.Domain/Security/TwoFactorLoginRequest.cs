using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// A user request to login to an app server.
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_TwoFactorLoginRequest_DisplayNameForNew))]
    [Serializable]
    public class TwoFactorLoginRequest
    {

        /// <summary>
        /// An email of the user that is used as a user account id.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
            Description = nameof(Resources.Security_TwoFactorLoginRequest_Token_Description),
            Name = nameof(Resources.Security_TwoFactorLoginRequest_Token_Name),
            ShortName = nameof(Resources.Security_TwoFactorLoginRequest_Token_ShortName),
            Prompt = nameof(Resources.Security_TwoFactorLoginRequest_Token_Prompt))]
        [ValueRequired]
        public string Token { get; set; } = string.Empty;

    }
}
