using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case that updates an existing <see cref="CustomUserRole"/>.
    /// </summary>
    [GroupAdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_UpdateCustomUserRole_Name))]
    [UseCase(ServiceLifetime.Transient)]
    public interface IUpdateCustomUserRoleUseCase
    {
        /// <summary>
        /// Updates an existing <see cref="CustomUserRole"/> using the data specified.
        /// </summary>
        /// <param name="domainDto">data for an updated <see cref="CustomUserRole"/></param>
        /// <returns>the updated <see cref="CustomUserRole"/></returns>
        [RemoteMethod]
        Task<CustomUserRole> SaveAsync(ICustomUserRole domainDto);

        /// <inheritdoc cref="IAuthorizedUseCase.CanInvoke"/>
        bool CanInvoke(bool throwOnNotAuthorized);

        /// <inheritdoc cref="IAuthorizedUseCase.User"/>
        ClaimsIdentity User { get; }
    }
}
