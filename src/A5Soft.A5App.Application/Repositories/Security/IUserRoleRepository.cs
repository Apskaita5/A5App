using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static A5Soft.A5App.Domain.Security.UserRole;

namespace A5Soft.A5App.Application.Repositories.Security
{
    /// <summary>
    /// a repository for <see cref="UserRole"/>
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public interface IUserRoleRepository
    {

        /// <summary>
        /// Fetches a user role DTO for the identity specified.
        /// </summary>
        /// <param name="id">an id of the user role to fetch</param>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<UserRoleDto> FetchAsync(IDomainEntityIdentity id,
            CancellationToken ct = default);

        /// <summary>
        /// Fetches a list of <see cref="UserRoleQueryResult"/>.
        /// </summary>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<List<UserRoleQueryResult>> QueryAsync(CancellationToken ct = default);

        /// <summary>
        /// Fetches a lookup list for reference to <see cref="UserRole"/>.
        /// </summary>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<List<UserRoleLookup>> FetchLookupAsync(CancellationToken ct = default);

        /// <summary>
        /// Inserts (creates) a new <see cref="UserRole"/>.
        /// </summary>
        /// <param name="dto">a DTO containing data for the new <see cref="UserRole"/></param>
        /// <param name="userId">an email of the user who creates the new role</param>
        Task<UserRoleDto> InsertAsync(UserRoleDto dto, string userId);

        /// <summary>
        /// Updates an existing <see cref="UserRole"/>.
        /// </summary>
        /// <param name="dto">a DTO containing data for the <see cref="UserRole"/> to update</param>
        /// <param name="userId">an email of the user who updates the role</param>
        Task<UserRoleDto> UpdateAsync(UserRoleDto dto, string userId);

        /// <summary>
        /// Deletes an existing <see cref="UserRole"/>.
        /// </summary>
        /// <param name="id">an id of the user role to delete</param>
        Task DeleteAsync(IDomainEntityIdentity id);

    }
}
