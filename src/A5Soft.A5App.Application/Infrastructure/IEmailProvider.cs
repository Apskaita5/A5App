using A5Soft.CARMA.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// Interface for sending system messages (emails) to users (e.g. to confirm password reset).
    /// </summary>       
    [Service(ServiceLifetime.Singleton)]
    public interface IEmailProvider
    {
        /// <summary>
        /// Sends the email message using system email account.
        /// </summary>
        /// <param name="message">an email message to send</param>
        /// <param name="ct">cancellation token (if any)</param>
        Task SendAsync(EmailMessage message, CancellationToken ct = default);
    }
}
