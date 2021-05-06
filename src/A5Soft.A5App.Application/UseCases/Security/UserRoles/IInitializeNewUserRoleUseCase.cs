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
    /// A use case that initializes a new <see cref="UserRole"/> with available permissions.
    /// </summary>
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_CreateUserRole_Name))]
    [UseCase(ServiceLifetime.Transient)]
    public interface IInitializeNewUserRoleUseCase : IAuthorizedUseCase
    {
        /// <summary>
        /// Fetches a new <see cref="UserRole"/> instance with available permissions.
        /// </summary>
        /// <param name="ct">a cancellation token (if any)</param>
        /// <returns>a new <see cref="UserRole"/> instance with available permissions</returns>
        Task<UserRole> FetchAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets <see cref="UserRole"/> metadata.
        /// </summary>
        IEntityMetadata GetMetadata();
    }
}