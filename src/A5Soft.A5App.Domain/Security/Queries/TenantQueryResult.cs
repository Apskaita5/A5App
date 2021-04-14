using A5Soft.A5App.Domain.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using A5Soft.CARMA.Domain;
using System.ComponentModel;
using A5Soft.A5App.Domain.Properties;

namespace A5Soft.A5App.Domain.Security.Queries
{
    /// <summary>
    /// A description of a tenant returned as a query result.
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_TenantQueryResult_DisplayNameForNew))]
    [Serializable]
    public class TenantQueryResult
    {
        protected IDomainEntityIdentity _id;
        protected string _name;
        protected string _databaseName;
        protected DateTime _insertedAt;
        protected string _insertedBy;
        protected string _userGroups;
        protected int _userCount;


        protected TenantQueryResult() { }


        /// <summary>
        /// an id of the tenant
        /// </summary>
        [Key]
        public IDomainEntityIdentity Id => _id;

        /// <inheritdoc cref="ITenant.Name"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
            Description = nameof(Resources.Security_ITenant_Name_Description),
            Name = nameof(Resources.Security_ITenant_Name_Name),
            ShortName = nameof(Resources.Security_ITenant_Name_ShortName))]
        [ScaffoldColumn(true)]
        public string Name => _name;

        /// <inheritdoc cref="ITenant.DatabaseName"/>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
            Description = nameof(Resources.Security_ITenant_DatabaseName_Description),
            Name = nameof(Resources.Security_ITenant_DatabaseName_Name),
            ShortName = nameof(Resources.Security_ITenant_DatabaseName_ShortName))]
        [ScaffoldColumn(true)]
        public string DatabaseName => _databaseName;

        /// <inheritdoc cref="ITenant.InsertedAt"/>
        [Browsable(true)]
        [Display(AutoGenerateField = false, ResourceType = typeof(Resources),
            Description = nameof(Resources.IAuditableEntity_InsertedAt_Description),
            Name = nameof(Resources.IAuditableEntity_InsertedAt_Name),
            ShortName = nameof(Resources.IAuditableEntity_InsertedAt_ShortName))]
        [ScaffoldColumn(false)]
        public DateTime InsertedAt => _insertedAt;

        /// <inheritdoc cref="ITenant.InsertedBy"/>
        [Browsable(true)]
        [Display(AutoGenerateField = false, ResourceType = typeof(Resources),
            Description = nameof(Resources.IAuditableEntity_InsertedBy_Description),
            Name = nameof(Resources.IAuditableEntity_InsertedBy_Name),
            ShortName = nameof(Resources.IAuditableEntity_InsertedBy_ShortName))]
        [ScaffoldColumn(false)]
        public string InsertedBy => _insertedBy;

        /// <summary>
        /// User groups that have access to the tenant data.
        /// </summary>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 5,
        Description = nameof(Resources.Security_TenantQueryResult_UserGroups_Description),
            Name = nameof(Resources.Security_TenantQueryResult_UserGroups_Name),
            ShortName = nameof(Resources.Security_TenantQueryResult_UserGroups_ShortName))]
        [ScaffoldColumn(true)]
        public string UserGroups => _userGroups;

        /// <summary>
        /// Number of the users that have access to the tenant data (excluding user group admins).
        /// </summary>
        [Browsable(true)]
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 6,
        Description = nameof(Resources.Security_TenantQueryResult_UserCount_Description),
            Name = nameof(Resources.Security_TenantQueryResult_UserCount_Name),
            ShortName = nameof(Resources.Security_TenantQueryResult_UserCount_ShortName))]
        [ScaffoldColumn(true)]
        public int UserCount => _userCount;

    }
}
