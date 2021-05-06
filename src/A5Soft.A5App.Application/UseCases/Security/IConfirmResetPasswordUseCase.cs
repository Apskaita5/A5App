using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// A use case for confirming user password reset.
    /// </summary> 
    [UseCase(ServiceLifetime.Singleton)]
    public interface IConfirmResetPasswordUseCase : IUseCase
    {
        /// <summary>
        /// Confirm password reset and login by specifying the password reset confirmation token (in url). 
        /// </summary>
        /// <param name="urlToken">password reset confirmation token (in url)</param>
        /// <param name="ct">cancellation token (if any)</param>
        [RemoteMethod]
        Task<LoginResponse> InvokeAsync(string urlToken, CancellationToken ct);
    }
}