using A5Soft.A5App.Application.Properties;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case that queries existing <see cref="Tenant"/>, i.e. gets a list of 
    /// <see cref = "TenantQueryResult" />, i.e.description of all tenants.
    /// </summary>
    [AdminAuthorization(typeof(Resources),
        nameof(Resources.DefaultPermissions_Admin_QueryTenants_Name))]
    [UseCase(ServiceLifetime.Transient)]
    public interface IQueryTenantsUseCase
    {
        /// <summary>
        /// Gets a list of <see cref="TenantQueryResult"/>, i.e. description of all tenants.
        /// </summary>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<List<TenantQueryResult>> QueryAsync(CancellationToken ct);

        /// <summary>
        /// Gets a metadata for <see cref="TenantQueryResult"/>.
        /// </summary>
        IEntityMetadata GetMetadata();

        /// <inheritdoc cref="IAuthorizedUseCase.CanInvoke"/>
        bool CanInvoke(bool throwOnNotAuthorized);

        /// <inheritdoc cref="IAuthorizedUseCase.User"/>
        ClaimsIdentity User { get; }
    }
}
