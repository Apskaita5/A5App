using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.Repositories.Security
{
    /// <summary>
    /// a repository for <see cref="Tenant"/>
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public interface ITenantRepository
    {
        /// <summary>
        /// Fetches a tenant DTO for the identity specified.
        /// </summary>
        /// <param name="id">an id of the tenant to fetch</param>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<Tenant.TenantDto> FetchAsync(IDomainEntityIdentity id,
            CancellationToken ct = default);

        /// <summary>
        /// Fetches a list of the tenant assignments to user groups.
        /// </summary>
        /// <param name="id">a tenant id</param>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<TenantGroupAssignments.TenantGroupAssignmentsDto> FetchAssignmentsAsync(
            IDomainEntityIdentity id, CancellationToken ct = default);

        /// <summary>
        /// Fetches a (full) lookup list for reference to <see cref="Tenant"/>. 
        /// </summary>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<List<TenantLookup>> FetchLookupAsync(CancellationToken ct = default);

        /// <summary>
        /// Fetches a lookup list for reference to <see cref="Tenant"/>
        /// that are accessible by the user specified.
        /// </summary>
        /// <param name="userId">an id of the user to fetch a lookup list for</param>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<List<TenantLookup>> FetchLookupForUserAsync(IDomainEntityIdentity userId, 
            CancellationToken ct = default);

        /// <summary>
        /// Fetches a list of <see cref="TenantQueryResult"/>.
        /// </summary>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<List<TenantQueryResult>> QueryAsync(CancellationToken ct = default);

        /// <summary>
        /// Inserts (creates) a new <see cref="Tenant"/>
        /// (does not create a database only registers already existing).
        /// </summary>
        /// <param name="dto">a DTO containing data for the new <see cref="Tenant"/></param>
        /// <param name="userId">an email of the user who creates the new tenant</param>
        Task<Tenant.TenantDto> InsertAsync(Tenant.TenantDto dto, string userId);

        /// <summary>
        /// Updates an existing <see cref="Tenant"/>.
        /// </summary>
        /// <param name="dto">a DTO containing data for the <see cref="Tenant"/> to update</param>
        Task<Tenant.TenantDto> UpdateAsync(Tenant.TenantDto dto);

        /// <summary>
        /// Updates a list of the tenant assignments to user groups.
        /// </summary>
        /// <param name="dto">a list of the tenant assignments to user groups</param>
        Task<TenantGroupAssignments.TenantGroupAssignmentsDto> UpdateAssignmentsAsync(
            TenantGroupAssignments.TenantGroupAssignmentsDto dto);

        /// <summary>
        /// Deletes an existing <see cref="Tenant"/>.
        /// (does not drop the database only unregisters)
        /// </summary>
        /// <param name="id">an id of the tenant to delete (unregister)</param>
        Task DeleteAsync(IDomainEntityIdentity id);

        /// <summary>
        /// Assigns a tenant to a user group.
        /// </summary>
        /// <param name="id">an id of the tenant</param>
        /// <param name="groupId">an id of the user group</param>
        Task AssignToUserGroupAsync(IDomainEntityIdentity id, IDomainEntityIdentity groupId);

        /// <summary>
        /// Gets a value indicating whether one more tenant could be added to the user group specified.
        /// (see <see cref="IUserGroup.MaxTenants"/>)
        /// </summary>
        /// <param name="groupId">an id of the user group to check</param>
        Task<bool> CanAssignToUserGroup(IDomainEntityIdentity groupId);

    }
}