using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// a permission assignable to a user role
    /// </summary>
    public interface IUserRolePermission : IDomainEntity
    {
        /// <summary>
        /// <see cref="Permission.Id"/>
        /// </summary>
        Guid PermissionId { get; }

        /// <summary>
        /// A name of the assignable permission.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_IUserRolePermission_Name_Description),
            Name = nameof(Resources.Security_IUserRolePermission_Name_Name),
            ShortName = nameof(Resources.Security_IUserRolePermission_Name_ShortName))]
        string Name { get; }

        /// <summary>
        /// A description of the assignable permission.
        /// </summary>
        [Display(AutoGenerateField = false, ResourceType = typeof(Resources), Order = 0,
        Description = nameof(Resources.Security_IUserRolePermission_Description_Description),
            Name = nameof(Resources.Security_IUserRolePermission_Description_Name),
            ShortName = nameof(Resources.Security_IUserRolePermission_Description_ShortName))]
        string Description { get; }

        /// <summary>
        /// A name of the application module that the permission belongs to.
        /// </summary>
        [Display(AutoGenerateField = false, ResourceType = typeof(Resources), Order = 0,
            Description = nameof(Resources.Security_IUserRolePermission_ModuleName_Description),
            Name = nameof(Resources.Security_IUserRolePermission_ModuleName_Name),
            ShortName = nameof(Resources.Security_IUserRolePermission_ModuleName_ShortName))]
        string ModuleName { get; }

        /// <summary>
        /// A name of the permission group that the permission belongs to.
        /// </summary>
        [Display(AutoGenerateField = false, ResourceType = typeof(Resources), Order = 0,
            Description = nameof(Resources.Security_IUserRolePermission_GroupName_Description),
            Name = nameof(Resources.Security_IUserRolePermission_GroupName_Name),
            ShortName = nameof(Resources.Security_IUserRolePermission_GroupName_ShortName))]
        string GroupName { get; }

        /// <summary>
        /// A visible index of the permission within a group.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Whether the user role has the permission.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
            Description = nameof(Resources.Security_IUserRolePermission_Assigned_Description),
            Name = nameof(Resources.Security_IUserRolePermission_Assigned_Name),
            ShortName = nameof(Resources.Security_IUserRolePermission_Assigned_ShortName))]
        bool Assigned { get; }

    }

}
