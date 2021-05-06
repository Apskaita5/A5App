using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using System.ComponentModel.DataAnnotations;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// An assignment of the parent tenant to a user group.
    /// </summary>
    [DomainClassDescription(NameForCreateNew = nameof(Resources.Security_ITenantGroupAssignment_DisplayNameForCreateNew))]
    public interface ITenantGroupAssignment : IDomainEntity
    {
        /// <summary>
        /// A user group that the tenant is assigned to.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
            Description = nameof(Resources.Security_ITenantGroupAssignment_Group_Description),
            Name = nameof(Resources.Security_ITenantGroupAssignment_Group_Name),
            ShortName = nameof(Resources.Security_ITenantGroupAssignment_Group_ShortName))]
        IDomainEntityIdentity GroupId { get; }
    }

}
