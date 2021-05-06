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
    /// A use case that fetches an existing <see cref="CustomUserRole"/>.
    /// </summary>
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_FetchCustomUserRole_Name))]
    [UseCase(ServiceLifetime.Transient)]
    public interface IFetchCustomUserRoleUseCase : IAuthorizedUseCase
    {
        /// <summary>
        /// Gets a <see cref="CustomUserRole"/> for the specified id.
        /// </summary>
        /// <param name="id">a complex id of type <see cref="CustomUserRoleIdentity"/>, containing reference
        /// to both <see cref="Domain.Security.User"/> and the <see cref="Tenant"/> to fetch a custom role for</param>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<CustomUserRole> FetchAsync(IDomainEntityIdentity id, CancellationToken ct);

        /// <summary>
        /// Gets <see cref="CustomUserRole"/> metadata.
        /// </summary>
        IEntityMetadata GetMetadata();
    }
}
