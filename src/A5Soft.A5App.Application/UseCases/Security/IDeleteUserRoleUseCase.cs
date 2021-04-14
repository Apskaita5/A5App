using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;
using System.Security.Claims;
using System.Threading.Tasks;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case that deletes a <see cref="UserRole"/>.
    /// </summary>
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_DeleteUserRole_Name))]
    [UseCase(ServiceLifetime.Transient)]
    public interface IDeleteUserRoleUseCase
    {
        /// <summary>
        /// Validates an id of the <see cref="UserRole"/> to delete.
        /// </summary>
        /// <param name="parameter">an id of the <see cref="UserRole"/> to delete</param>
        BrokenRulesCollection Validate(IDomainEntityIdentity parameter);

        /// <summary>
        /// Deletes a <see cref="UserRole"/>.
        /// </summary>
        /// <param name="parameter">an id of the <see cref="UserRole"/> to delete</param>
        [RemoteMethod]
        Task InvokeAsync(IDomainEntityIdentity parameter);

        /// <inheritdoc cref="IAuthorizedUseCase.CanInvoke"/>
        bool CanInvoke(bool throwOnNotAuthorized);

        /// <inheritdoc cref="IAuthorizedUseCase.User"/>
        ClaimsIdentity User { get; }

    }
}
