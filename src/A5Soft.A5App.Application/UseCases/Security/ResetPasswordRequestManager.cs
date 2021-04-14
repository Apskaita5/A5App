using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// Manages user requests to reset their passwords.
    /// </summary>
    internal static class ResetPasswordRequestManager
    {
        private static readonly ConcurrentDictionary<string, ResetPasswordRequest> _requests
            = new ConcurrentDictionary<string, ResetPasswordRequest>();


        /// <summary>
        /// Registers a new reset password request and returns generated url token and new password values.
        /// </summary>
        /// <param name="userId">an id of the user to register the request for</param>
        /// <param name="randomProvider">a provider for cryptographically secure random values</param>
        /// <param name="policy">a security policy configured for the app server</param>
        /// <returns>generated url token and new password values</returns>
        public static (string UrlToken, string NewPassword) RegisterRequest(Guid userId, 
            ISecureRandomProvider randomProvider, ISecurityPolicy policy)
        {
            if (null == randomProvider) throw new ArgumentNullException(nameof(randomProvider));
            if (null == policy) throw new ArgumentNullException(nameof(policy));

            var passwordLength = policy.NewPasswordLength;
            if (passwordLength < 6) passwordLength = 6;
            if (passwordLength > 9) passwordLength = 9;
            var expiresIn = policy.ResetPasswordRequestExpiresIn;
            if (expiresIn < 15) expiresIn = 15;
            if (expiresIn > 60) expiresIn = 60;

            foreach (var token in _requests
                .Where(r => r.Value.UserId == userId)
                .Select(r => r.Key).ToList())
            {
                _requests.TryRemove(token, out var _);
            }

            var newPassword = randomProvider.CreateNewPassword(passwordLength);
            var urlToken = randomProvider.CreateSecureUrlToken();
            var request = new ResetPasswordRequest(userId, newPassword, expiresIn);

            while (!_requests.TryAdd(urlToken, request))
            {
                // refresh url token for a very unlikely event of same token generation
                urlToken = randomProvider.CreateSecureUrlToken();
            }

            return (UrlToken: urlToken, NewPassword: newPassword);
        }

        /// <summary>
        /// Gets a user id and new password for a password reset request identified by the url token specified.
        /// If no such password reset request or it has expired, returns null for user id.
        /// A password reset request can only be consumed once.
        /// </summary>
        /// <param name="urlToken">an url token that identifies a password reset request</param>
        public static (Guid? UserId, string NewPassword) ConsumeRequest(string urlToken)
        {
            if (urlToken.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(urlToken));

            if (!_requests.TryRemove(urlToken, out ResetPasswordRequest request))
                return (UserId: null, NewPassword: string.Empty);

            if (request.ExpiresAt < DateTime.UtcNow)
                return (UserId: null, NewPassword: string.Empty);

            return (request.UserId, request.NewPassword);
        }


        private class ResetPasswordRequest
        {
            public ResetPasswordRequest(Guid userId, string newPassword,
                int resetPasswordRequestExpiresIn)
            {
                UserId = userId;
                NewPassword = newPassword;
                ExpiresAt = DateTime.UtcNow.AddMinutes(resetPasswordRequestExpiresIn);
            }


            public DateTime ExpiresAt { get; }
                                              
            public Guid UserId { get; }

            public string NewPassword { get; }

        }

    }
}
