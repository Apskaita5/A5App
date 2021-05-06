using System;
using System.Security.Claims;
using System.Xml.Serialization;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Domain.Security
{
    /// <summary>
    /// A response to a user attempts to login an app server.
    /// </summary>
    [Serializable]
    public class LoginResponse
    {
        /// <summary>
        /// Creates a new LoginResponse instance for second factor authentication step.
        /// </summary>
        public LoginResponse()
        {
            Identity = null;
            Success = false;
            ProceedToSecondFactor = true;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// Creates a new LoginResponse instance for successful authentication.
        /// </summary>
        /// <param name="identity">a user identity object</param>
        public LoginResponse(ClaimsIdentity identity)
        {
            Identity = identity ?? throw new ArgumentNullException(nameof(identity));
            Success = true;
            ProceedToSecondFactor = false;
            ErrorMessage = string.Empty;
        }

        /// <summary>
        /// Creates a new LoginResponse instance for failed authentication.
        /// </summary>
        /// <param name="errorMessage">an error message for the user</param>
        public LoginResponse(string errorMessage)
        {
            if (errorMessage.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(errorMessage));
            Identity = null;
            Success = false;
            ProceedToSecondFactor = false;
            ErrorMessage = errorMessage;
        }


        /// <summary>
        /// whether the login succeeded
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// whether the second factor authentication step is required
        /// </summary> 
        public bool ProceedToSecondFactor { get; set; }

        /// <summary>
        /// an error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// an identity object for the user (if success)
        /// </summary>
        public ClaimsIdentity Identity { get; set; }

    }

}
