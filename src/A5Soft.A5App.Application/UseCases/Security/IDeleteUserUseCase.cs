using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case that deletes a <see cref="Domain.Security.User"/>.
    /// </summary>
    [GroupAdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_DeleteUser_Name))]
    [UseCase(ServiceLifetime.Transient, typeof(List<UserGroupLookup>), typeof(List<UserRoleLookup>))]
    public interface IDeleteUserUseCase
    {
        /// <summary>
        /// Validates an id of the <see cref="Domain.Security.User"/> to delete.
        /// </summary>
        /// <param name="parameter">an id of the <see cref="Domain.Security.User"/> to delete</param>
        BrokenRulesCollection Validate(IDomainEntityIdentity parameter);

        /// <summary>
        /// Deletes a <see cref="Domain.Security.User"/>.
        /// </summary>
        /// <param name="parameter">an id of the <see cref="Domain.Security.User"/> to delete</param>
        [RemoteMethod]
        Task InvokeAsync(IDomainEntityIdentity parameter);

        /// <inheritdoc cref="IAuthorizedUseCase.CanInvoke"/>
        bool CanInvoke(bool throwOnNotAuthorized);

        /// <inheritdoc cref="IAuthorizedUseCase.User"/>
        ClaimsIdentity User { get; }

    }
}
