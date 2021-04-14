using System;

namespace A5Soft.A5App.Domain.Core
{
    /// <summary>
    /// auditable domain entity dto in order not to replicate audit trace and id props
    /// </summary>
    public abstract class AuditableDomainEntityDto : DomainEntityDto
    {
        /// <inheritdoc />
        protected AuditableDomainEntityDto() : base() { }

        /// <inheritdoc />
        protected AuditableDomainEntityDto(IAuditableEntity entity) : base(entity)
        {
            InsertedAt = entity.InsertedAt ?? DateTime.MaxValue;
            InsertedBy = entity.InsertedBy;
            UpdatedAt = entity.UpdatedAt ?? DateTime.MaxValue;
            UpdatedBy = entity.UpdatedBy;
        }


        /// <inheritdoc cref="IAuditable.InsertedAt"/> 
        public DateTime InsertedAt { get; set; }

        /// <inheritdoc cref="IAuditable.InsertedBy"/>   
        public string InsertedBy { get; set; }

        /// <inheritdoc cref="IAuditable.UpdatedAt"/>  
        public DateTime UpdatedAt { get; set; }

        /// <inheritdoc cref="IAuditable.UpdatedBy"/>  
        public string UpdatedBy { get; set; }

    }
}
