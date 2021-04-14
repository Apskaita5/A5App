using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// A custom user role for a tenant.
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_ICustomUserRole_DisplayNameForNew),
    NameForOld = nameof(Resources.Security_ICustomUserRole_DisplayNameForOld),
    NameForCreateNew = nameof(Resources.Security_ICustomUserRole_DisplayNameForCreateNew))]
    public interface ICustomUserRole : IDomainEntity
    {

        /// <summary>
        /// A id of the user whom the custom role is configured for.
        /// </summary>
        IDomainEntityIdentity UserId { get; }

        /// <summary>
        /// A id of the user group that the user belongs to.
        /// </summary>
        IDomainEntityIdentity UserGroupId { get; }

        /// <summary>
        /// A name of the user whom the custom role is configured for.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_ICustomUserRole_UserName_Description),
            Name = nameof(Resources.Security_ICustomUserRole_UserName_Name),
            ShortName = nameof(Resources.Security_ICustomUserRole_UserName_ShortName))]
        string UserName { get; }

        /// <summary>
        /// An id of the tenant whom the custom user role is configured for.
        /// </summary>
        IDomainEntityIdentity TenantId { get; }

        /// <summary>
        /// A name of the tenant whom the custom user role is configured for.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
        Description = nameof(Resources.Security_ICustomUserRole_TenantName_Description),
            Name = nameof(Resources.Security_ICustomUserRole_TenantName_Name),
            ShortName = nameof(Resources.Security_ICustomUserRole_TenantName_ShortName))]
        string TenantName { get; }

        /// <summary>
        /// A list of permissions for the user to have while managing the tenant data.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 3,
        Description = nameof(Resources.Security_ICustomUserRole_Permissions_Description),
            Name = nameof(Resources.Security_ICustomUserRole_Permissions_Name),
            ShortName = nameof(Resources.Security_ICustomUserRole_Permissions_ShortName))]
        IList<IUserPermission> Permissions { get; }

        /// <inheritdoc cref="IAuditable.OccHash"/> 
        [IgnorePropertyMetadata]
        string OccHash { get; }

    }
}
