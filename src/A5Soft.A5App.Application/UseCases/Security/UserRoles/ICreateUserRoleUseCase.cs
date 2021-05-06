using A5Soft.A5App.Application.Properties;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security.UserRoles
{
    /// <summary>
    /// A use case that creates a new <see cref="UserRole"/>.
    /// </summary>
    [GroupAdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_CreateUserRole_Name))]
    [UseCase(ServiceLifetime.Transient)]
    public interface ICreateUserRoleUseCase : IAuthorizedUseCase
    {
        /// <summary>
        /// Creates a new <see cref="UserRole"/> using the data specified.
        /// </summary>
        /// <param name="domainDto">data for a new <see cref="UserRole"/></param>
        /// <returns>the created user group</returns>
        [RemoteMethod]
        Task<UserRole> SaveAsync(IUserRole domainDto);

    }
}
