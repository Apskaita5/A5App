using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security.Queries
{
    /// <summary>
    /// A description of a user role returned as a query result.
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_UserRoleQueryResult_DisplayNameForNew))]
    [Serializable]
    public class UserRoleQueryResult
    {
        protected IDomainEntityIdentity _id;
        protected string _name;
        protected string _description;
        protected DateTime _insertedAt;
        protected string _insertedBy;
        protected DateTime _updatedAt;
        protected string _updatedBy;
        protected int _userCount;


        protected UserRoleQueryResult() { }


        /// <summary>
        /// an id of the user role
        /// </summary>
        [Key]
        public IDomainEntityIdentity Id => _id;

        /// <inheritdoc cref="IUserRole.Name"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_IUserRole_Name_Description),
            Name = nameof(Resources.Security_IUserRole_Name_Name),
            ShortName = nameof(Resources.Security_IUserRole_Name_ShortName))]
        [ScaffoldColumn(true)]
        public string Name => _name;

        /// <inheritdoc cref="IUserRole.Description"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
        Description = nameof(Resources.Security_IUserRole_Description_Description),
            Name = nameof(Resources.Security_IUserRole_Description_Name),
            ShortName = nameof(Resources.Security_IUserRole_Description_ShortName))]
        [ScaffoldColumn(true)]
        public string Description => _description;

        /// <inheritdoc cref="IUserRole.UserCount"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 7,
            Description = nameof(Resources.Security_IUserRole_UserCount_Description),
            Name = nameof(Resources.Security_IUserRole_UserCount_Name),
            ShortName = nameof(Resources.Security_IUserRole_UserCount_ShortName))]
        [ScaffoldColumn(true)]
        public int UserCount => _userCount;

        /// <inheritdoc cref="IAuditable.InsertedAt"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 3,
        Description = nameof(Resources.IAuditableEntity_InsertedAt_Description),
            Name = nameof(Resources.IAuditableEntity_InsertedAt_Name),
            ShortName = nameof(Resources.IAuditableEntity_InsertedAt_ShortName))]
        [ScaffoldColumn(true)]
        public DateTime InsertedAt => _insertedAt;

        /// <inheritdoc cref="IAuditable.InsertedBy"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 4,
        Description = nameof(Resources.IAuditableEntity_InsertedBy_Description),
            Name = nameof(Resources.IAuditableEntity_InsertedBy_Name),
            ShortName = nameof(Resources.IAuditableEntity_InsertedBy_ShortName))]
        [ScaffoldColumn(true)]
        public string InsertedBy => _insertedBy;

        /// <inheritdoc cref="IAuditable.UpdatedAt"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 5,
        Description = nameof(Resources.IAuditableEntity_UpdatedAt_Description),
            Name = nameof(Resources.IAuditableEntity_UpdatedAt_Name),
            ShortName = nameof(Resources.IAuditableEntity_UpdatedAt_ShortName))]
        [ScaffoldColumn(true)]
        public DateTime UpdatedAt => _updatedAt;

        /// <inheritdoc cref="IAuditable.UpdatedBy"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 6,
        Description = nameof(Resources.IAuditableEntity_UpdatedBy_Description),
            Name = nameof(Resources.IAuditableEntity_UpdatedBy_Name),
            ShortName = nameof(Resources.IAuditableEntity_UpdatedBy_ShortName))]
        [ScaffoldColumn(true)]
        public string UpdatedBy => _updatedBy;
        
    }

}
