using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata.DataAnnotations;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Domain.Core
{
    /// <summary>
    /// A base class for all domain entities that have an identity (i.e. more than one entity
    /// of the type exists in the domain and they are distinguished by identity value)
    /// and auditing functionality (IAuditableEntity) implemented.
    /// </summary>
    /// <typeparam name="T">a type of the domain entity implementation</typeparam> 
    [Serializable]
    public abstract class AuditableDomainEntity<T> : DomainEntity<T>, IAuditableEntity
        where T : AuditableDomainEntity<T>
    {
        #region Private Fields

        private DateTime? _insertedAt = null;
        private string _insertedBy = string.Empty;
        private DateTime? _updatedAt = null;
        private string _updatedBy = string.Empty;
        private readonly string _occHash = string.Empty;

        #endregion
         
        #region Constructors

        /// <summary>
        /// Creates a new instance for a new entity.
        /// </summary>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        protected AuditableDomainEntity(IValidationEngineProvider validationEngineProvider) 
            : base(validationEngineProvider) { }

        /// <summary>
        /// Creates a new instance for an existing entity.
        /// </summary>
        /// <param name="dto">DTO containing the entity data</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        protected AuditableDomainEntity(AuditableDomainEntityDto dto, IValidationEngineProvider validationEngineProvider) 
            : base(dto?.Id, validationEngineProvider)
        {
            if (dto.IsNull()) throw new ArgumentNullException(nameof(dto));

            _insertedBy = dto.InsertedBy ?? string.Empty;
            _insertedAt = dto.InsertedAt;
            _updatedBy = dto.UpdatedBy ?? string.Empty;
            _updatedAt = dto.UpdatedAt;
            _occHash = GetOccHash(dto.UpdatedAt);
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="IAuditable.InsertedAt"/> 
        [Browsable(true)]
        [ScaffoldColumn(false)]
        public DateTime? InsertedAt
            => _insertedAt;

        /// <inheritdoc cref="IAuditable.InsertedBy"/>   
        [Browsable(true)]
        [ScaffoldColumn(false)]
        public string InsertedBy
            => _insertedBy;

        /// <inheritdoc cref="IAuditable.UpdatedAt"/>  
        [Browsable(true)]
        [ScaffoldColumn(false)]
        public DateTime? UpdatedAt
            => _updatedAt;

        /// <inheritdoc cref="IAuditable.UpdatedBy"/>  
        [Browsable(true)]
        [ScaffoldColumn(false)]
        public string UpdatedBy
            => _updatedBy;

        /// <inheritdoc cref="IAuditable.OccHash"/> 
        [Browsable(false)]
        [ScaffoldColumn(false)]
        [IgnorePropertyMetadata]
        public string OccHash
            => _occHash;

        #endregion

        /// <inheritdoc cref="IAuditable.HideAuditTrace"/>  
        public void HideAuditTrace()
        {
            _updatedAt = _insertedAt = null;
            _updatedBy = _insertedBy = string.Empty;
        }

        protected void CheckConcurrency(IAuditable mergedEntity)
        {
            if (mergedEntity.OccHash != _occHash) throw new ConcurrencyException(this);
        }

        private string GetOccHash(DateTime updatedAt)
        {
            return updatedAt.CreateOccHash<T>();
        }

    }
}
