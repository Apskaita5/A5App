using System;
using System.Collections.Generic;
using System.Linq;
using A5Soft.A5App.Domain.Core;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Domain.Security
{
    /// <inheritdoc cref="IUserRole"/>
    [Serializable]
    public sealed class UserRole : AuditableDomainEntity<UserRole>, IUserRole
    {
        #region Private Fields

        private string _name = string.Empty;
        private string _description = string.Empty;
        private readonly int _userCount = 0;
        private readonly DomainBindingList<UserRolePermission> _permissions;

        #endregion

        #region Constructors

        /// <inheritdoc />
        public UserRole(IValidationEngineProvider validationEngineProvider) : base(validationEngineProvider)
        {
            _permissions = new DomainBindingList<UserRolePermission>(validationEngineProvider)
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
        public UserRole(UserRoleDto dto, IValidationEngineProvider validationEngineProvider)
            : base(dto, validationEngineProvider)
        {
            _name = dto.Name ?? string.Empty;
            _description = dto.Description ?? string.Empty;
            _userCount = dto.UserCount;
            _permissions = new DomainBindingList<UserRolePermission>(validationEngineProvider)
            {
                AllowNew = false,
                AllowRemove = false
            };
            _permissions.AddRange(dto.Permissions.Select(
                d => new UserRolePermission(d, validationEngineProvider)));
            RegisterChildValue(_permissions, nameof(_permissions), true);
        }

        /// <inheritdoc />
        protected override IDomainEntityIdentity CreateNewIdentity()
        {
            return new GuidDomainEntityIdentity(typeof(UserRole));
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="IUserRole.Name"/>
        public string Name
        {
            get => _name;
            set => SetPropertyValue(nameof(Name), ref _name, value);
        }

        /// <inheritdoc cref="IUserRole.Description"/>
        public string Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
        }

        /// <inheritdoc cref="IUserRole.UserCount"/>
        public int UserCount => _userCount;

        /// <inheritdoc cref="IUserRole.Permissions"/>
        public DomainBindingList<UserRolePermission> Permissions => _permissions;

        /// <inheritdoc cref="IUserRole.Permissions"/>
        IList<IUserRolePermission> IUserRole.Permissions 
            => _permissions.Select(p => (IUserRolePermission)p).ToList();
            
        #endregion

        #region Methods

        /// <summary>
        /// Exports entity data to DTO (variation of memento pattern)
        /// </summary>
        /// <returns>a DTO that contains the data of the UserRole instance to persist</returns>
        public UserRoleDto ToDto()
        {
            return new UserRoleDto(this);
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
        public void Merge(IUserRole entity, bool validate = true)
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
                    foreach (var permission in _permissions)
                    {
                        permission.Assigned = selectedPermissions
                            .Contains((Guid)permission.Id.IdentityValue);
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
        /// DTO for <see cref="UserRole"/> persistence (memento pattern variation).
        /// </summary>  
        [Serializable]
        public class UserRoleDto : AuditableDomainEntityDto
        {
            /// <inheritdoc />
            public UserRoleDto() : base() { }

            /// <inheritdoc />
            public UserRoleDto(UserRole entity) : base(entity)
            {
                Name = entity.Name;
                Description = entity.Description;
                UserCount = entity.UserCount;
                Permissions = entity.Permissions.Select(i => i.ToDto()).ToList();
            }


            /// <inheritdoc cref="IUserRole.Name"/>
            public string Name { get; set; }
              
            /// <inheritdoc cref="IUserRole.Description"/>
            public string Description { get; set; }

            /// <inheritdoc cref="IUserRole.UserCount"/>
            public int UserCount { get; set; }

            /// <inheritdoc cref="IUserRole.Permissions"/>
            public List<UserRolePermission.UserRolePermissionDto> Permissions { get; set; }

        }

        #endregion
    }
}
