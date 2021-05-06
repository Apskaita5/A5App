using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security.ClaimsIdentityExtensions
{
    /// <summary>
    /// Extensions methods for creating and validating ClaimsIdentity instances for app users.
    /// </summary>
    public static class Extensions
    {
        private static readonly string[] CommonApplicationClaims = new string[]
        { ClaimTypes.Email, ClaimTypes.Expiration, ClaimTypes.Name, ClaimTypes.MobilePhone,
            ClaimTypes.Sid, ClaimTypes.GroupSid};


        /// <summary>
        /// Creates a Claims identity instance for the user identity specified.
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="securityPolicy">a security policy that defines (inter alia) identity expiration period</param>
        /// <param name="securityTokenProvider">a security token provider that issues a verifiable
        /// security token for the ClaimsIdentity state ("signs" the ClaimsIdentity)</param>
        /// <returns>a Claims identity instance for the user identity specified</returns>
        public static async Task<ClaimsIdentity> ToClaimsIdentityAsync(this UserIdentity identity, 
            ISecurityPolicy securityPolicy, ISecurityTokenProvider securityTokenProvider)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));
            if (null == securityPolicy) throw new ArgumentNullException(nameof(securityPolicy));
            if (null == securityTokenProvider) throw new ArgumentNullException(nameof(securityTokenProvider));


            var expiresIn = securityPolicy.IdentityExpiresIn;
            if (expiresIn < 1) expiresIn = 24;
            if (expiresIn > 168) expiresIn = 168;

            var result = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, identity.Email, ClaimValueTypes.Email),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddHours(expiresIn)
                    .ToString(CultureInfo.InvariantCulture), ClaimValueTypes.DateTime),
                new Claim(ClaimTypes.Name, identity.Name, ClaimValueTypes.String),
                new Claim(ClaimTypes.MobilePhone, identity.Phone, ClaimValueTypes.String),
                new Claim(ClaimTypes.Sid, identity.Id.ToString("N"), ClaimValueTypes.Sid),
                new Claim(ApplicationClaimTypes.OccHash, identity.OccHash, ClaimValueTypes.String)
            };

            if (identity.GroupId.HasValue)
            {
                result.Add(new Claim(ClaimTypes.GroupSid,
                    identity.GroupId.Value.ToString("N"), ClaimValueTypes.Sid));
                result.Add(new Claim(ApplicationClaimTypes.GroupName, identity.GroupName,
                    ClaimValueTypes.String));
            }

            if (identity.TenantId.HasValue)
            {
                result.Add(new Claim(ApplicationClaimTypes.TenantSid, identity.TenantId.Value.ToString("N"),
                    ClaimValueTypes.Sid));
            }

            if (identity.IsSuspended)
            {
                result.Add(new Claim(ApplicationClaimTypes.Suspended, string.Empty,
                    ClaimValueTypes.Boolean));
            }

            if (identity.AdminRole == AdministrativeRole.Admin)
            {
                result.Add(new Claim(ApplicationClaimTypes.Admin, string.Empty,
                    ClaimValueTypes.Boolean));
            }

            if (identity.AdminRole == AdministrativeRole.GroupAdmin)
            {
                result.Add(new Claim(ApplicationClaimTypes.GroupAdmin, string.Empty,
                    ClaimValueTypes.Boolean));
            }

            foreach (var permission in identity.Permissions)
            {
                result.Add(new Claim(ApplicationClaimTypes.Permission + permission.ToString("N"),
                    string.Empty, ClaimValueTypes.Boolean));
            }

            var securityToken = await securityTokenProvider.CreateTokenAsync(
                result.ToApplicationClaimsString());

            result.Add(new Claim(ApplicationClaimTypes.SecurityToken, 
                securityToken, ClaimValueTypes.String));

            return new ClaimsIdentity(result, securityPolicy.DefaultAuthenticationType);
        }

        /// <summary>
        /// Creates a ClaimsIdentity instance that is the same as specified excluding tenant specific claims.
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="securityTokenProvider">a security token provider that issues a verifiable
        /// security token for the ClaimsIdentity state ("signs" the ClaimsIdentity)</param>
        /// <returns>a ClaimsIdentity instance that is the same as specified excluding tenant specific claims</returns>
        public static async Task<ClaimsIdentity> ToLoggedOutClaimsIdentityAsync(this ClaimsIdentity identity,
            ISecurityTokenProvider securityTokenProvider)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));
            if (null == securityTokenProvider) throw new ArgumentNullException(nameof(securityTokenProvider));

            var claims = identity.Claims
                .Where(c => !c.Type.Equals(ApplicationClaimTypes.TenantSid, StringComparison.OrdinalIgnoreCase)
                && !c.Type.Equals(ApplicationClaimTypes.SecurityToken, StringComparison.OrdinalIgnoreCase)
                && !c.Type.StartsWith(ApplicationClaimTypes.Permission, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var securityToken = await securityTokenProvider.CreateTokenAsync(
                claims.ToApplicationClaimsString());

            claims.Add(new Claim(ApplicationClaimTypes.SecurityToken,
                securityToken, ClaimValueTypes.String));

            return new ClaimsIdentity(claims, identity.AuthenticationType);
        }

        /// <summary>
        /// Returns true if the identity claims were signed by the app server.
        /// </summary>
        /// <param name="identity">an identity to validate</param>
        /// <param name="provider"></param>
        public static async Task<bool> IsValidAsync(this ISecurityTokenProvider provider, ClaimsIdentity identity)
        {
            if (null == identity) throw new ArgumentNullException(nameof(identity));
            if (null == provider) throw new ArgumentNullException(nameof(provider));

            var securityToken = identity.FindFirst(ApplicationClaimTypes.SecurityToken)?.Value;
            if (securityToken.IsNullOrWhiteSpace()) return false;

            return await provider.ValidateTokenAsync(securityToken,
                identity.Claims.ToApplicationClaimsString());
        }

        /// <summary>
        /// Creates a new ClaimsIdentity instance for a local (desktop) user.
        /// </summary>
        /// <param name="tenantId">a tenant id (if logged to a tenant database)</param>
        /// <param name="email">local user email</param>
        /// <param name="name">local user name</param>
        /// <param name="phone">local user phone no</param>
        /// <returns>a new ClaimsIdentity instance for a local (desktop) user</returns>
        public static ClaimsIdentity LocalUserIdentity(string email, string name, string phone, Guid? tenantId)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email ?? string.Empty, ClaimValueTypes.Email),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddYears(10)
                    .ToString(CultureInfo.InvariantCulture), ClaimValueTypes.DateTime),
                new Claim(ClaimTypes.Name, name ?? string.Empty, ClaimValueTypes.String),
                new Claim(ClaimTypes.MobilePhone, phone ?? string.Empty, ClaimValueTypes.String),
                new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString("N"), ClaimValueTypes.Sid),
                new Claim(ApplicationClaimTypes.OccHash, string.Empty, ClaimValueTypes.String),
                new Claim(ApplicationClaimTypes.Admin, string.Empty, ClaimValueTypes.Boolean)
            };

            if (tenantId.HasValue)
            {
                claims.Add(new Claim(ApplicationClaimTypes.TenantSid, tenantId.Value.ToString("N"),
                    ClaimValueTypes.Sid));
            }

            return new ClaimsIdentity(claims, "LocalUser");
        }


        private static string ToApplicationClaimsString(this IEnumerable<Claim> claims)
        {
            return string.Join("|", claims
                .Where(c => c.IsApplicationClaim())
                .OrderBy(c => c.Type).ThenBy(c => c.Value)
                .Select(c => $"{c.Type}:{c.Value}"));
        }

        private static bool IsApplicationClaim(this Claim claim, bool includeSecurityToken = false)
        {
            if (Array.IndexOf(CommonApplicationClaims, claim.Type) > -1) return true;
            return claim.Type.StartsWith(ApplicationClaimTypes.Namespace) &&
                (includeSecurityToken || claim.Type != ApplicationClaimTypes.SecurityToken);
        }

    }
}
