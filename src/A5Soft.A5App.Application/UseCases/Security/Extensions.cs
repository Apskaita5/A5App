using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    public static class Extensions
    {

        /// <summary>
        /// Gets an internal ID of the user.
        /// </summary>
        /// <param name="identity"></param>
        public static Guid Sid(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));
            
            var result = identity.FindFirst(ClaimTypes.Sid)?.Value;
            if (result.IsNullOrWhiteSpace()) throw new InvalidOperationException(
                "ClaimsIdentity state corrupted: no SID value.");
            
            return new Guid(result);
        }

        /// <summary>
        /// Gets an UTC date and time when the identity expires, i.e. can no longer be used
        /// for subsequent requests.
        /// </summary>
        /// <param name="identity"></param>
        public static DateTime Expiration(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var result = identity.FindFirst(ClaimTypes.Expiration)?.Value;
            if (result.IsNullOrWhiteSpace()) throw new InvalidOperationException(
                "ClaimsIdentity state corrupted: no expiration value.");

            return DateTime.SpecifyKind(DateTime.Parse(result, CultureInfo.InvariantCulture), 
                DateTimeKind.Utc);
        }

        /// <summary>
        /// Gets a phone No of the user (if any).
        /// </summary>
        /// <param name="identity"></param>
        public static string Phone(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var result = identity.FindFirst(ClaimTypes.MobilePhone)?.Value;
            
            return result ?? string.Empty;
        }

        /// <summary>
        /// Gets an email No of the user (if any).
        /// </summary>
        /// <param name="identity"></param>
        public static string Email(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var result = identity.FindFirst(ClaimTypes.Email)?.Value;
            if (result.IsNullOrWhiteSpace()) throw new InvalidOperationException(
                "ClaimsIdentity state corrupted: no user email.");

            return result;
        }

        /// <summary>
        /// Gets a hash value for last updated time stamp so that an app server
        /// can invalidated the identity if the respective user data has changed.
        /// </summary>
        /// <param name="identity"></param>
        public static string OccHash(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var result = identity.FindFirst(ClaimTypes.Expiration)?.Value;
            if (result.IsNullOrWhiteSpace()) throw new InvalidOperationException(
                "ClaimsIdentity state corrupted: no OCC hash value.");

            return result;
        }

        /// <summary>
        /// Gets an ID of the user group that the user belongs to (if any).
        /// </summary>
        /// <param name="identity"></param>
        public static Guid? GroupSid(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var result = identity.FindFirst(ClaimTypes.GroupSid)?.Value;

            if (result.IsNullOrWhiteSpace())
            {
                if (identity.IsGroupAdmin()) throw new InvalidOperationException(
                    $"ClaimsIdentity is corrupt: user {identity.Name} is a group admin yet group id is null.");
                return null;
            }

            return new Guid(result);
        }

        /// <summary>
        /// Gets a name of the user group that the user belongs to (if any).
        /// </summary>
        /// <param name="identity"></param>
        public static string GroupName(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var result = identity.FindFirst(ApplicationClaimTypes.GroupName)?.Value;

            return result ?? string.Empty;
        }

        /// <summary>
        /// Gets an ID of the tenant that the user is logged in to (if any).
        /// </summary>
        /// <param name="identity"></param>
        public static Guid? TenantSid(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var result = identity.FindFirst(ApplicationClaimTypes.TenantSid)?.Value;

            if (result.IsNullOrWhiteSpace()) return null;

            return new Guid(result);
        }

        /// <summary>
        /// Gets a value indicating whether the user is suspended, i.e. only have
        /// read privileges irrespective of the permissions.
        /// </summary>
        /// <param name="identity"></param>
        /// <remarks>should be implemented by authorization engine/attributes</remarks>
        public static bool IsSuspended(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var result = identity.FindFirst(ApplicationClaimTypes.Suspended);

            return (null != result);
        }

        /// <summary>
        /// Gets a value indicating whether the user is an app admin.
        /// </summary>
        /// <param name="identity"></param>
        public static bool IsAdmin(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var result = identity.FindFirst(ApplicationClaimTypes.Admin);

            return (null != result);
        }

        /// <summary>
        /// Gets a value indicating whether the user is a user group admin.
        /// </summary>
        /// <param name="identity"></param>
        public static bool IsGroupAdmin(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var result = identity.FindFirst(ApplicationClaimTypes.GroupAdmin);

            return (null != result);
        }

        /// <summary>
        /// Gets a security token for the identity.
        /// </summary>
        /// <param name="identity"></param>
        internal static string SecurityToken(this ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            var result = identity.FindFirst(ApplicationClaimTypes.SecurityToken)?.Value;
            if (result.IsNullOrWhiteSpace()) throw new InvalidOperationException(
                "ClaimsIdentity state corrupted: no security token.");

            return result;
        }

        /// <summary>
        /// Gets a value indicating whether the user has the permission specified.
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="permission">an id of the permission to check for</param>
        public static bool HasPermission(this ClaimsIdentity identity, Guid permission)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));

            if (identity.IsAdmin() || identity.IsGroupAdmin()) return true;

            var result = identity.FindFirst(ApplicationClaimTypes.Permission + permission.ToString("N"));

            return (null != result);
        }


        /// <summary>
        /// Returns true if the identity claims were signed using serverSecret value.
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="serverSecret"><see cref="ISecurityPolicy.ServerSecret"/></param>
        internal static bool IsValid(this ClaimsIdentity identity, string serverSecret)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));
            if (serverSecret.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(serverSecret));

            var securityToken = identity.FindFirst(ApplicationClaimTypes.SecurityToken)?.Value;
            if (securityToken.IsNullOrWhiteSpace()) return false;

            var validSecurityToken = identity.Claims.CreateSecurityToken(serverSecret);

            return validSecurityToken == securityToken;
        }

        /// <summary>
        /// Creates a SecurityToken value for the claims collection specified using
        /// the serverSecret as a salt.
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="serverSecret"><see cref="ISecurityPolicy.ServerSecret"/></param>
        internal static string CreateSecurityToken(this IEnumerable<Claim> claims, string serverSecret)
        {
            return serverSecret + "|" + string.Join("|",
                claims.Where(c => c.IsApplicationClaim())
                    .OrderBy(c => c.Type).ThenBy(c => c.Value)
                    .Select(c => $"{c.Type}:{c.Value}")).ComputeSha256Hash();
        }


        private static readonly string[] CommonApplicationClaims = new string[]
            { ClaimTypes.Email, ClaimTypes.Expiration, ClaimTypes.Name, ClaimTypes.MobilePhone,
            ClaimTypes.Sid, ClaimTypes.GroupSid};

        private static bool IsApplicationClaim(this Claim claim, bool includeSecurityToken = false)
        {
            if (Array.IndexOf(CommonApplicationClaims, claim.Type) > -1) return true;
            return claim.Type.StartsWith(ApplicationClaimTypes.Namespace) &&
                (includeSecurityToken || claim.Type != ApplicationClaimTypes.SecurityToken);
        }

    }
}
