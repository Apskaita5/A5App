using System;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Domain.Core
{
    /// <summary>
    /// a domain entity DTO, in order not to replicate properties
    /// </summary>
    public abstract class DomainEntityDto
    {
        /// <summary>
        /// creates a new empty DTO instance
        /// </summary>
        protected DomainEntityDto() { }

        /// <summary>
        /// creates a new DTO instance and sets its data for the entity
        /// </summary>
        /// <param name="entity">an entity to use for the DTO data</param>
        protected DomainEntityDto(IDomainEntity entity)
        {
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));
            if (entity.Id.IsNull()) throw new ArgumentException("An entity cannot have a null identity.", nameof(entity));
            Id = entity.Id;
        }

        /// <inheritdoc cref="DomainEntity{T}.Id"/>
        public IDomainEntityIdentity Id { get; set; }
    }
}
