using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case that checks if the current user identity is valid, i.e. not forged, not expired,
    /// user credentials has not changed. Throws <see cref="UnauthenticatedException"/> if
    /// the identity is not valid for any reason.
    /// </summary>
    [UseCase(ServiceLifetime.Singleton)]
    public interface IValidateUserIdentityUseCase : IUseCase
    {
        /// <summary>
        /// Checks if the current user identity is valid, i.e. not forged, not expired,
        /// user credentials has not changed. Throws <see cref="UnauthenticatedException"/> if
        /// the identity is not valid for any reason.
        /// </summary>
        /// <param name="identity">identity to validate</param>
        [RemoteMethod]
        Task InvokeAsync(ClaimsIdentity identity);
    }
}