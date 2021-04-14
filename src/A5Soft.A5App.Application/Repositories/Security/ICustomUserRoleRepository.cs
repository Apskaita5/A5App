using A5Soft.CARMA.Domain;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using static A5Soft.A5App.Domain.Security.CustomUserRole;

namespace A5Soft.A5App.Application.Repositories.Security
{
    /// <summary>
    /// a repository for <see cref="CustomUserRole"/>
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public interface ICustomUserRoleRepository
    {

        /// <summary>
        /// Fetches a custom user role DTO for the user and tenant specified.
        /// Returns null if no such user, no such tenant, or the user has any admin privileges
        /// (because admin users do not have any roles for tenants)
        /// </summary>
        /// <param name="userId">an id of the user to fetch a custom role for</param>
        /// <param name="tenantId">an id of the tenant to fetch a custom role for</param>
        /// <param name="ct">a cancellation token (if any)</param>
        Task<CustomUserRoleDto> FetchAsync(IDomainEntityIdentity userId,
            IDomainEntityIdentity tenantId, CancellationToken ct = default);
                                            
        /// <summary>
        /// Updates an existing <see cref="CustomUserRole"/>.
        /// </summary>
        /// <param name="dto">a DTO containing data for the <see cref="CustomUserRole"/> to update</param>
        /// <param name="userId">an email of the user who updates the custom role</param>
        Task<CustomUserRoleDto> UpdateAsync(CustomUserRoleDto dto, string userId);

    }
}
