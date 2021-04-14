using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace A5Soft.A5App.Domain.Security.Queries
{
    /// <summary>
    /// 
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.UserQueryResult_DisplayNameForNew))]
    [Serializable]
    public class UserQueryResult
    {
        protected IDomainEntityIdentity _id;
        protected IDomainEntityIdentity _groupId;
        protected string _groupName;
        protected string _name;
        protected string _email;
        protected string _phone;
        protected bool _isSuspended;
        protected bool _isDisabled;
        protected bool _twoFactorEnabled;
        protected AdministrativeRole _adminRole;
        protected string _roles;
        protected int _tenantCount;
        protected DateTime _insertedAt;
        protected string _insertedBy;
        protected DateTime _updatedAt;
        protected string _updatedBy;


        protected UserQueryResult() { }


        /// <inheritdoc cref="IUser.Id"/>
        [Key]
        public IDomainEntityIdentity Id => _id;

        /// <inheritdoc cref="IUser.UserGroupId"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 3,
            Description = nameof(Resources.Security_IUser_UserGroup_Description),
            Name = nameof(Resources.Security_IUser_UserGroup_Name),
            ShortName = nameof(Resources.Security_IUser_UserGroup_ShortName))]
        [ScaffoldColumn(false)]
        public IDomainEntityIdentity GroupId => _groupId;

        /// <inheritdoc cref="IUser.UserGroupId"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 3,
            Description = nameof(Resources.Security_IUser_UserGroup_Description),
            Name = nameof(Resources.Security_IUser_UserGroup_Name),
            ShortName = nameof(Resources.Security_IUser_UserGroup_ShortName))]
        [ScaffoldColumn(true)]
        public string GroupName => _groupName;

        /// <inheritdoc cref="IUser.Name"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
            Description = nameof(Resources.Security_IUser_Name_Description),
            Name = nameof(Resources.Security_IUser_Name_Name),
            ShortName = nameof(Resources.Security_IUser_Name_ShortName),
            Prompt = nameof(Resources.Security_IUser_Name_Prompt))]
        [ScaffoldColumn(true)]
        public string Name => _name;

        /// <inheritdoc cref="IUser.Email"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 4,
            Description = nameof(Resources.Security_IUser_Email_Description),
            Name = nameof(Resources.Security_IUser_Email_Name),
            ShortName = nameof(Resources.Security_IUser_Email_ShortName))]
        [ScaffoldColumn(true)]
        public string Email => _email;

        /// <inheritdoc cref="IUser.Phone"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 5,
            Description = nameof(Resources.Security_IUser_Phone_Description),
            Name = nameof(Resources.Security_IUser_Phone_Name),
            ShortName = nameof(Resources.Security_IUser_Phone_ShortName))]
        [ScaffoldColumn(true)]
        public string Phone => _phone;

        /// <inheritdoc cref="IUser.IsSuspended"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 6,
            Description = nameof(Resources.Security_IUser_IsSuspended_Description),
            Name = nameof(Resources.Security_IUser_IsSuspended_Name),
            ShortName = nameof(Resources.Security_IUser_IsSuspended_ShortName))]
        [ScaffoldColumn(true)]
        public bool IsSuspended => _isSuspended;

        /// <inheritdoc cref="IUser.IsDisabled"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 7,
            Description = nameof(Resources.Security_IUser_IsDisabled_Description),
            Name = nameof(Resources.Security_IUser_IsDisabled_Name),
            ShortName = nameof(Resources.Security_IUser_IsDisabled_ShortName))]
        [ScaffoldColumn(true)]
        public bool IsDisabled => _isDisabled;

        /// <inheritdoc cref="IUser.TwoFactorEnabled"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 8,
            Description = nameof(Resources.Security_IUser_TwoFactorEnabled_Description),
            Name = nameof(Resources.Security_IUser_TwoFactorEnabled_Name),
            ShortName = nameof(Resources.Security_IUser_TwoFactorEnabled_ShortName))]
        [ScaffoldColumn(true)]
        public bool TwoFactorEnabled => _twoFactorEnabled;

        /// <inheritdoc cref="IUser.AdminRole"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 9,
            Description = nameof(Resources.Security_IUser_AdminRole_Description),
            Name = nameof(Resources.Security_IUser_AdminRole_Name),
            ShortName = nameof(Resources.Security_IUser_AdminRole_ShortName))]
        [ScaffoldColumn(true)]
        public AdministrativeRole AdminRole => _adminRole;

        /// <summary>
        /// Roles that the user has.
        /// </summary>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 9,
        Description = nameof(Resources.UserQueryResult_Roles_Description),
            Name = nameof(Resources.UserQueryResult_Roles_Name),
            ShortName = nameof(Resources.UserQueryResult_Roles_ShortName))]
        [ScaffoldColumn(true)]
        public string Roles => _roles;

        /// <summary>
        /// Number of tenants that the user has access to.
        /// </summary>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 10,
        Description = nameof(Resources.UserQueryResult_TenantCount_Description),
            Name = nameof(Resources.UserQueryResult_TenantCount_Name),
            ShortName = nameof(Resources.UserQueryResult_TenantCount_ShortName))]
        [ScaffoldColumn(true)]
        public int TenantCount => _tenantCount;

        /// <inheritdoc cref="IAuditable.InsertedAt"/>
        [Browsable(true)]
        [Display(AutoGenerateField = false, ResourceType = typeof(Resources), Order = 11,
            Description = nameof(Resources.IAuditableEntity_InsertedAt_Description),
            Name = nameof(Resources.IAuditableEntity_InsertedAt_Name),
            ShortName = nameof(Resources.IAuditableEntity_InsertedAt_ShortName),
            GroupName = nameof(Resources.IAuditableEntity_GroupName))]
        [ScaffoldColumn(true)]
        public DateTime InsertedAt => _insertedAt;

        /// <inheritdoc cref="IAuditable.InsertedBy"/>
        [Browsable(true)]
        [Display(AutoGenerateField = false, ResourceType = typeof(Resources), Order = 12,
            Description = nameof(Resources.IAuditableEntity_InsertedBy_Description),
            Name = nameof(Resources.IAuditableEntity_InsertedBy_Name),
            ShortName = nameof(Resources.IAuditableEntity_InsertedBy_ShortName),
            GroupName = nameof(Resources.IAuditableEntity_GroupName))]
        [ScaffoldColumn(true)]
        public string InsertedBy => _insertedBy;

        /// <inheritdoc cref="IAuditable.UpdatedAt"/>
        [Browsable(true)]
        [Display(AutoGenerateField = false, ResourceType = typeof(Resources), Order = 13,
            Description = nameof(Resources.IAuditableEntity_UpdatedAt_Description),
            Name = nameof(Resources.IAuditableEntity_UpdatedAt_Name),
            ShortName = nameof(Resources.IAuditableEntity_UpdatedAt_ShortName),
            GroupName = nameof(Resources.IAuditableEntity_GroupName))]
        [ScaffoldColumn(true)]
        public DateTime UpdatedAt => _updatedAt;

        /// <inheritdoc cref="IAuditable.UpdatedBy"/>
        [Browsable(true)]
        [Display(AutoGenerateField = false, ResourceType = typeof(Resources), Order = 14,
            Description = nameof(Resources.IAuditableEntity_UpdatedBy_Description),
            Name = nameof(Resources.IAuditableEntity_UpdatedBy_Name),
            ShortName = nameof(Resources.IAuditableEntity_UpdatedBy_ShortName),
            GroupName = nameof(Resources.IAuditableEntity_GroupName))]
        [ScaffoldColumn(true)]
        public string UpdatedBy => _updatedBy;

    }

}
