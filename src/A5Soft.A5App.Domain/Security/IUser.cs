using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// a user of the app
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_IUser_DisplayNameForNew),
    NameForOld = nameof(Resources.Security_IUser_DisplayNameForOld),
    NameForCreateNew = nameof(Resources.Security_IUser_DisplayNameForCreateNew))]
    public interface IUser : IAuditableEntity
    {

        /// <summary>
        /// A given name of the user.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
        Description = nameof(Resources.Security_IUser_Name_Description),
            Name = nameof(Resources.Security_IUser_Name_Name),
            ShortName = nameof(Resources.Security_IUser_Name_ShortName),
            Prompt = nameof(Resources.Security_IUser_Name_Prompt))]
        [ValueRequired]
        [StringValueLength(255)]
        string Name { get; }

        /// <summary>
        /// An email of the user that is also used as a user name for login.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_IUser_Email_Description),
            Name = nameof(Resources.Security_IUser_Email_Name),
            ShortName = nameof(Resources.Security_IUser_Email_ShortName),
            Prompt = nameof(Resources.Security_IUser_Email_Prompt))]
        [ValueRequired]
        [DomainEmailAddress]
        [StringValueLength(255)]
        string Email { get; }

        /// <summary>
        /// A phone No of the user.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
        Description = nameof(Resources.Security_IUser_Phone_Description),
            Name = nameof(Resources.Security_IUser_Phone_Name),
            ShortName = nameof(Resources.Security_IUser_Phone_ShortName),
            Prompt = nameof(Resources.Security_IUser_Phone_Prompt))]
        [StringValueLength(255)]
        string Phone { get; }

        /// <summary>
        /// Administrative role of the user.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 3,
        Description = nameof(Resources.Security_IUser_AdminRole_Description),
            Name = nameof(Resources.Security_IUser_AdminRole_Name),
            ShortName = nameof(Resources.Security_IUser_AdminRole_ShortName),
            Prompt = nameof(Resources.Security_IUser_AdminRole_Prompt))]
        [EnumDataType(typeof(AdministrativeRole))]
        AdministrativeRole AdminRole { get; }

        /// <summary>
        /// An user group that the user belongs to (if any).
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 4,
        Description = nameof(Resources.Security_IUser_UserGroup_Description),
            Name = nameof(Resources.Security_IUser_UserGroup_Name),
            ShortName = nameof(Resources.Security_IUser_UserGroup_ShortName),
            Prompt = nameof(Resources.Security_IUser_UserGroup_Prompt))]
        IDomainEntityIdentity UserGroupId { get; }

        /// <summary>
        /// Whether the user is (temporally) suspended. Suspended users only have readonly privileges
        /// (irrespective of their configured roles).
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 5,
        Description = nameof(Resources.Security_IUser_IsSuspended_Description),
            Name = nameof(Resources.Security_IUser_IsSuspended_Name),
            ShortName = nameof(Resources.Security_IUser_IsSuspended_ShortName))]
        bool IsSuspended { get; }

        /// <summary>
        /// Whether the user is disabled, i.e. no longer in use, only left as a historic record.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 6,
        Description = nameof(Resources.Security_IUser_IsDisabled_Description),
            Name = nameof(Resources.Security_IUser_IsDisabled_Name),
            ShortName = nameof(Resources.Security_IUser_IsDisabled_ShortName))]
        bool IsDisabled { get; }

        /// <summary>
        /// Whether to require two factor authentication for the user.
        /// E.g. when a particular user works from an unsafe environment.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 7,
        Description = nameof(Resources.Security_IUser_TwoFactorEnabled_Description),
            Name = nameof(Resources.Security_IUser_TwoFactorEnabled_Name),
            ShortName = nameof(Resources.Security_IUser_TwoFactorEnabled_ShortName))]
        bool TwoFactorEnabled { get; }

        /// <summary>
        /// Configure user roles per tenant (for users with no administrative privileges).
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 8,
        Description = nameof(Resources.Security_IUser_RolesForTenants_Description),
            Name = nameof(Resources.Security_IUser_RolesForTenants_Name),
            ShortName = nameof(Resources.Security_IUser_RolesForTenants_ShortName),
            Prompt = nameof(Resources.Security_IUser_RolesForTenants_Prompt))]
        IList<IUserTenant> RolesForTenants { get; }

    }
}
