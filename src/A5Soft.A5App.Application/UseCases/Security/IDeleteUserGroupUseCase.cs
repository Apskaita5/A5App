using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.A5App.Domain.Security;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case that deletes a <see cref="UserGroup"/>.
    /// </summary>
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_DeleteUserGroup_Name))]
    [UseCase(ServiceLifetime.Transient)]
    public interface IDeleteUserGroupUseCase
    {
        /// <summary>
        /// Validates an id of the user group to delete.
        /// </summary>
        /// <param name="parameter">an id of the user group to delete</param>
        BrokenRulesCollection Validate(IDomainEntityIdentity parameter);

        /// <summary>
        /// Deletes a <see cref="UserGroup"/>.
        /// </summary>
        /// <param name="parameter">an id of the user group to delete</param>
        [RemoteMethod]
        Task InvokeAsync(IDomainEntityIdentity parameter);

        /// <inheritdoc cref="IAuthorizedUseCase.CanInvoke"/>
        bool CanInvoke(bool throwOnNotAuthorized);

        /// <inheritdoc cref="IAuthorizedUseCase.User"/>
        ClaimsIdentity User { get; }

    }
}