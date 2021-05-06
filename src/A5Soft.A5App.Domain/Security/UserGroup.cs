using System;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.A5App.Domain.Core;

namespace A5Soft.A5App.Domain.Security
{
    /// <inheritdoc cref="IUserGroup"/>
    [Serializable]
    public sealed class UserGroup : AuditableDomainEntity<UserGroup>, IUserGroup
    {
        #region Const

        internal const int DefaultMaxUsers = 10;
        internal const int DefaultMaxTenants = 10;
        internal const int AbsoluteMaxUsers = 10;
        internal const int AbsoluteMaxTenants = 10;

        #endregion

        #region Private Fields

        private string _groupName = string.Empty;
        private int _maxUsers = DefaultMaxUsers;
        private int _maxTenants = DefaultMaxTenants;

        #endregion

        #region Constructors

        /// <inheritdoc />
        public UserGroup(IValidationEngineProvider validationEngineProvider) : base(validationEngineProvider) { }

        /// <summary>
        /// Creates a new instance for an existing entity.
        /// </summary>
        /// <param name="dto">DTO containing the entity data</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public UserGroup(UserGroupDto dto, IValidationEngineProvider validationEngineProvider)
            : base(dto, validationEngineProvider)
        {
            _maxUsers = dto.MaxUsers;
            _groupName = dto.GroupName ?? string.Empty;
            _maxTenants = dto.MaxTenants;
            UserCount = dto.UserCount;
        }

        /// <inheritdoc />
        protected override IDomainEntityIdentity CreateNewIdentity()
        {
            return new GuidDomainEntityIdentity(typeof(UserGroup));
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="IUserGroup.GroupName"/>
        public string GroupName
        {
            get => _groupName;
            set => SetPropertyValue(nameof(GroupName), ref _groupName, value);
        }

        /// <inheritdoc cref="IUserGroup.MaxUsers"/>
        public int MaxUsers
        {
            get => _maxUsers;
            set => SetPropertyValue(nameof(MaxUsers), ref _maxUsers, value);
        }

        /// <inheritdoc cref="IUserGroup.MaxTenants"/>
        public int MaxTenants
        {
            get => _maxTenants;
            set => SetPropertyValue(nameof(MaxTenants), ref _maxTenants, value);
        }

        /// <inheritdoc cref="IUserGroup.UserCount"/>
        public int UserCount { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Exports entity data to DTO (variation of memento pattern)
        /// </summary>
        /// <returns>a DTO that contains the data of the UserGroup instance to persist</returns>
        public UserGroupDto ToDto()
        {
            return new UserGroupDto(this);
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
        public void Merge(IUserGroup entity, bool validate = true)
        {
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));

            using (SuspendBindings())
            {
                using (SuspendValidation())
                {
                    MaxUsers = entity.MaxUsers;
                    MaxTenants = entity.MaxTenants;
                    GroupName = entity.GroupName ?? string.Empty;
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
        /// DTO for <see cref="UserGroup"/> persistence (memento pattern variation).
        /// </summary>
        [Serializable]
        public class UserGroupDto : AuditableDomainEntityDto
        {
            /// <inheritdoc />
            public UserGroupDto() : base() { }

            /// <inheritdoc />
            public UserGroupDto(UserGroup entity) : base(entity)
            {
                GroupName = entity.GroupName;
                MaxTenants = entity.MaxTenants;
                MaxUsers = entity.MaxUsers;
                UserCount = entity.UserCount;
            }


            /// <inheritdoc cref="IUserGroup.GroupName"/>
            public string GroupName { get; set; }

            /// <inheritdoc cref="IUserGroup.MaxUsers"/>
            public int MaxUsers { get; set; }

            /// <inheritdoc cref="IUserGroup.MaxTenants"/>
            public int MaxTenants { get; set; }

            /// <inheritdoc cref="IUserGroup.UserCount"/>
            public int UserCount { get; set; }

        }

        #endregion
    }
}
