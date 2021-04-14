using A5Soft.A5App.Domain.Core;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;
using System;

namespace A5Soft.A5App.Domain.Security
{
    /// <inheritdoc cref="IUserPermission"/>
    [Serializable]
    public sealed class UserPermission : DomainEntity<UserPermission>, IUserPermission
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
        public UserPermission(IValidationEngineProvider validationEngineProvider) : base(validationEngineProvider)
        {}

        /// <summary>
        /// Creates a new instance for an existing entity.
        /// </summary>
        /// <param name="dto">DTO containing the entity data</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public UserPermission(UserPermissionDto dto, IValidationEngineProvider validationEngineProvider)
            : base(dto?.Id, validationEngineProvider, true)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            _permissionId = dto.PermissionId;
            _name = dto.Name ?? string.Empty;
            _description = dto.Description ?? string.Empty;
            _groupName = dto.GroupName ?? string.Empty;
            _moduleName = dto.ModuleName ?? string.Empty;
            _order = dto.Order;
            _assigned = dto.Assigned;
        }

        /// <inheritdoc />
        protected override IDomainEntityIdentity CreateNewIdentity()
        {
            return new GuidDomainEntityIdentity(typeof(UserPermission));
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="IUserPermission.PermissionId"/>
        public Guid PermissionId => _permissionId;

        /// <inheritdoc cref="IUserPermission.Name"/>
        public string Name => _name;

        /// <inheritdoc cref="IUserPermission.Description"/>
        public string Description => _description;

        /// <inheritdoc cref="IUserPermission.GroupName"/>
        public string GroupName => _groupName;

        /// <inheritdoc cref="IUserPermission.ModuleName"/>
        public string ModuleName => _moduleName;

        /// <inheritdoc cref="IUserPermission.Order"/>
        public int Order => _order;

        /// <inheritdoc cref="IUserPermission.Assigned"/>
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
        /// <returns>a DTO that contains the data of the UserPermission instance to persist</returns>
        public UserPermissionDto ToDto()
        {
            return new UserPermissionDto(this);
        }

        #endregion

        #region DTO

        /// <summary>
        /// DTO for <see cref="UserPermission"/> persistence (memento pattern variation).
        /// </summary> 
        [Serializable]
        public class UserPermissionDto : DomainEntityDto
        {
            /// <inheritdoc />
            public UserPermissionDto() : base() { }

            /// <inheritdoc />
            public UserPermissionDto(UserPermission entity) : base(entity)
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
            public UserPermissionDto(Permission permission) : base()
            {
                PermissionId = permission.Id;
                Name = permission.Name;
                Description = permission.Description;
                GroupName = permission.GroupName;
                ModuleName = permission.ModuleName;
                Order = permission.Order;
            }


            /// <inheritdoc cref="IUserPermission.PermissionId"/>
            public Guid PermissionId { get; set; }

            /// <inheritdoc cref="IUserPermission.Name"/>
            public string Name { get; set; }

            /// <inheritdoc cref="IUserPermission.Description"/>
            public string Description { get; set; }

            /// <inheritdoc cref="IUserPermission.GroupName"/>
            public string GroupName { get; set; }

            /// <inheritdoc cref="IUserPermission.ModuleName"/>
            public string ModuleName { get; set; }

            /// <inheritdoc cref="IUserPermission.Order"/>
            public int Order { get; set; }

            /// <inheritdoc cref="IUserPermission.Assigned"/>
            public bool Assigned { get; set; }

        }

        #endregion
    }
}
