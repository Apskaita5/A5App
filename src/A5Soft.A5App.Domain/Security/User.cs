using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using A5Soft.A5App.Domain.Core;
using A5Soft.A5App.Domain.Properties;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata.DataAnnotations;
using A5Soft.CARMA.Domain.Rules;
using static A5Soft.A5App.Domain.Security.UserTenant;

namespace A5Soft.A5App.Domain.Security
{
    /// <inheritdoc cref="IUser"/>
    [Serializable]
    public sealed class User : AuditableDomainEntity<User>, IUser
    {
        #region Private Fields

        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private AdministrativeRole _adminRole = AdministrativeRole.None;
        private UserGroupLookup _userGroup;
        private bool _isSuspended = false;
        private bool _isDisabled = false;
        private bool _twoFactorEnabled = false;
        private readonly DomainBindingList<UserTenant> _rolesForTenants;
        [NonSerialized]
        private bool _promotedToAdminRole;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance for a new entity.
        /// </summary>
        /// <param name="tenants">a lookup for the tenant data</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public User(IEnumerable<TenantLookup> tenants, IValidationEngineProvider validationEngineProvider)
            : base(validationEngineProvider)
        {
            _rolesForTenants = new DomainBindingList<UserTenant>(validationEngineProvider)
            {
                AllowNew = false,
                AllowRemove = false
            };
            _rolesForTenants.AddRange(tenants.Select(
                d => new UserTenant(d, validationEngineProvider)));
            RegisterChildValue(_rolesForTenants, nameof(_rolesForTenants), true);
        }

        /// <summary>
        /// Creates a new instance for an existing entity.
        /// </summary>
        /// <param name="dto">DTO containing the entity data</param>
        /// <param name="tenants">a lookup for the tenant data</param>
        /// <param name="roles">a lookup for the roles data</param>
        /// <param name="userGroups">a lookup for the user group data</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public User(UserDto dto, IEnumerable<TenantLookup> tenants, IEnumerable<UserRoleLookup> roles,
            IEnumerable<UserGroupLookup> userGroups, IValidationEngineProvider validationEngineProvider)
            : base(dto, validationEngineProvider)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            _name = dto.Name ?? string.Empty;
            _email = dto.Email ?? string.Empty;
            _phone = dto.Phone ?? string.Empty;
            _adminRole = dto.AdminRole;
            _isSuspended = dto.IsSuspended;
            _isDisabled = dto.IsDisabled;
            _twoFactorEnabled = dto.TwoFactorEnabled;
            
            if (!(dto.UserGroupId?.IsNew ?? true))
            {
                _userGroup = userGroups.FirstOrDefault(
                    g => g.Id.IsSameIdentityAs(dto.UserGroupId));
                if (null == _userGroup) throw new InvalidOperationException(
                    $"Failed to identify user group (id = {dto.UserGroupId}).");
            }

