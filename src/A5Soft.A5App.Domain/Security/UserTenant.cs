using A5Soft.A5App.Domain.Core;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using A5Soft.CARMA.Domain.Metadata.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using A5Soft.A5App.Domain.Properties;

namespace A5Soft.A5App.Domain.Security
{
    /// <inheritdoc cref="IUserTenant"/>
    [Serializable]
    public sealed class UserTenant : DomainEntity<UserTenant>, IUserTenant
    {
        #region Private Fields

        private readonly IDomainEntityIdentity _tenantId;
        private readonly string _tenantName;
        private UserRoleLookup _role;
        [NonSerialized]
        private bool _hadCustomRole;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance for a new entity.
        /// </summary>
        /// <param name="tenant">a tenant to create a new entity for</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public UserTenant(TenantLookup tenant, IValidationEngineProvider validationEngineProvider)
            : base(validationEngineProvider)
        {
            if (tenant?.Id?.IsNew ?? true) throw new ArgumentNullException(nameof(tenant));

            _tenantId = tenant.Id;
            _tenantName = tenant.Name;
        }

        /// <summary>
        /// Creates a new instance for an existing entity.
        /// </summary>
        /// <param name="dto">DTO containing the entity data</param>
        /// <param name="tenants">a lookup for the tenant data</param>
        /// <param name="roles">a lookup for the roles data</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public UserTenant(UserTenantDto dto, IEnumerable<TenantLookup> tenants, 
            IEnumerable<UserRoleLookup> roles, IValidationEngineProvider validationEngineProvider)
            : base(dto?.Id, validationEngineProvider)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));
            if (null == tenants) throw new ArgumentNullException(nameof(tenants));
            if (null == roles) throw new ArgumentNullException(nameof(roles));

            _tenantId = dto.TenantId;
            if (_tenantId?.IsNew ?? true) throw new ArgumentException(
                $"Tenant id cannot be null or new.", nameof(dto));

            var tenant = tenants.FirstOrDefault(t => t.Id.IsSameIdentityAs(_tenantId));
            if (tenant.IsNull()) throw new InvalidOperationException(
                $"Failed to identity a tenant (id = {_tenantId}).");
            _tenantName = tenant.Name;

            if (!(dto.RoleId?.IsNew ?? true))
            {
                _role = roles.FirstOrDefault(r => r.Id.IsSameIdentityAs(dto.RoleId));
                if (_role?.Id?.IsNew ?? true) throw new InvalidOperationException(
                    $"Failed to identity a role (id = {dto.RoleId}).");
            }
            else if (dto.IsCustomRole)
            {
                _role = UserRoleLookup.CustomRole;
            }
        }

        /// <inheritdoc />
        protected override IDomainEntityIdentity CreateNewIdentity()
        {
            return new GuidDomainEntityIdentity(typeof(UserTenant));
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="IUserTenant.TenantId"/>
        public IDomainEntityIdentity TenantId 
            => _tenantId;

        /// <inheritdoc cref="IUserTenant.TenantName"/>
        public string TenantName 
            => _tenantName;

        /// <inheritdoc cref="IUserTenant.RoleId"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 2,
            Description = nameof(Resources.Security_IUserTenant_Role_Description),
            Name = nameof(Resources.Security_IUserTenant_Role_Name),
            ShortName = nameof(Resources.Security_IUserTenant_Role_ShortName),
            Prompt = nameof(Resources.Security_IUserTenant_Role_Prompt))]
        public UserRoleLookup Role
        {
            get => _role;
            set => SetLookupPropertyValue(nameof(Role), ref _role, value);
        }

        /// <inheritdoc cref="IUserTenant.RoleId"/>
        [IgnorePropertyMetadata]
        IDomainEntityIdentity IUserTenant.RoleId 
            => _role?.Id;

        #endregion

        #region Methods

        /// <summary>
        /// Exports entity data to DTO (variation of memento pattern)
        /// </summary>
        /// <returns>a DTO that contains the data of the UserTenant instance to persist</returns>
        public UserTenantDto ToDto()
        {
            return new UserTenantDto(this);
        }

        /// <summary>
        /// Merges business data coming from an untrusted source into the entity;
        /// subject to <paramref name="validate"/> param throws an exception if the entity becomes invalid
        /// </summary>
        /// <param name="entity">business data coming from an untrusted source</param>
        /// <param name="validate">whether to throw an exception if the entity becomes invalid</param>
        /// <exception cref="ArgumentNullException">if the <paramref name="entity"/> is null
        /// or <paramref name="roles"/> is null/></exception>
        /// <exception cref="InvalidOperationException">on tenant id mismatch</exception>
        internal void Merge(IUserTenant entity, IEnumerable<UserRoleLookup> roles)
        {
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));
            if (roles.IsNull()) throw new ArgumentNullException(nameof(roles));
            if (!_tenantId.IsSameIdentityAs(entity.TenantId)) throw new InvalidOperationException(
                "Invalid merge: tenant id mismatch.");

            using (SuspendBindings())
            {
                using (SuspendValidation())
                {
                    UserRoleLookup currentRole = null;
                    if (!(entity.RoleId?.IsNew ?? true) && !entity.RoleId.IsSameIdentityAs(UserRoleLookup.CustomRole.Id))
                    {
                        currentRole = roles.FirstOrDefault(
                            r => r.Id.IsSameIdentityAs(entity.RoleId));
                        if (null == currentRole) throw new InvalidOperationException(
                            $"Failed to identify user role (id = {entity.RoleId}).");
                    }
                    else if (!(entity.RoleId?.IsNew ?? true) && entity.RoleId.IsSameIdentityAs(UserRoleLookup.CustomRole.Id))
                    {
                        currentRole = UserRoleLookup.CustomRole;
                    }

                    if (!(_role?.Id?.IsNew ?? true) && _role.IsCustomRole && 
                        ((currentRole?.Id?.IsNew ?? true) || !currentRole.IsCustomRole )) 
                        _hadCustomRole = true;

                    _role = currentRole;
                }
            }
        }

        #endregion

        #region DTO

        /// <summary>
        /// DTO for <see cref="UserTenant"/> persistence (memento pattern variation).
        /// </summary>
        [Serializable]
        public class UserTenantDto : DomainEntityDto
        {
            /// <inheritdoc />
            public UserTenantDto() : base() { }

            /// <inheritdoc />
            public UserTenantDto(UserTenant entity) : base(entity)
            {
                TenantId = entity.TenantId;
                RoleId = ((entity.Role?.Id.IsNew ?? true) || entity.Role.IsCustomRole) ? 
                    null : entity.Role.Id;
                IsCustomRole = (!(entity.Role?.Id.IsNew ?? true) && entity.Role.IsCustomRole);
                HadCustomRole = entity._hadCustomRole;
            }


            /// <inheritdoc cref="IUserTenant.TenantId"/>
            public IDomainEntityIdentity TenantId { get; set; }
                          
            /// <inheritdoc cref="IUserTenant.RoleId"/>
            public IDomainEntityIdentity RoleId { get; set; }

            /// <summary>
            /// whether the user has been assigned a custom role
            /// </summary>
            public bool IsCustomRole { get; set; }

            /// <summary>
            /// whether the user had a custom role but it was set to null by the user
            /// </summary>
            public bool HadCustomRole { get; set; }

        }

        #endregion
    }
}
