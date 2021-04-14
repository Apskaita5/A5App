using System;
using System.Collections.Concurrent;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// Manages user lockouts due to failed login attempts.
    /// </summary>
    internal static class LockoutManager
    {
        private static readonly ConcurrentDictionary<string, DateTime> _lockedOutUsers
            = new ConcurrentDictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        private static readonly ConcurrentDictionary<string, int> _failedAttempts
            = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Registers failed attempt to login.
        /// </summary>
        /// <param name="userEmail">an email of the user whose login attempt failed</param>
        /// <param name="policy">security policy configured for the app server</param>
        public static void RegisterFailedAttempt(string userEmail, ISecurityPolicy policy)
        {
            if (userEmail.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(userEmail));

            var lockoutAfter = policy.LockoutAfterFailedAttempts;
            if (lockoutAfter < 3) lockoutAfter = 3;
            if (lockoutAfter > 10) lockoutAfter = 10;
            var actualLockoutDuration = policy.LockoutDuration;
            if (actualLockoutDuration < 1) actualLockoutDuration = 1;
            if (actualLockoutDuration > 60) actualLockoutDuration = 60;

            var failedAttempts = _failedAttempts.AddOrUpdate(userEmail.Trim(), 
                1, (s, v) => v += 1);

            if (failedAttempts >= lockoutAfter)
            {
                _failedAttempts.TryRemove(userEmail.Trim(), out int _);
                var lockoutEndsAt = DateTime.UtcNow.AddMinutes(actualLockoutDuration);
                _lockedOutUsers.AddOrUpdate(userEmail.Trim(),
                    lockoutEndsAt, (s, v) => lockoutEndsAt);
            }
        }

        /// <summary>
        /// Registers successful login (clears failed attempt count).
        /// </summary>
        /// <param name="userEmail">an email of the user who logged in successfully</param>
        public static void RegisterSuccessfulLogin(string userEmail)
        {
            if (userEmail.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(userEmail));

            _failedAttempts.TryRemove(userEmail.Trim(), out int _);
            _lockedOutUsers.TryRemove(userEmail.Trim(), out DateTime _);
        }

        /// <summary>
        /// Gets an UTC date and time when the user lockout ends.
        /// Returns null if the user is not locked out.
        /// </summary>
        /// <param name="userEmail">an email of the user to get the lockout end time for</param>
        public static DateTime? LockoutEndsAt(string userEmail)
        {
            if (userEmail.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(userEmail));

            if (_lockedOutUsers.TryGetValue(userEmail.Trim(), out DateTime result))
            {
                if (result < DateTime.UtcNow)
                {
                    _lockedOutUsers.TryRemove(userEmail.Trim(), out DateTime _);
                    return null;
                }

                return result;
            }

            return null;
        }

    }
}
