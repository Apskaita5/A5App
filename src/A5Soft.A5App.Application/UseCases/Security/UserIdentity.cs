using A5Soft.A5App.Domain.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// User identity data used for authentication and authorization.
    /// </summary>
    public class UserIdentity
    {
        protected Guid _id;
        protected Guid? _groupId;
        protected Guid? _tenantId;
        protected string _groupName;
        protected string _name;
        protected string _email;
        protected string _phone;
        protected bool _isSuspended;
        protected bool _isDisabled;
        protected bool _twoFactorEnabled;
        protected AdministrativeRole _adminRole;
        protected List<Guid> _permissions;
        private string _occHash;
        [NonSerialized]
        protected DateTime _updatedAt;
        


        protected UserIdentity() { }


        /// <inheritdoc cref="IUser.Id"/>
        [Key]
        public Guid Id => _id;

        /// <inheritdoc cref="IUser.UserGroupId"/>
        public Guid? GroupId => _groupId;

        /// <summary>
        /// an id of the tenant that the user is logged in to
        /// </summary>
        public Guid? TenantId => _tenantId;

        /// <inheritdoc cref="IUser.UserGroupId"/>
        public string GroupName => _groupName;

        /// <inheritdoc cref="IUser.Name"/>
        public string Name => _name;

        /// <inheritdoc cref="IUser.Email"/>
        public string Email => _email;

        /// <inheritdoc cref="IUser.Phone"/>
        public string Phone => _phone;

        /// <inheritdoc cref="IUser.IsSuspended"/>
        public bool IsSuspended => _isSuspended;

        /// <inheritdoc cref="IUser.IsDisabled"/>
        public bool IsDisabled => _isDisabled;

        /// <inheritdoc cref="IUser.TwoFactorEnabled"/>
        public bool TwoFactorEnabled => _twoFactorEnabled;

        /// <inheritdoc cref="IUser.AdminRole"/>
        public AdministrativeRole AdminRole => _adminRole;

        /// <inheritdoc cref="IAuditable.OccHash"/>
        public string OccHash => _occHash;

        /// <summary>
        /// Permissions that the user has for the current tenant.
        /// </summary>  
        public List<Guid> Permissions => _permissions;


        protected void SetOccHash()
        {
            _occHash = _updatedAt.CreateOccHash<User>();
            _updatedAt = DateTime.MinValue;
        }


        /// <summary>
        /// Gets a claim identity instance that encapsulates the app user identity and
        /// authorization data.
        /// </summary>
        /// <param name="policy">security policy configured by the app server</param>
        public ClaimsIdentity ToClaimsIdentity(ISecurityPolicy policy)
        {
            if (null == policy) throw new ArgumentNullException(nameof(policy));

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, _email, ClaimValueTypes.Email),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1)
                    .ToString(CultureInfo.InvariantCulture), ClaimValueTypes.DateTime),
                new Claim(ClaimTypes.Name, _name, ClaimValueTypes.String),
                new Claim(ClaimTypes.MobilePhone, _phone, ClaimValueTypes.String),
                new Claim(ClaimTypes.Sid, _id.ToString("N"), ClaimValueTypes.Sid),
                new Claim(ApplicationClaimTypes.OccHash, _occHash, ClaimValueTypes.String)
            };

            if (_groupId.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.GroupSid,
                    _groupId.Value.ToString("N"), ClaimValueTypes.Sid));
                claims.Add(new Claim(ApplicationClaimTypes.GroupName, _groupName, 
                    ClaimValueTypes.String));
            }

            if (_tenantId.HasValue)
            {
                claims.Add(new Claim(ApplicationClaimTypes.TenantSid, _tenantId.Value.ToString("N"), 
                    ClaimValueTypes.Sid));
            }

            if (_isSuspended)
            {
                claims.Add(new Claim(ApplicationClaimTypes.Suspended, string.Empty,
                    ClaimValueTypes.Boolean));
            }

            if (_adminRole == AdministrativeRole.Admin)
            {
                claims.Add(new Claim(ApplicationClaimTypes.Admin, string.Empty,
                    ClaimValueTypes.Boolean));
            }

            if (_adminRole == AdministrativeRole.GroupAdmin)
            {
                claims.Add(new Claim(ApplicationClaimTypes.GroupAdmin, string.Empty,
                    ClaimValueTypes.Boolean));
            }

            foreach (var permission in _permissions)
            {
                claims.Add(new Claim(ApplicationClaimTypes.Permission + permission.ToString("N"), 
                    string.Empty, ClaimValueTypes.Boolean));
            }

            claims.Add(new Claim(ApplicationClaimTypes.SecurityToken,
                claims.CreateSecurityToken(policy.ServerSecret), ClaimValueTypes.Boolean));
            
            return new ClaimsIdentity(claims, policy.DefaultAuthenticationType);
        }

    }
}
