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
    [AuthenticatedAuthorization]
    [UseCase(ServiceLifetime.Transient)]
    public interface IValidateUserIdentityUseCase
    {
        /// <summary>
        /// Checks if the current user identity is valid, i.e. not forged, not expired,
        /// user credentials has not changed. Throws <see cref="UnauthenticatedException"/> if
        /// the identity is not valid for any reason.
        /// </summary>
        Task InvokeAsync();
    }
}