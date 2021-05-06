using System;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case that tries to log into the local tenant database.
    /// </summary>
    [UseCase(ServiceLifetime.Transient)]
    public interface ILoginLocalTenantUseCase
    {
        /// <summary>
        /// Tries to log into the local tenant database and returns local login result.
        /// </summary>
        /// <param name="tenantId">an id of the tenant to login for</param>
        /// <param name="password">a local tenant database password (if requested by the login result)</param>
        Task<LocalLoginResponse> Invoke(Guid tenantId, string password = null);
    }
}