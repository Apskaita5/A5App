﻿using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization.Default;
using A5Soft.CARMA.Domain.Metadata.DataAnnotations;
using System;
using System.Security.Claims;

namespace A5Soft.A5App.Application
{
    /// <summary>
    /// Use cases marked with this attribute are only available for users with an admin privileges.
    /// </summary>
    public class AdminAuthorizationAttribute : AuthorizationBaseAttribute
    {
        private readonly LocalizableString _permissionName = new LocalizableString("PermissionName");
        

        /// <summary>
        /// Sets an admin permission that is required for the use case, i.e. only admin is allowed.
        /// </summary>
        /// <param name="resourceType">a type of the resource that is used for localization</param>
        /// <param name="permissionNameResourceName">a localization resource key for the permission name</param>
        public AdminAuthorizationAttribute(Type resourceType, string permissionNameResourceName)
            : base()
        {
            _permissionName.Value = permissionNameResourceName;
            _permissionName.ResourceType = resourceType;
        }
        

        /// <inheritdoc />
        public override bool AuthorizationImplementedForParam<TParam>() => false;

        protected override bool Authorize(ClaimsIdentity identity, Type useCaseType, ILogger logger, bool throwOnUnauthorized)
        {
            if (identity.IsAdmin()) return true;

            logger?.LogWarning($"User {identity.Name} ({identity.Email()}) " +
                $"has attempted an unauthorized action - {useCaseType?.FullName}.");

            if (!throwOnUnauthorized) return false;

            throw new AuthorizationException(_permissionName.GetLocalizableValue());
        }

        protected override bool Authorize<TParam>(ClaimsIdentity identity, Type useCaseType,
            TParam parameter, ILogger logger, bool throwOnUnauthorized)
            => Authorize(identity, useCaseType, logger, throwOnUnauthorized);

        protected override void ThrowNotAuthenticatedException()
        {
            throw new UnauthenticatedException();
        }
    }
}