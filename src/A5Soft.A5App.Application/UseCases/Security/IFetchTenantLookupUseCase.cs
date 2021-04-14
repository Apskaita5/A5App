using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case that gets a list of <see cref="TenantLookup"/> accessible for the user.
    /// </summary>
    [AuthenticatedAuthorization]
    [UseCase(ServiceLifetime.Transient)]
    public interface IFetchTenantLookupUseCase
    {
        /// <summary>
        /// Gets a list of <see cref="TenantLookup"/> accessible for the user.
        /// </summary>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<List<TenantLookup>> QueryAsync(CancellationToken ct = default);

        /// <summary>
        /// Gets a metadata for query result.
        /// </summary>
        IEntityMetadata GetMetadata();

        /// <inheritdoc cref="IAuthorizedUseCase.CanInvoke"/>
        bool CanInvoke(bool throwOnNotAuthorized);

        /// <inheritdoc cref="IAuthorizedUseCase.User"/>
        ClaimsIdentity User { get; }
    }
}