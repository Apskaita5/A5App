using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Core;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// An exception that is thrown when a user invokes a use case without a required permission.
    /// </summary>
    public class AuthorizationException : BusinessException
    {
        /// <summary>
        /// An exception that is thrown when a user invokes a use case without a required permission.
        /// </summary>
        /// <param name="permissionName">a localized name of the permission that the user does not have</param>
        public AuthorizationException(string permissionName) 
            : base(string.Format(Resources.AuthorizationException_Message, permissionName)) { }
    }
}
