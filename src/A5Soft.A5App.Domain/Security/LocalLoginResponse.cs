using A5Soft.CARMA.Domain;
using System;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// A response to a user attempt to login a local (desktop) database.
    /// </summary>
    /// <remarks>on success a local user identity could be created by a static UserIdentity.LocalUserIdentity</remarks>
    [Serializable]
    public class LocalLoginResponse
    {
        /// <summary>
        /// Creates a new LoginResponse instance for success or database password request.
        /// </summary>
        public LocalLoginResponse(bool requestPassword)
        {
            Success = !requestPassword;
            RequirePassword = requestPassword;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// Creates a new LoginResponse instance for failed authentication.
        /// </summary>
        /// <param name="errorMessage">an error message for the user</param>
        public LocalLoginResponse(string errorMessage)
        {
            if (errorMessage.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(errorMessage));
            Success = false;
            RequirePassword = false;
            ErrorMessage = errorMessage;
        }


        /// <summary>
        /// whether the login succeeded
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// whether the local database is encrypted (requires password)
        /// </summary> 
        public bool RequirePassword { get; set; }

        /// <summary>
        /// an error message
        /// </summary>
        public string ErrorMessage { get; set; }

    }
}
