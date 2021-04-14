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
    /// <inheritdoc cref="ITenantGroupAssignment"/>
    [Serializable]
    public sealed class TenantGroupAssignment : DomainEntity<TenantGroupAssignment>, ITenantGroupAssignment
    {
        #region Private Fields

        private readonly UserGroupLookup _group;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance for a new entity (assignment).
        /// </summary>
        /// <param name="userGroup">a user group to assign to the tenant</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public TenantGroupAssignment(UserGroupLookup userGroup, IValidationEngineProvider validationEngineProvider)
            : base(validationEngineProvider)
        {
            _group = userGroup ?? throw new ArgumentNullException(nameof(userGroup));
        }

        /// <summary>
        /// Creates a new instance for a new entity (assignment).
        /// </summary>
        /// <param name="userGroupId">an id of the user group to assign to the tenant</param>
        /// <param name="userGroups">user group lookup list</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public TenantGroupAssignment(IDomainEntityIdentity userGroupId, IEnumerable<UserGroupLookup> userGroups, 
            IValidationEngineProvider validationEngineProvider) : base(validationEngineProvider)
        {
            if (userGroupId?.IsNew ?? true) throw new ArgumentNullException(nameof(userGroupId));

            _group = userGroups.FirstOrDefault(g => g.Id.IsSameIdentityAs(userGroupId)) 
                ?? throw new ArgumentException($"No such user group (id = {userGroupId}).");
        }


        /// <summary>
        /// Creates a new instance for an existing entity.
        /// </summary>
        /// <param name="dto">DTO containing the entity data</param>
        /// <param name="userGroups">user group lookup list</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public TenantGroupAssignment(TenantGroupAssignmentDto dto, IEnumerable<UserGroupLookup> userGroups, 
            IValidationEngineProvider validationEngineProvider)
            : base(dto?.Id, validationEngineProvider)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));
            if (null == userGroups) throw new ArgumentNullException(nameof(userGroups));
            if (dto.GroupId?.IsNew ?? true) throw new ArgumentException("Group id is not specified.", nameof(dto));

            _group = userGroups.FirstOrDefault(g => g.Id.IsSameIdentityAs(dto.GroupId));

            if (null == _group) throw new InvalidOperationException($"No such user group (id = {dto.GroupId}).");
        }

        /// <inheritdoc />
        protected override IDomainEntityIdentity CreateNewIdentity()
        {
            return new GuidDomainEntityIdentity(typeof(TenantGroupAssignment));
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="ITenantGroupAssignment.GroupId"/>
        [Display(AutoGenerateField = true, ResourceType = typeof(Resources), Order = 0,
            Description = nameof(Resources.Security_ITenantGroupAssignment_Group_Description),
            Name = nameof(Resources.Security_ITenantGroupAssignment_Group_Name),
            ShortName = nameof(Resources.Security_ITenantGroupAssignment_Group_ShortName))]
        public UserGroupLookup Group => _group;

        /// <inheritdoc cref="ITenantGroupAssignment.GroupId"/>
        [IgnorePropertyMetadata]
        IDomainEntityIdentity ITenantGroupAssignment.GroupId 
            => _group.Id;

        #endregion

        #region Methods

        /// <summary>
        /// Exports entity data to DTO (variation of memento pattern)
        /// </summary>
        /// <returns>a DTO that contains the data of the TenantGroupAssignment instance to persist</returns>
        public TenantGroupAssignmentDto ToDto()
        {
            return new TenantGroupAssignmentDto(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return _group.GroupName;
        }

        #endregion

        #region DTO

        /// <summary>
        /// DTO for <see cref="TenantGroupAssignment"/> persistence (memento pattern variation).
        /// </summary>
        [Serializable]
        public class TenantGroupAssignmentDto : DomainEntityDto
        {
            /// <inheritdoc />
            public TenantGroupAssignmentDto() : base() { }

            /// <inheritdoc />
            public TenantGroupAssignmentDto(TenantGroupAssignment entity) : base(entity)
            {
                GroupId = entity.Group.Id;
            }


            /// <inheritdoc cref="ITenantGroupAssignment.GroupId"/>
            public IDomainEntityIdentity GroupId { get; set; }

        }

        #endregion
    }
}
