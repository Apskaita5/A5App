using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case that queries existing <see cref="Domain.Security.User"/>, i.e. gets a list of 
    /// <see cref="UserQueryResult"/>, i.e. description of app users.
    /// </summary>
    [GroupAdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_QueryUsers_Name))]
    [UseCase(ServiceLifetime.Transient)]
    public interface IQueryUsersUseCase
    {
        /// <summary>
        /// Gets a list of <see cref="UserQueryResult"/>, i.e. description of app users.
        /// </summary>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<List<UserQueryResult>> QueryAsync(CancellationToken ct);

        /// <summary>
        /// Gets a metadata for <see cref="UserQueryResult"/>.
        /// </summary>
        IEntityMetadata GetMetadata();

        /// <inheritdoc cref="IAuthorizedUseCase.CanInvoke"/>
        bool CanInvoke(bool throwOnNotAuthorized);

        /// <inheritdoc cref="IAuthorizedUseCase.User"/>
        ClaimsIdentity User { get; }
    }
}
