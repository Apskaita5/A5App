using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Security;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;

namespace A5Soft.A5App.Application.UseCases.Security.UserRoles
{
    /// <summary>
    /// A use case that queries existing <see cref="UserRole"/>, i.e. gets a list of 
    /// <see cref = "UserRoleQueryResult" />, i.e.description of all the user roles.
    /// </summary>
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_QueryUserRoles_Name))]
    [UseCase(ServiceLifetime.Transient)]
    public interface IQueryUserRolesUseCase : IAuthorizedUseCase
    {
        /// <summary>
        /// Gets a list of <see cref="UserRoleQueryResult"/>, i.e. description of all the user roles.
        /// </summary>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<List<UserRoleQueryResult>> QueryAsync(CancellationToken ct);

        /// <summary>
        /// Gets a metadata for <see cref="UserRoleQueryResult"/>.
        /// </summary>
        IEntityMetadata GetMetadata();
    }
}