            _rolesForTenants = new DomainBindingList<UserTenant>(validationEngineProvider)
            {
                AllowNew = false,
                AllowRemove = false
            };
            _rolesForTenants.AddRange(dto.RolesForTenants.Select(
                d => new UserTenant(d, tenants, roles, validationEngineProvider)));
            RegisterChildValue(_rolesForTenants, nameof(_rolesForTenants), true);
        }

        /// <inheritdoc />
        protected override IDomainEntityIdentity CreateNewIdentity()
        {
            return new GuidDomainEntityIdentity(typeof(User));
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="IUser.Name"/>
        public string Name
        {
            get => _name;
            set => SetPropertyValue(nameof(Name), ref _name, value);
        }

        /// <inheritdoc cref="IUser.Email"/>
        public string Email
        {
            get => _email;
            set => SetPropertyValue(nameof(Email), ref _email, value);
        }

        /// <inheritdoc cref="IUser.Phone"/>
        public string Phone
        {
            get => _phone;
            set => SetPropertyValue(nameof(Phone), ref _phone, value);
        }

        /// <inheritdoc cref="IUser.AdminRole"/>
        public AdministrativeRole AdminRole
        {
            get => _adminRole;
            set => SetPropertyValue(nameof(AdminRole), ref _adminRole, value);
        }

        /// <inheritdoc cref="IUser.UserGroupId"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 4,
            Description = nameof(Resources.Security_IUser_UserGroup_Description),
            Name = nameof(Resources.Security_IUser_UserGroup_Name),
            ShortName = nameof(Resources.Security_IUser_UserGroup_ShortName),
            Prompt = nameof(Resources.Security_IUser_UserGroup_Prompt))]
        public UserGroupLookup UserGroup
        {
            get => _userGroup;
            set => SetLookupPropertyValue(nameof(UserGroup), ref _userGroup, value);

        }

        /// <inheritdoc cref="IUser.UserGroupId"/> 
        [IgnorePropertyMetadata]
        IDomainEntityIdentity IUser.UserGroupId
            => _userGroup?.Id;

        /// <inheritdoc cref="IUser.IsSuspended"/>
        public bool IsSuspended
        {
            get => _isSuspended;
            set => SetPropertyValue(nameof(IsSuspended), ref _isSuspended, value);
        }

        /// <inheritdoc cref="IUser.IsDisabled"/>
        public bool IsDisabled
        {
            get => _isDisabled;
            set => SetPropertyValue(nameof(IsDisabled), ref _isDisabled, value);
        }

        /// <inheritdoc cref="IUser.TwoFactorEnabled"/>
        public bool TwoFactorEnabled
        {
            get => _twoFactorEnabled;
            set => SetPropertyValue(nameof(TwoFactorEnabled), ref _twoFactorEnabled, value);
        }

        /// <inheritdoc cref="IUser.RolesForTenants"/>
        public DomainBindingList<UserTenant> RolesForTenants 
            => _rolesForTenants;

        /// <inheritdoc cref="IUser.RolesForTenants"/>
        IList<IUserTenant> IUser.RolesForTenants 
            => _rolesForTenants.Select(r => (IUserTenant) r).ToList();
                                
        #endregion

        #region Methods

        /// <summary>
        /// Exports entity data to DTO (variation of memento pattern)
        /// </summary>
        /// <returns>a DTO that contains the data of the User instance to persist</returns>
        public UserDto ToDto()
        {
            return new UserDto(this);
        }

        /// <summary>
        /// Merges business data coming from an untrusted source into the entity;
        /// subject to <paramref name="validate"/> param throws an exception if the entity becomes invalid
        /// </summary>
        /// <param name="entity">business data coming from an untrusted source</param>
        /// <param name="roles">a lookup for the roles data</param>
        /// <param name="userGroups">a lookup for the user group data</param>
        /// <param name="validate">whether to throw an exception if the entity becomes invalid</param>
        /// <exception cref="ConcurrencyException">if there is a concurrency conflict</exception>
        /// <exception cref="ValidationException">if the entity becomes invalid after merge</exception>
        /// <exception cref="ArgumentNullException">if the <paramref name="entity"/> is null</exception>
        public void Merge(IUser entity, IEnumerable<UserGroupLookup> userGroups,
            IEnumerable<UserRoleLookup> roles, bool validate = true)
        {
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));
            if (null == userGroups) throw new ArgumentNullException(nameof(userGroups));
            if (null == roles) throw new ArgumentNullException(nameof(roles));

            using (SuspendBindings())
            {
                using (SuspendValidation())
                {
                    Name = entity.Name;
                    Email = entity.Email;
                    Phone = entity.Phone;
                    _promotedToAdminRole = (AdminRole == AdministrativeRole.None 
                        && entity.AdminRole != AdministrativeRole.None);
                    AdminRole = entity.AdminRole;
                    IsSuspended = entity.IsSuspended;
                    IsDisabled = entity.IsDisabled;
                    TwoFactorEnabled = entity.TwoFactorEnabled;

                    if (!(entity.UserGroupId?.IsNew ?? true))
                    {
                        _userGroup = userGroups.FirstOrDefault(
                            g => g.Id.IsSameIdentityAs(entity.UserGroupId));
                        if (null == _userGroup) throw new InvalidOperationException(
                            $"Unknown user group (id = {entity.UserGroupId}).");
                        if (AdminRole == AdministrativeRole.Admin) throw new ValidationException(
                            Resources.Security_User_Cannot_Assign_Admin_UserGroup);
                    }

                    if (AdminRole == AdministrativeRole.None)
                    {
                        foreach (var roleForTenant in _rolesForTenants)
                        {
                            var current =
                                entity.RolesForTenants.FirstOrDefault(r =>
                                    r.TenantId?.IsSameIdentityAs(roleForTenant.TenantId) ?? false);
                            if (null != current) roleForTenant.Merge(current, roles);
                        }
                    }
                    else
                    {
                        foreach (var roleForTenant in _rolesForTenants)
                        {
                            roleForTenant.Role = null;
                        }
                    }
                }
            }

            if (validate)
            {
                this.CheckRules();
                if (!this.IsValid) throw new ValidationException(this.GetBrokenRulesTree());
                CheckConcurrency(entity);
            }
        }

        #endregion

        #region DTO

        /// <summary>
        /// DTO for <see cref="User"/> persistence (memento pattern variation).
        /// </summary>
        [Serializable]
        public class UserDto : AuditableDomainEntityDto
        {
            /// <inheritdoc />
            public UserDto() : base() { }

            /// <inheritdoc />
            public UserDto(User entity) : base(entity)
            {
                Name = entity.Name;
                Email = entity.Email;
                Phone = entity.Phone;
                AdminRole = entity.AdminRole;
                IsSuspended = entity.IsSuspended;
                IsDisabled = entity.IsDisabled;
                TwoFactorEnabled = entity.TwoFactorEnabled;
                UserGroupId = entity.UserGroup?.Id;
                RolesForTenants = entity.RolesForTenants.Select(r => r.ToDto()).ToList();
                PromotedToAdminRole = entity._promotedToAdminRole;
            }


            /// <inheritdoc cref="IUser.Name"/>
            public string Name { get; set; }

            /// <inheritdoc cref="IUser.Email"/>
            public string Email { get; set; }

            /// <inheritdoc cref="IUser.Phone"/>
            public string Phone { get; set; }

            /// <inheritdoc cref="IUser.AdminRole"/>
            public AdministrativeRole AdminRole { get; set; }

            /// <inheritdoc cref="IUser.UserGroupId"/>
            public IDomainEntityIdentity UserGroupId { get; set; }

            /// <inheritdoc cref="IUser.IsSuspended"/>
            public bool IsSuspended { get; set; }

            /// <inheritdoc cref="IUser.IsDisabled"/>
            public bool IsDisabled { get; set; }

            /// <inheritdoc cref="IUser.TwoFactorEnabled"/>
            public bool TwoFactorEnabled { get; set; }

            /// <inheritdoc cref="IUser.RolesForTenants"/>
            public List<UserTenantDto> RolesForTenants { get; set; }

            /// <summary>
            /// Only for a new user, to insert a temporary password.
            /// </summary>
            public string HashedPassword { get; set; }

            public bool PromotedToAdminRole { get; set; }

        }

        #endregion
    }
}
