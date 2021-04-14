using A5Soft.A5App.Domain.Properties;
using System.ComponentModel.DataAnnotations;
using A5Soft.A5App.Domain.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// a user group, used to enable app as a service for a client
    /// that needs to manage its own tenants and users
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_IUserGroup_DisplayNameForNew),
        NameForOld = nameof(Resources.Security_IUserGroup_DisplayNameForOld),
        NameForCreateNew = nameof(Resources.Security_IUserGroup_DisplayNameForCreateNew))]
    public interface IUserGroup : IAuditableEntity
    {
        /// <summary>
        /// a name of the user group
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0, 
            Description = nameof(Resources.Security_IUserGroup_GroupName_Description),
            Name = nameof(Resources.Security_IUserGroup_GroupName_Name),
            ShortName = nameof(Resources.Security_IUserGroup_GroupName_ShortName),
            Prompt = nameof(Resources.Security_IUserGroup_GroupName_Prompt))]
        [ValueRequired]
        [StringValueLength(127)]
        string GroupName { get; }

        /// <summary>
        /// a max users count per group
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
            Description = nameof(Resources.Security_IUserGroup_MaxUsers_Description),
            Name = nameof(Resources.Security_IUserGroup_MaxUsers_Name),
            ShortName = nameof(Resources.Security_IUserGroup_MaxUsers_ShortName),
            Prompt = nameof(Resources.Security_IUserGroup_MaxUsers_Prompt))]
        [ValueRange(1, UserGroup.AbsoluteMaxUsers)]
        int MaxUsers { get; }

        /// <summary>
        /// current user count per group
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 3,
            Description = nameof(Resources.Security_IUserGroup_UserCount_Description),
            Name = nameof(Resources.Security_IUserGroup_UserCount_Name),
            ShortName = nameof(Resources.Security_IUserGroup_UserCount_ShortName))]
        int UserCount { get; }

        /// <summary>
        /// a max tenants count per group
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
            Description = nameof(Resources.Security_IUserGroup_MaxTenants_Description),
            Name = nameof(Resources.Security_IUserGroup_MaxTenants_Name),
            ShortName = nameof(Resources.Security_IUserGroup_MaxTenants_ShortName),
            Prompt = nameof(Resources.Security_IUserGroup_MaxTenants_Prompt))]
        [ValueRange(1, UserGroup.AbsoluteMaxTenants)]
        int MaxTenants { get; }

    }
}