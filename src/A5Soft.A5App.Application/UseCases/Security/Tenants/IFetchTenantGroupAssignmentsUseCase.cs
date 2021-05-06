using System.Collections.Generic;
using A5Soft.A5App.Application.Properties;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;

namespace A5Soft.A5App.Application.UseCases.Security.Tenants
{
    /// <summary>
    /// A use case that fetches an existing <see cref="TenantGroupAssignments"/>.
    /// </summary>
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_FetchTenantGroupAssignment_Name))]
    [UseCase(ServiceLifetime.Transient, typeof(List<UserGroupLookup>))]
    public interface IFetchTenantGroupAssignmentsUseCase : IAuthorizedUseCase
    {
        /// <summary>
        /// Gets a <see cref="TenantGroupAssignments"/> for the specified id.
        /// </summary>
        /// <param name="id">an id of the <see cref="TenantGroupAssignments"/> to fetch
        /// (same as <see cref="Tenant"/> id)</param>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<TenantGroupAssignments> FetchAsync(IDomainEntityIdentity id, CancellationToken ct = default);


        /// <summary>
        /// Gets a lookup to use for editing the <see cref="TenantGroupAssignments"/>.
        /// </summary>
        Task<List<UserGroupLookup>> FetchLookupAsync();

        /// <summary>
        /// Gets <see cref="TenantGroupAssignment"/> metadata.
        /// </summary>
        IEntityMetadata GetMetadata();
    }
}
