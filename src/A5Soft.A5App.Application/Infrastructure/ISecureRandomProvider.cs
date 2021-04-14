using A5Soft.A5App.Application.UseCases.Security;

namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// Provides methods to generate cryptographically secure random values.
    /// </summary>
    public interface ISecureRandomProvider
    {
        /// <summary>
        /// Gets a cryptographically secure random int value within the range specified.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        int GetSecureRandom(int min, int max);

        /// <summary>
        /// Creates a new cryptographically secure random password value.
        /// </summary>
        /// <param name="length"><see cref="ISecurityPolicy.NewPasswordLength"/></param>
        string CreateNewPassword(int length);

        /// <summary>
        /// Creates a new cryptographically secure, url safe random token value, e.g. for password reset.
        /// </summary>
        string CreateSecureUrlToken();

        /// <summary>
        /// Creates a new cryptographically secure two factor authentication token value.
        /// </summary>
        /// <param name="length"><see cref="ISecurityPolicy.TwoFactorTokenLength"/></param>
        string CreateNewTwoFactorToken(int length);
    }
}
