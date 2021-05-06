using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// a use case to logout a (current) tenant database
    /// </summary>
    [AuthenticatedAuthorization]
    [UseCase(ServiceLifetime.Transient)]
    public interface ILogOutUseCase : IAuthorizedUseCase
    {

        /// <summary>
        /// Logs out of the (current) tenant database and returns the respective ClaimsIdentity instance.
        /// </summary>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<ClaimsIdentity> QueryAsync(CancellationToken ct = default);

    }
}
