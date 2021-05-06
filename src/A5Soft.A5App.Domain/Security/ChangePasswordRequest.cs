using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using System;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// a user request to change his password
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_ChangePasswordRequest_DisplayNameForNew))]
    [Serializable]
    public class ChangePasswordRequest
    {

        /// <summary>
        /// Current user password.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
            Description = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_CurrentPassword_Description),
            Name = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_CurrentPassword_Name),
            ShortName = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_CurrentPassword_ShortName),
            Prompt = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_CurrentPassword_Prompt))]
        [ValueRequired]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// New user password.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_NewPassword_Description),
            Name = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_NewPassword_Name),
            ShortName = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_NewPassword_ShortName),
            Prompt = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_NewPassword_Prompt))]
        [ValueRequired]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Repeated new user password.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
        Description = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_RepeatedPassword_Description),
            Name = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_RepeatedPassword_Name),
            ShortName = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_RepeatedPassword_ShortName),
            Prompt = nameof(Resources.Security_ChangePasswordRequestQueryCriteria_RepeatedPassword_Prompt))]
        [DataType(DataType.Password)]
        [EqualTo(nameof(NewPassword))]
        public string RepeatedPassword { get; set; } = string.Empty;

    }
}
