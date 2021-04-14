using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// A user request to login to an app server.
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_LoginRequestQueryCriteria_DisplayNameForNew))]
    [Serializable]
    public class LoginRequest
    {

        /// <summary>
        /// An email of the user that is used as a user account id.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
        Description = nameof(Resources.Security_LoginRequestQueryCriteria_Email_Description),
            Name = nameof(Resources.Security_LoginRequestQueryCriteria_Email_Name),
            ShortName = nameof(Resources.Security_LoginRequestQueryCriteria_Email_ShortName),
            Prompt = nameof(Resources.Security_LoginRequestQueryCriteria_Email_Prompt))]
        [ValueRequired]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User password.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_LoginRequestQueryCriteria_Password_Description),
            Name = nameof(Resources.Security_LoginRequestQueryCriteria_Password_Name),
            ShortName = nameof(Resources.Security_LoginRequestQueryCriteria_Password_ShortName),
            Prompt = nameof(Resources.Security_LoginRequestQueryCriteria_Password_Prompt))]
        [ValueRequired]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }

}
