using System.Threading.Tasks;

namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// Interface for sending system messages (emails) to users (e.g. to confirm password reset).
    /// </summary>
    public interface IEmailProvider
    {
        /// <summary>
        /// Sends the email message using system email account.
        /// </summary>
        /// <param name="message">an email message to send</param>
        Task SendAsync(EmailMessage message);
    }
}
