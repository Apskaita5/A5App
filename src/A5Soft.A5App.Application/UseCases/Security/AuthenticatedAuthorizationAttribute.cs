using System;
using System.Security.Claims;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization.Default;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// Use cases marked with this attribute are available for all authenticated users.
    /// </summary>
    public class AuthenticatedAuthorizationAttribute : AuthorizationBaseAttribute
    {
        protected override bool Authorize(ClaimsIdentity identity, Type useCaseType,
            ILogger logger, bool throwOnUnauthorized) => true;

        
        protected override void ThrowNotAuthenticatedException()
        {
            throw new UnauthenticatedException();
        }

    }
}
