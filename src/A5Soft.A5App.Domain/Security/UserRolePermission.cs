using System;
using A5Soft.A5App.Domain.Core;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Domain.Security
{
    /// <inheritdoc cref="IUserRolePermission"/>
    [Serializable]
    public sealed class UserRolePermission : DomainEntity<UserRolePermission>, IUserRolePermission
    {
        #region Private Fields

        private Guid _permissionId;
        private readonly string _name;
        private readonly string _description;
        private readonly string _groupName;
        private readonly string _moduleName;
        private readonly int _order;
        private bool _assigned;

        #endregion

        #region Constructors

        /// <inheritdoc />
        public UserRolePermission(IValidationEngineProvider validationEngineProvider) : base(validationEngineProvider)
        { }

        /// <summary>
        /// Creates a new instance for an existing entity.
        /// </summary>
        /// <param name="dto">DTO containing the entity data</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public UserRolePermission(UserRolePermissionDto dto, IValidationEngineProvider validationEngineProvider)
            : base(dto?.Id, validationEngineProvider, true)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            _permissionId = dto.PermissionId;
            _name = dto.Name;
            _description = dto.Description;
            _groupName = dto.GroupName;
            _moduleName = dto.ModuleName;
            _order = dto.Order;
            _assigned = dto.Assigned;
        }

        /// <inheritdoc />
        protected override IDomainEntityIdentity CreateNewIdentity()
        {
            return new GuidDomainEntityIdentity(typeof(UserRolePermission));
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="IUserRolePermission.PermissionId"/>
        public Guid PermissionId => _permissionId;

        /// <inheritdoc cref="IUserRolePermission.Name"/>
        public string Name => _name;

        /// <inheritdoc cref="IUserRolePermission.Description"/>
        public string Description => _description;

        /// <inheritdoc cref="IUserRolePermission.GroupName"/>
        public string GroupName => _groupName;

        /// <inheritdoc cref="IUserRolePermission.ModuleName"/>
        public string ModuleName => _moduleName;

        /// <inheritdoc cref="IUserRolePermission.Order"/>
        public int Order => _order;

        /// <inheritdoc cref="IUserRolePermission.Assigned"/>
        public bool Assigned
        {
            get => _assigned;
            set => SetPropertyValue(nameof(Assigned), ref _assigned, value);
        }
         
        #endregion

        #region Methods

        /// <summary>
        /// Exports entity data to DTO (variation of memento pattern)
        /// </summary>
        /// <returns>a DTO that contains the data of the UserRolePermission instance to persist</returns>
        public UserRolePermissionDto ToDto()
        {
            return new UserRolePermissionDto(this);
        }

        /// <summary>
        /// Merges business data coming from an untrusted source into the entity;
        /// subject to <paramref name="validate"/> param throws an exception if the entity becomes invalid
        /// </summary>
        /// <param name="entity">business data coming from an untrusted source</param>
        /// <param name="validate">whether to throw an exception if the entity becomes invalid</param>
        /// <exception cref="ValidationException">if the entity becomes invalid after merge</exception>
        /// <exception cref="ArgumentNullException">if the <paramref name="entity"/> is null</exception>
        public void Merge(IUserRolePermission entity, bool validate = true)
        {
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));

            using (SuspendBindings())
            {
                using (SuspendValidation())
                {
                    Assigned = entity.Assigned;
                }
            }

            if (validate)
            {
                this.CheckRules();
                if (!this.IsValid) throw new ValidationException(this.GetBrokenRulesTree());
            }
        }

        #endregion

        #region DTO

        /// <summary>
        /// DTO for <see cref="UserRolePermission"/> persistence (memento pattern variation).
        /// </summary> 
        [Serializable]
        public class UserRolePermissionDto : DomainEntityDto
        {
            /// <inheritdoc />
            public UserRolePermissionDto() : base() { }

            /// <inheritdoc />
            public UserRolePermissionDto(UserRolePermission entity) : base(entity)
            {
                PermissionId = entity.PermissionId;
                Name = entity.Name;
                Description = entity.Description;
                GroupName = entity.GroupName;
                ModuleName = entity.ModuleName;
                Order = entity.Order;
                Assigned = entity.Assigned;
            }

            /// <inheritdoc />
            public UserRolePermissionDto(Permission permission) : base()
            {
                PermissionId = permission.Id;
                Name = permission.Name;
                Description = permission.Description;
                GroupName = permission.GroupName;
                ModuleName = permission.ModuleName;
                Order = permission.Order;
            }


            /// <inheritdoc cref="IUserRolePermission.PermissionId"/>
            public Guid PermissionId { get; set; }

            /// <inheritdoc cref="IUserRolePermission.Name"/>
            public string Name { get; set; }

            /// <inheritdoc cref="IUserRolePermission.Description"/>
            public string Description { get; set; }

            /// <inheritdoc cref="IUserRolePermission.GroupName"/>
            public string GroupName { get; set; }

            /// <inheritdoc cref="IUserRolePermission.ModuleName"/>
            public string ModuleName { get; set; }

            /// <inheritdoc cref="IUserRolePermission.Order"/>
            public int Order { get; set; }

            /// <inheritdoc cref="IUserRolePermission.Assigned"/>
            public bool Assigned { get; set; }

        }

        #endregion
    }
}
