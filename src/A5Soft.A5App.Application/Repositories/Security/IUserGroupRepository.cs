using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Domain;
using static A5Soft.A5App.Domain.Security.UserGroup;

namespace A5Soft.A5App.Application.Repositories.Security
{
    /// <summary>
    /// a repository for <see cref="UserGroup"/>
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public interface IUserGroupRepository
    {
        /// <summary>
        /// Fetches a user group DTO for the identity specified.
        /// </summary>
        /// <param name="id">an id of the user group to fetch</param>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<UserGroupDto> FetchAsync(IDomainEntityIdentity id, 
            CancellationToken ct = default);

        /// <summary>
        /// Fetches a list of <see cref="UserGroupQueryResult"/>.
        /// </summary>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<List<UserGroupQueryResult>> QueryAsync(CancellationToken ct = default);

        /// <summary>
        /// Fetches a lookup list for reference to <see cref="UserGroup"/>.
        /// </summary>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<List<UserGroupLookup>> FetchLookupAsync(CancellationToken ct = default);

        /// <summary>
        /// Inserts (creates) a new <see cref="UserGroup"/>.
        /// </summary>
        /// <param name="dto">a DTO containing data for the new <see cref="UserGroup"/></param>
        /// <param name="userId">an email of the user who creates the new group</param>
        Task<UserGroupDto> InsertAsync(UserGroupDto dto, string userId);

        /// <summary>
        /// Updates an existing <see cref="UserGroup"/>.
        /// </summary>
        /// <param name="dto">a DTO containing data for the <see cref="UserGroup"/> to update</param>
        /// <param name="userId">an email of the user who updates the group</param>
        Task<UserGroupDto> UpdateAsync(UserGroupDto dto, string userId);

        /// <summary>
        /// Deletes an existing <see cref="UserGroup"/>.
        /// </summary>
        /// <param name="id">an id of the user group to delete</param>
        Task DeleteAsync(IDomainEntityIdentity id);

    }
}