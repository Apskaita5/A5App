using System;

namespace A5Soft.A5App.Domain.Core
{
    /// <summary>
    /// a base class for auditable domain singleton dto's in order not to replicate audit trace props
    /// </summary>
    public abstract class AuditableDto
    {

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
