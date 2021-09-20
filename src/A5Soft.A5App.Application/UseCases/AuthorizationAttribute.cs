using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization.Default;
using System;
using System.Security.Claims;
using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Domain.Reflection;

namespace A5Soft.A5App.Application.UseCases
{
    /// <summary>
    /// Use cases marked with this attribute are only available for users that have the respective permission
    /// (and users with any administrative privileges).
    /// </summary>
    public class AuthorizationAttribute : AuthorizationBaseAttribute
    {
        private readonly Permission _permission;


        /// <summary>
        /// Sets a permission that is required for the use case.
        /// Due to limitations of attribute constructor params cannot pass Permission instance.
        /// </summary>
        /// <param name="permissionType">a type of the required permission</param>
        public AuthorizationAttribute(Type permissionType) : base()
        {
            if (null == permissionType) throw new ArgumentNullException(nameof(permissionType));
            if (!typeof(Permission).IsAssignableFrom(permissionType)) throw new ArgumentException(
                $"Type {permissionType.FullName} is not a descendant of Permission.",
                nameof(permissionType));

            _permission = (Permission)ObjectActivator.CreateInstance(permissionType);
        }


        protected override bool Authorize(ClaimsIdentity identity, Type useCaseType, 
            ILogger logger, bool throwOnUnauthorized)
        {
            if (!identity.TenantSid().HasValue)
            {
                throw new InvalidOperationException($"Tenant specific use case {useCaseType?.FullName} " +
                    $"has been invoked by a user not logged in to any tenant database.");
            }

            if (identity.HasPermission(_permission.Id) && (!identity.IsSuspended() 
                || _permission.AllowForSuspendedUser)) return true;

            logger?.LogWarning($"User {identity.Name} ({identity.Email()}) " +
                $"has attempted an unauthorized action - {useCaseType?.FullName}.");

            if (!throwOnUnauthorized) return false;

            throw new AuthorizationException(_permission.Name);
        }

        protected override void ThrowNotAuthenticatedException()
        {
            throw new UnauthenticatedException();
        }
    }
}
