using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;

namespace A5Soft.A5App.Application.UseCases.Security.Users
{
    /// <summary>
    /// A use case that fetches a new <see cref="Domain.Security.User"/> instance with the tenants available.
    /// </summary> 
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_FetchUser_Name))]
    [UseCase(ServiceLifetime.Transient, typeof(List<UserGroupLookup>), typeof(List<UserRoleLookup>))]
    public interface IInitializeNewUserUseCase : IAuthorizedUseCase
    {
        /// <summary>
        /// Fetches a new <see cref="Domain.Security.User"/> instance with the tenants available.
        /// </summary>
        /// <param name="ct">cancellation token (if any)</param>
        /// <returns>a new <see cref="Domain.Security.User"/> instance with the tenants available</returns>
        Task<User> FetchAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets a user group lookup to use for editing the <see cref="Domain.Security.User"/>.
        /// </summary>
        Task<List<UserGroupLookup>> FetchUserGroupLookupAsync();

        /// <summary>
        /// Gets a user role lookup to use for editing the <see cref="Domain.Security.User"/>.
        /// </summary>
        /// <param name="forUserTenant">a tenant to get the lookup for</param>
        Task<List<UserRoleLookup>> FetchUserRoleLookup(IUserTenant forUserTenant);

        /// <summary>
        /// Gets <see cref="Domain.Security.User"/> metadata.
        /// </summary>
        IEntityMetadata GetMetadata();
    }
}