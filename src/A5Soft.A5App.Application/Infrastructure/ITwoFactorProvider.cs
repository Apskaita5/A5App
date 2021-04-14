using System.Threading.Tasks;
using A5Soft.A5App.Application.UseCases.Security;

namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// An interface that should be implemented by a particular two factor authentication implementation
    /// for a particular communication channel (email, SMS etc.).
    /// </summary>
    public interface ITwoFactorProvider
    {

        /// <summary>
        /// Sends a two factor authentication request to the user.
        /// </summary>
        /// <param name="token">two factor authentication token</param>
        /// <param name="user">user to send the request to</param>
        Task SendSecondFactorRequest(string token, UserIdentity user);

    }
}
