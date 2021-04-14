using System.Security.Claims;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// a container for security related server scope settings
    /// </summary>
    [Service]
    public interface ISecurityPolicy
    {

        /// <summary>
        /// Gets a value, that is only known by the app server, to use as a salt when
        /// issuing identity security token. 
        /// </summary>
        string ServerSecret { get; }

        /// <summary>
        /// Gets a (default) authentication type for the app server.
        /// (see <see cref="ClaimsIdentity.AuthenticationType"/>)
        /// same server might implement different authentication types for different channels
        /// (cookies for web site visitors, Bearer token for API, custom for DataPortal etc.);
        /// in that case the server shall modify its value in a data portal method.
        /// </summary>
        string DefaultAuthenticationType { get; }

        /// <summary>
        /// Gets a length of two factor authentication token (min 4, max 9).
        /// </summary>
        int TwoFactorTokenLength { get; }

        /// <summary>
        /// Gets a value indicating whether two factor authentication is always required.
        /// </summary>
        bool RequireTwoFactorAuthentication { get; }

        /// <summary>
        /// Gets a value (in minutes) indicating for how long a two factor authentication token remains valid.
        /// (min 5, max 30)
        /// </summary>
        int TwoFactorAuthenticationTokenExpiresIn { get; }

        /// <summary>
        /// Gets a length of a new (cryptographically random) password for a new user
        /// or for a password reset (min 6, max 9).
        /// </summary>
        int NewPasswordLength { get; }
               
        /// <summary>
        /// Gets a failed login count that initiates user lockout (min 3, max 10).
        /// </summary>
        int LockoutAfterFailedAttempts { get; }

        /// <summary>
        /// Gets a user lockout duration in minutes. (min 1, max 60)
        /// </summary>
        int LockoutDuration { get; }

        /// <summary>
        /// Gets a value (in minutes) indicating for how long a user password reset request remains valid.
        /// (min 15, max 60)
        /// </summary>
        int ResetPasswordRequestExpiresIn { get; }

        /// <summary>
        /// A subject of an email message asking a user to confirm his password reset.
        /// </summary>
        string ConfirmPasswordResetMessageSubject { get; }

        /// <summary>
        /// A (HTML) content of an email message asking a user to confirm his password reset.
        /// Placeholders: {0} - user name; {1} - new password; {2} - reset url.
        /// </summary>
        string ConfirmPasswordResetMessageContent { get; }

        /// <summary>
        /// An url template for user password reset (shall have {0} placeholder for request token).
        /// </summary>
        string ConfirmPasswordResetUrl { get; }

    }
}
