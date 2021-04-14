using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case that fetches an existing <see cref="Domain.Security.User"/>.
    /// </summary>
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_FetchUser_Name))]
    [UseCase(ServiceLifetime.Transient, typeof(List<UserGroupLookup>), typeof(List<UserRoleLookup>))]
    public interface IFetchUserUseCase
    {
        /// <summary>
        /// Gets a <see cref="Domain.Security.User"/> for the specified id.
        /// </summary>
        /// <param name="id">an id of the <see cref="Domain.Security.User"/> to fetch</param>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<User> FetchAsync(IDomainEntityIdentity id, CancellationToken ct);

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

        /// <inheritdoc cref="IAuthorizedUseCase.CanInvoke"/>
        bool CanInvoke(bool throwOnNotAuthorized);

        /// <inheritdoc cref="IAuthorizedUseCase.User"/>
        ClaimsIdentity User { get; }
    }
}
