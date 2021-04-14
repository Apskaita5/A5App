using A5Soft.A5App.Domain.Core;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace A5Soft.A5App.Domain.Security
{
    /// <inheritdoc cref="ICustomUserRole"/>
    [Serializable]
    public sealed class CustomUserRole : DomainEntity<CustomUserRole>, ICustomUserRole
    {
        #region Private Fields

        private readonly IDomainEntityIdentity _userId;
        private readonly string _userName;
        private readonly IDomainEntityIdentity _userGroupId;
        private readonly IDomainEntityIdentity _tenantId;
        private readonly string _tenantName;
        private readonly DomainBindingList<UserPermission> _permissions;
        private readonly string _occHash;
        [NonSerialized]
        private readonly string _updatedBy;

        #endregion

        #region Constructors

        /// <inheritdoc />
        public CustomUserRole(IValidationEngineProvider validationEngineProvider) : base(validationEngineProvider)
        {
            _permissions = new DomainBindingList<UserPermission>(validationEngineProvider)
            {
                AllowNew = false,
                AllowRemove = false
            };
            RegisterChildValue(_permissions, nameof(_permissions), true);
        }

        /// <summary>
        /// Creates a new instance for an existing entity.
        /// </summary>
        /// <param name="dto">DTO containing the entity data</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public CustomUserRole(CustomUserRoleDto dto, IValidationEngineProvider validationEngineProvider)
            : base(dto.Id, validationEngineProvider)
        {
            _userId = dto.UserId;
            _userName = dto.UserName;
            _userGroupId = dto.UserGroupId;
            _tenantId = dto.TenantId;
            _tenantName = dto.TenantName;
            _permissions = new DomainBindingList<UserPermission>(validationEngineProvider)
            {
                AllowNew = false,
                AllowRemove = false
            };
            _permissions.AddRange(dto.Permissions.Select(d => new UserPermission(d, validationEngineProvider)));
            RegisterChildValue(_permissions, nameof(_permissions), true);
            _updatedBy = dto.UpdatedBy;
            _occHash = dto.UpdatedAt.CreateOccHash<User>();
        }

        /// <inheritdoc />
        protected override IDomainEntityIdentity CreateNewIdentity()
        {
            return new IntDomainEntityIdentity(typeof(CustomUserRole));
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="ICustomUserRole.UserId"/>
        public IDomainEntityIdentity UserId => _userId;

        /// <inheritdoc cref="ICustomUserRole.UserGroupId"/>
        public IDomainEntityIdentity UserGroupId => _userGroupId;

        /// <inheritdoc cref="ICustomUserRole.UserName"/>
        public string UserName => _userName;

        /// <inheritdoc cref="ICustomUserRole.TenantId"/>
        public IDomainEntityIdentity TenantId => _tenantId;

        /// <inheritdoc cref="ICustomUserRole.TenantName"/>
        public string TenantName => _tenantName;

        /// <inheritdoc cref="ICustomUserRole.Permissions"/>
        public DomainBindingList<UserPermission> Permissions => _permissions;

        /// <inheritdoc cref="ICustomUserRole.Permissions"/>
        IList<IUserPermission> ICustomUserRole.Permissions 
            => _permissions.Select(p => (IUserPermission) p).ToList();

        /// <inheritdoc cref="ICustomUserRole.OccHash"/> 
        public string OccHash
            => _occHash;

        #endregion

        #region Methods

        /// <summary>
        /// Exports entity data to DTO (variation of memento pattern)
        /// </summary>
        /// <returns>a DTO that contains the data of the CustomUserRole instance to persist</returns>
        public CustomUserRoleDto ToDto()
        {
            return new CustomUserRoleDto(this);
        }

        /// <summary>
        /// Merges business data coming from an untrusted source into the entity;
        /// subject to <paramref name="validate"/> param throws an exception if the entity becomes invalid
        /// </summary>
        /// <param name="entity">business data coming from an untrusted source</param>
        /// <param name="validate">whether to throw an exception if the entity becomes invalid</param>
        /// <exception cref="ConcurrencyException">if there is a concurrency conflict</exception>
        /// <exception cref="ValidationException">if the entity becomes invalid after merge</exception>
        /// <exception cref="ArgumentNullException">if the <paramref name="entity"/> is null</exception>
        public void Merge(ICustomUserRole entity, bool validate = true)
        {
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));

            var selectedPermissions = entity.Permissions
                .Where(p => p.Assigned)
                .Select(p => (Guid)p.Id.IdentityValue)
                .ToList();

            using (SuspendBindings())
            {
                using (SuspendValidation())
                {
                    foreach (var userPermission in _permissions)
                    {
                        userPermission.Assigned = selectedPermissions
                            .Contains((Guid)userPermission.Id.IdentityValue);
                    }
                }
            }

            if (validate)
            {
                this.CheckRules();
                if (!this.IsValid) throw new ValidationException(this.GetBrokenRulesTree());
                if (entity.OccHash != _occHash) throw new ConcurrencyException(_updatedBy);
            }
        }

        #endregion

        #region DTO

        /// <summary>
        /// DTO for <see cref="CustomUserRole"/> persistence (memento pattern variation).
        /// </summary> 
        [Serializable]
        public class CustomUserRoleDto : DomainEntityDto
        {
            /// <inheritdoc />
            public CustomUserRoleDto() : base() { }

            /// <inheritdoc />
            public CustomUserRoleDto(CustomUserRole entity) : base(entity)
            {
                UserId = entity.UserId;
                UserName = entity.UserName;
                UserGroupId = entity.UserGroupId;
                TenantId = entity.TenantId;
                TenantName = entity.TenantName;
                Permissions = entity.Permissions.Select(i => i.ToDto()).ToList();
            }


            /// <inheritdoc cref="ICustomUserRole.UserId"/>
            public IDomainEntityIdentity UserId { get; set; }

            /// <inheritdoc cref="ICustomUserRole.UserGroupId"/>
            public IDomainEntityIdentity UserGroupId { get; set; }

            /// <inheritdoc cref="ICustomUserRole.UserName"/>
            public string UserName { get; set; }

            /// <inheritdoc cref="ICustomUserRole.TenantId"/>
            public IDomainEntityIdentity TenantId { get; set; }

            /// <inheritdoc cref="ICustomUserRole.TenantName"/>
            public string TenantName { get; set; }

            /// <inheritdoc cref="ICustomUserRole.Permissions"/>
            public List<UserPermission.UserPermissionDto> Permissions { get; set; }

            /// <inheritdoc cref="IAuditable.UpdatedAt"/>  
            public DateTime UpdatedAt { get; set; }

            /// <inheritdoc cref="IAuditable.UpdatedBy"/>  
            public string UpdatedBy { get; set; }

        }

        #endregion
    }
}
