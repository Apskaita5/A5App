using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Infrastructure
{
    /// <summary>
    /// configuration data for native email client
    /// </summary>
    [Service(ServiceLifetime.Singleton)]
    public interface INativeEmailConfiguration
    {
        /// <summary>
        /// sender email address
        /// </summary>
        string EmailAddress { get; }

        /// <summary>
        /// sender display name
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// smtp server address
        /// </summary>
        string Host { get; }

        /// <summary>
        /// smtp server port
        /// </summary>
        int Port { get; }

        /// <summary>
        /// whether to use ssl for smtp server
        /// </summary>
        bool EnableSsl { get; }

        /// <summary>
        /// whether to use default credentials configured for the process
        /// </summary>
        bool UseDefaultCredentials { get; }

        /// <summary>
        /// user name for the smtp server
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// user password for the smtp server
        /// </summary>
        string Password { get; }

        /// <summary>
        /// how many times retry sending a message (on recoverable exception) before failing (0-10)
        /// </summary>
        int Retries { get; }

        /// <summary>
        /// how long (in ms) to wait before retrying to send a message (on recoverable exception) (10-120000)
        /// </summary>
        int RetryWaitPeriod { get; }
    }
}
