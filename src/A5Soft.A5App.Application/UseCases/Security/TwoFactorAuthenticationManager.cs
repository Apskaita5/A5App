using A5Soft.A5App.Application.Infrastructure;
using A5Soft.CARMA.Domain;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// Manages storage of second factor authentication token.
    /// </summary>
    public static class TwoFactorAuthenticationManager
    {
        private static readonly ConcurrentDictionary<string, (DateTime ExpiresAt, UserIdentity User)> _requests
            = new ConcurrentDictionary<string, (DateTime CreatedAt, UserIdentity User)>();

        /// <summary>
        /// Registers a new second factor authentication request and returns a generated token.
        /// </summary>
        /// <param name="user">a user to register the request for</param>
        /// <param name="randomProvider">a provider for cryptographically secure random values</param>
        /// <param name="policy">a security policy configured for the app server</param>
        /// <returns>generated url token and new password values</returns>
        public static string RegisterRequest(UserIdentity user, ISecureRandomProvider randomProvider, 
            ISecurityPolicy policy)
        {
            if (null == user) throw new ArgumentNullException(nameof(user));
            if (null == randomProvider) throw new ArgumentNullException(nameof(randomProvider));
            if (null == policy) throw new ArgumentNullException(nameof(policy));

            var tokenLength = policy.TwoFactorTokenLength;
            if (tokenLength < 4) tokenLength = 4;
            if (tokenLength > 9) tokenLength = 9;
            var expiresIn = policy.TwoFactorAuthenticationTokenExpiresIn;
            if (expiresIn < 5) expiresIn = 5;
            if (expiresIn > 30) expiresIn = 30;

            foreach (var currentToken in _requests
                .Where(v => v.Value.User.Id == user.Id)
                .Select(v => v.Key).ToList())
            {
                _requests.TryRemove(currentToken, out var _);
            }

            var result = randomProvider.CreateNewTwoFactorToken(tokenLength);
            var expiresAt = DateTime.UtcNow.AddMinutes(expiresIn);

            while (!_requests.TryAdd(result, (ExpiresAt: expiresAt, User: user)))
            {
                // refresh url token for a very unlikely event of same token generation
                result = randomProvider.CreateNewTwoFactorToken(tokenLength);
            }

            return result;
        }

        /// <summary>
        /// Gets a user for a second factor authentication request identified by the token specified.
        /// If no such second factor authentication request or it has expired, returns null.
        /// A second factor authentication request can only be consumed once.
        /// </summary>
        /// <param name="token">a second factor authentication token that identifies the user</param>
        public static UserIdentity ConsumeRequest(string token)
        {
            if (token.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(token));

            if (!_requests.TryRemove(token, out var request) 
                || request.ExpiresAt < DateTime.UtcNow) return null;

            return request.User;
        }

    }
}
