using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MaxLengthAttribute = A5Soft.A5App.Domain.DataAnnotations.MaxLengthAttribute;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// a user role used to simplify user permission management based on a role that user plays in the tenant company
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_IUserRole_DisplayNameForNew),
    NameForOld = nameof(Resources.Security_IUserRole_DisplayNameForOld),
    NameForCreateNew = nameof(Resources.Security_IUserRole_DisplayNameForCreateNew))]
    public interface IUserRole : IAuditableEntity
    {

        /// <summary>
        /// A human readable name that is displayed in a lookup for a role assignment.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
        Description = nameof(Resources.Security_IUserRole_Name_Description),
            Name = nameof(Resources.Security_IUserRole_Name_Name),
            ShortName = nameof(Resources.Security_IUserRole_Name_ShortName),
            Prompt = nameof(Resources.Security_IUserRole_Name_Prompt))]
        [ValueRequired]
        [MaxLength(127)]
        string Name { get; }

        /// <summary>
        /// A description of the role that is displayed as a tooltip for lookup values.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_IUserRole_Description_Description),
            Name = nameof(Resources.Security_IUserRole_Description_Name),
            ShortName = nameof(Resources.Security_IUserRole_Description_ShortName),
            Prompt = nameof(Resources.Security_IUserRole_Description_Prompt))]
        [ValueRequired]
        [MaxLength(255)]
        string Description { get; }

        /// <summary>
        /// A number of users (excluding disabled) that have this role.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
            Description = nameof(Resources.Security_IUserRole_UserCount_Description),
            Name = nameof(Resources.Security_IUserRole_UserCount_Name),
            ShortName = nameof(Resources.Security_IUserRole_UserCount_ShortName))]
        int UserCount { get; }

        /// <summary>
        /// A list of permissions that will be assigned to a user for this role.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 3,
        Description = nameof(Resources.Security_IUserRole_Permissions_Description),
            Name = nameof(Resources.Security_IUserRole_Permissions_Name),
            ShortName = nameof(Resources.Security_IUserRole_Permissions_ShortName),
            Prompt = nameof(Resources.Security_IUserRole_Permissions_Prompt))]
        IList<IUserRolePermission> Permissions { get; }
              
    }

}
