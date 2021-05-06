using System.Collections.Generic;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security.Tenants
{
    /// <summary>
    /// A use case that updates an existing <see cref="TenantGroupAssignments"/>.
    /// </summary>
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_UpdateTenantGroupAssignment_Name))]
    [UseCase(ServiceLifetime.Transient, typeof(List<UserGroupLookup>))]
    public interface IUpdateTenantGroupAssignmentsUseCase : IAuthorizedUseCase
    {
        /// <summary>
        /// Updates an existing <see cref="TenantGroupAssignments"/> using the data specified.
        /// </summary>
        /// <param name="domainDto">data for an updated <see cref="TenantGroupAssignments"/></param>
        /// <returns>the updated <see cref="TenantGroupAssignments"/></returns>
        [RemoteMethod]
        Task<TenantGroupAssignments> SaveAsync(ITenantGroupAssignments domainDto);
    }
}
