using A5Soft.A5App.Domain.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        
    }
}
