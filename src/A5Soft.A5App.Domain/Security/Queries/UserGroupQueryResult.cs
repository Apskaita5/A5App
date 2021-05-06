using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Domain.Security.Queries
{
    /// <summary>
    /// A description of a user group returned as a query result.
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_UserGroupQueryResult_DisplayNameForNew))]
    [Serializable]
    public class UserGroupQueryResult
    {
        protected IDomainEntityIdentity _id;
        protected string _groupName = string.Empty; 
        protected int _maxUsers; 
        protected int _maxTenants;
        protected DateTime _insertedAt;
        protected string _insertedBy;
        protected DateTime _updatedAt;
        protected string _updatedBy;
        protected int _tenantCount;
        protected int _userCount;


        protected UserGroupQueryResult() { }


        [Key]
        public IDomainEntityIdentity Id => _id;

        /// <inheritdoc cref="IUserGroup.GroupName"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
        Description = nameof(Resources.Security_IUserGroup_GroupName_Description),
            Name = nameof(Resources.Security_IUserGroup_GroupName_Name),
            ShortName = nameof(Resources.Security_IUserGroup_GroupName_ShortName))]
        public string GroupName => _groupName;

        /// <inheritdoc cref="IUserGroup.MaxUsers"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_IUserGroup_MaxUsers_Description),
            Name = nameof(Resources.Security_IUserGroup_MaxUsers_Name),
            ShortName = nameof(Resources.Security_IUserGroup_MaxUsers_ShortName))]
        public int MaxUsers => _maxUsers;

        /// <inheritdoc cref="IUserGroup.MaxTenants"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
        Description = nameof(Resources.Security_IUserGroup_MaxTenants_Description),
            Name = nameof(Resources.Security_IUserGroup_MaxTenants_Name),
            ShortName = nameof(Resources.Security_IUserGroup_MaxTenants_ShortName))]
        public int MaxTenants => _maxTenants;

        /// <inheritdoc cref="IAuditable.InsertedAt"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 4,
        Description = nameof(Resources.IAuditableEntity_InsertedAt_Description),
            Name = nameof(Resources.IAuditableEntity_InsertedAt_Name),
            ShortName = nameof(Resources.IAuditableEntity_InsertedAt_ShortName))]
        public DateTime InsertedAt => _insertedAt;

        /// <inheritdoc cref="IAuditable.InsertedBy"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 5,
        Description = nameof(Resources.IAuditableEntity_InsertedBy_Description),
            Name = nameof(Resources.IAuditableEntity_InsertedBy_Name),
            ShortName = nameof(Resources.IAuditableEntity_InsertedBy_ShortName))]
        public string InsertedBy => _insertedBy;

        /// <inheritdoc cref="IAuditable.UpdatedAt"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 6,
        Description = nameof(Resources.IAuditableEntity_UpdatedAt_Description),
            Name = nameof(Resources.IAuditableEntity_UpdatedAt_Name),
            ShortName = nameof(Resources.IAuditableEntity_UpdatedAt_ShortName))]
        public DateTime UpdatedAt => _updatedAt;

        /// <inheritdoc cref="IAuditable.UpdatedBy"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 7,
        Description = nameof(Resources.IAuditableEntity_UpdatedBy_Description),
            Name = nameof(Resources.IAuditableEntity_UpdatedBy_Name),
            ShortName = nameof(Resources.IAuditableEntity_UpdatedBy_ShortName))]
        public string UpdatedBy => _updatedBy;

        /// <summary>
        /// Actual count of tenants assigned to the group.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 8,
        Description = nameof(Resources.Security_UserGroupQueryResult_TenantCount_Description),
            Name = nameof(Resources.Security_UserGroupQueryResult_TenantCount_Name),
            ShortName = nameof(Resources.Security_UserGroupQueryResult_TenantCount_ShortName))]
        public int TenantCount => _tenantCount;

        /// <inheritdoc cref="IUserGroup.UserCount"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 9,
        Description = nameof(Resources.Security_IUserGroup_UserCount_Description),
            Name = nameof(Resources.Security_IUserGroup_UserCount_Name),
            ShortName = nameof(Resources.Security_IUserGroup_UserCount_ShortName))]
        public int UserCount => _userCount;

        /// <summary>
        /// Whether the user group can be deleted.
        /// </summary>
        public bool CanDelete => _userCount < 1;

    }
}
