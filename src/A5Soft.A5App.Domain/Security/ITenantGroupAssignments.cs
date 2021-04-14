using A5Soft.A5App.Domain.DataAnnotations;
using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// A list of user group assignments for a tenant.
    /// </summary>
    [DomainClassDescription(NameForOld = nameof(Resources.Security_ITenantGroupAssignments_DisplayNameForOld))]
    public interface ITenantGroupAssignments : IDomainEntity
    {

        /// <summary>
        /// A name of the tenant to manage user group assignments for.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
        Description = nameof(Resources.Security_ITenantGroupAssignments_TenantName_Description),
            Name = nameof(Resources.Security_ITenantGroupAssignments_TenantName_Name),
            ShortName = nameof(Resources.Security_ITenantGroupAssignments_TenantName_ShortName))]
        string TenantName { get; }

        /// <summary>
        /// A list of assignments.
        /// </summary>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 1,
        Description = nameof(Resources.Security_ITenantGroupAssignments_Assignments_Description),
            Name = nameof(Resources.Security_ITenantGroupAssignments_Assignments_Name),
            ShortName = nameof(Resources.Security_ITenantGroupAssignments_Assignments_ShortName),
            Prompt = nameof(Resources.Security_ITenantGroupAssignments_Assignments_Prompt))]
        List<IDomainEntityIdentity> Assignments { get; }

    }
}
