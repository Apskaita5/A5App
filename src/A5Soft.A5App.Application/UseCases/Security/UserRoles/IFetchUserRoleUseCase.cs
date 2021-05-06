using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;

namespace A5Soft.A5App.Application.UseCases.Security.UserRoles
{
    /// <summary>
    /// A use case that fetches an existing <see cref="UserRole"/>.
    /// </summary>
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_FetchUserRole_Name))]
    [UseCase(ServiceLifetime.Transient)]
    public interface IFetchUserRoleUseCase : IAuthorizedUseCase
    {
        /// <summary>
        /// Gets a <see cref="UserRole"/> for the specified id.
        /// </summary>
        /// <param name="id">an id of the <see cref="UserRole"/> to fetch</param>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<UserRole> FetchAsync(IDomainEntityIdentity id, CancellationToken ct);

        /// <summary>
        /// Gets <see cref="UserRole"/> metadata.
        /// </summary>
        IEntityMetadata GetMetadata();
    }
}
