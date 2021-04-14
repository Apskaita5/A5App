using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using static A5Soft.A5App.Domain.Security.User;

namespace A5Soft.A5App.Application.Repositories.Security
{
    /// <summary>
    /// a repository for <see cref="User"/>
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public interface IUserRepository
    {
        /// <summary>
        /// Fetches a user DTO for the identity specified.
        /// </summary>
        /// <param name="id">an id of the user to fetch</param>
        /// <param name="tenants">a lookup for the tenants that could be assigned to the user
        /// by the editing user (tenant scope depends on the editing user privileges)</param>
        /// <param name="ct">a cancellation token (if any)</param>  
        Task<UserDto> FetchAsync(IDomainEntityIdentity id, IEnumerable<TenantLookup> tenants, 
            CancellationToken ct = default);

        /// <summary>
        /// Fetches a user lookup list used for authentication via ClaimsIdentity object.
        /// </summary>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<List<UserLookup>> FetchLookupAsync(CancellationToken ct = default);

        /// <summary>
        /// Fetches a list of <see cref="UserQueryResult"/>.
        /// </summary>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<List<UserQueryResult>> QueryAsync(CancellationToken ct = default);

        /// <summary>
        /// Inserts (creates) a new <see cref="User"/>.
        /// </summary>
        /// <param name="dto">a DTO containing data for the new <see cref="User"/></param>
        /// <param name="userId">an email of the user who creates the new user</param>
        Task<UserDto> InsertAsync(UserDto dto, string userId);

        /// <summary>
        /// Updates an existing <see cref="User"/>.
        /// </summary>
        /// <param name="dto">a DTO containing data for the <see cref="User"/> to update</param>
        /// <param name="userId">an email of the user who updates the user</param>
        Task<UserDto> UpdateAsync(UserDto dto, string userId);

        /// <summary>
        /// Deletes an existing <see cref="User"/>.
        /// </summary>
        /// <param name="id">an id of the user to delete</param>
        Task DeleteAsync(IDomainEntityIdentity id);

        /// <summary>
        /// Updates password for a <see cref="User"/>.
        /// </summary>
        /// <param name="id">an id of the user to update the password for</param>
        /// <param name="hashedPassword">hashed value of the new password</param>
        Task UpdatePasswordAsync(IDomainEntityIdentity id, string hashedPassword);

        /// <summary>
        /// Fetches user credentials (id and password hash) for login.
        /// </summary>
        /// <param name="userEmail">an email of the user to fetch login credentials for</param>
        /// <returns>If success - user credentials (id and password hash);
        /// otherwise id is null.</returns>
        Task<(Guid? Id, string PasswordHash)> FetchLoginInfoAsync(string userEmail);

        /// <summary>
        /// Fetches user identity for authorization.
        /// </summary>
        /// <param name="userId">an id of the user</param>
        /// <param name="tenantId">an id of the tenant to log in (if any)</param>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<UserIdentity> FetchUserIdentityAsync(Guid userId, Guid? tenantId,
            CancellationToken ct = default);

        /// <summary>
        /// Fetches current user count within the <see cref="UserGroup"/> specified.
        /// </summary>
        /// <param name="groupId">an id of the <see cref="UserGroup"/> to count users for</param>
        Task<int> CountUsersInGroupAsync(IDomainEntityIdentity groupId);

    }
}