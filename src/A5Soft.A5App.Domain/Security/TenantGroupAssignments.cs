using A5Soft.A5App.Domain.Core;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace A5Soft.A5App.Domain.Security
{
    /// <inheritdoc cref="ITenantGroupAssignments"/>
    [Serializable]
    public sealed class TenantGroupAssignments : DomainEntity<TenantGroupAssignments>, ITenantGroupAssignments
    {
        #region Private Fields

        private readonly string _tenantName;
        private readonly DomainBindingList<TenantGroupAssignment> _assignments;

        #endregion

        #region Constructors
                 
        /// <summary>
        /// Creates a new instance for an existing entity.
        /// </summary>
        /// <param name="dto">DTO containing the entity data</param>
        /// <param name="userGroups">user group lookup list</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public TenantGroupAssignments(TenantGroupAssignmentsDto dto, IEnumerable<UserGroupLookup> userGroups, 
            IValidationEngineProvider validationEngineProvider) : base(dto?.Id, validationEngineProvider)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            _tenantName = dto.TenantName;
            _assignments = new DomainBindingList<TenantGroupAssignment>(validationEngineProvider)
            {
                AllowNew = false
            };
            _assignments.AddRange(dto.Assignments.Select(
                d => new TenantGroupAssignment(d, userGroups, validationEngineProvider)));
            RegisterChildValue(_assignments, nameof(_assignments), true);
        }

        /// <inheritdoc />
        protected override IDomainEntityIdentity CreateNewIdentity()
        {
            return new GuidDomainEntityIdentity(typeof(TenantGroupAssignments));
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="ITenantGroupAssignments.TenantName"/>
        public string TenantName => _tenantName;

        /// <inheritdoc cref="ITenantGroupAssignments.Assignments"/>
        public DomainBindingList<TenantGroupAssignment> Assignments => _assignments;

        /// <inheritdoc cref="ITenantGroupAssignments.Assignments"/>
        List<IDomainEntityIdentity> ITenantGroupAssignments.Assignments
            => _assignments.Select(a => a.Group.Id).ToList();
           
        #endregion

        #region Methods

        /// <summary>
        /// Exports entity data to DTO (variation of memento pattern)
        /// </summary>
        /// <returns>a DTO that contains the data of the TenantGroupAssignments instance to persist</returns>
        public TenantGroupAssignmentsDto ToDto()
        {
            return new TenantGroupAssignmentsDto(this);
        }

        /// <summary>
        /// Merges business data coming from an untrusted source into the entity;
        /// subject to <paramref name="validate"/> param throws an exception if the entity becomes invalid
        /// </summary>
        /// <param name="entity">business data coming from an untrusted source</param>
        /// <param name="userGroups">user group lookup list</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        /// <param name="validate">whether to throw an exception if the entity becomes invalid</param>
        /// <exception cref="ValidationException">if the entity becomes invalid after merge</exception>
        /// <exception cref="ArgumentNullException">if the <paramref name="entity"/> is null</exception>
        public void Merge(ITenantGroupAssignments entity, IEnumerable<UserGroupLookup> userGroups,
            IValidationEngineProvider validationEngineProvider, bool validate = true)
        {
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));
            if (validationEngineProvider.IsNull()) throw new ArgumentNullException(nameof(validationEngineProvider));
            if (!Id.IsSameIdentityAs(entity?.Id)) throw new InvalidOperationException(
                "Cannot merge user group assignments for different tenant.");

            var cleanList = entity.Assignments
                .Select(a => (Guid?) a.IdentityValue)
                .Where(g => g.HasValue)
                .Distinct()
                .Select(g => new TenantGroupAssignment(new GuidDomainEntityIdentity(g.Value, typeof(UserGroup)), 
                    userGroups, validationEngineProvider)).ToList();
            var newList = cleanList.Where(i => !_assignments
                .Any(a => a.Group.Id.IsSameIdentityAs(i.Group.Id)))
                .ToList();
            var deletedList = _assignments.Where(a => !cleanList
                .Any(i => a.Group.Id.IsSameIdentityAs(i.Group.Id)))
                .ToList();

            using (SuspendBindings())
            {
                using (SuspendValidation())
                {
                    _assignments.AddRange(newList);
                    foreach (var deletedAssignment in deletedList) _assignments.Remove(deletedAssignment);
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
        /// DTO for <see cref="TenantGroupAssignments"/> persistence (memento pattern variation).
        /// </summary>
        [Serializable]
        public class TenantGroupAssignmentsDto : DomainEntityDto
        {
            /// <inheritdoc />
            public TenantGroupAssignmentsDto() : base() { }

            /// <inheritdoc />
            public TenantGroupAssignmentsDto(TenantGroupAssignments entity) : base(entity)
            {
                TenantName = entity.TenantName;
                Assignments = entity.Assignments.Select(i => i.ToDto()).ToList();
                DeletedAssignments = entity.Assignments.DeletedList
                    .Where(i => !i.IsNew)
                    .Select(i => i.ToDto()).ToList();
            }


            /// <inheritdoc cref="ITenantGroupAssignments.TenantName"/>
            public string TenantName { get; set; }

            /// <inheritdoc cref="ITenantGroupAssignments.Assignments"/>
            public List<TenantGroupAssignment.TenantGroupAssignmentDto> Assignments { get; set; }
            public List<TenantGroupAssignment.TenantGroupAssignmentDto> DeletedAssignments { get; set; }

        }

        #endregion
    }
}
