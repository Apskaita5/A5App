using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case for user login to a particular tenant database.
    /// </summary>
    [AuthenticatedAuthorization]
    [UseCase(ServiceLifetime.Transient)]
    public interface ILoginTenantUseCase
    {
        /// <summary>
        /// login to the specified tenant database
        /// </summary>
        /// <param name="tenantId">an id of the tenant to login to</param>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<LoginResponse> QueryAsync(Guid tenantId, CancellationToken ct);

        /// <inheritdoc cref="IAuthorizedUseCase.CanInvoke"/>
        bool CanInvoke(bool throwOnNotAuthorized);

        /// <inheritdoc cref="IAuthorizedUseCase.User"/>
        ClaimsIdentity User { get; }
    }
}