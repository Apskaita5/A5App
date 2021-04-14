using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// a user role for a tenant
    /// </summary>
    [DomainClassDescription(NameForNew = nameof(Resources.Security_IUserTenant_DisplayNameForNew))]
    public interface IUserTenant : IDomainEntity
    {

        /// <summary>
        /// An id of the tenant to set a user role for.
        /// </summary>
        IDomainEntityIdentity TenantId { get; }

        /// <summary>
        /// A name of the tenant to set a user role for.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_IUserTenant_TenantName_Description),
            Name = nameof(Resources.Security_IUserTenant_TenantName_Name),
            ShortName = nameof(Resources.Security_IUserTenant_TenantName_ShortName))]
        string TenantName { get; }

        /// <summary>
        /// A user role for the tenant.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
        Description = nameof(Resources.Security_IUserTenant_Role_Description),
            Name = nameof(Resources.Security_IUserTenant_Role_Name),
            ShortName = nameof(Resources.Security_IUserTenant_Role_ShortName),
            Prompt = nameof(Resources.Security_IUserTenant_Role_Prompt))]
        IDomainEntityIdentity RoleId { get; }

    }
}
