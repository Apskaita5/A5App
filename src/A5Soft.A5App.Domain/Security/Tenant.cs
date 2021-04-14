using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;
using System;
using A5Soft.A5App.Domain.Core;

namespace A5Soft.A5App.Domain.Security
{
    /// <inheritdoc cref="ITenant"/>
    [Serializable]
    public sealed class Tenant : DomainEntity<Tenant>, ITenant
    {
        #region Private Fields

        private string _name = string.Empty;
        private string _databaseName = string.Empty;
        private readonly DateTime? _insertedAt = null;
        private readonly string _insertedBy = string.Empty;

        #endregion

        #region Constructors

        /// <inheritdoc />
        public Tenant(IValidationEngineProvider validationEngineProvider) : base(validationEngineProvider)
        { }

        /// <summary>
        /// Creates a new instance for an existing entity.
        /// </summary>
        /// <param name="dto">DTO containing the entity data</param>
        /// <param name="validationEngineProvider">validation engine provider for DI</param>
        public Tenant(TenantDto dto, IValidationEngineProvider validationEngineProvider)
            : base(dto?.Id, validationEngineProvider)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            _name = dto.Name ?? string.Empty;
            _databaseName = dto.DatabaseName ?? string.Empty;
            _insertedAt = dto.InsertedAt;
            _insertedBy = dto.InsertedBy;
        }

        /// <inheritdoc />
        protected override IDomainEntityIdentity CreateNewIdentity()
        {
            return new GuidDomainEntityIdentity(typeof(Tenant));
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="ITenant.Name"/>
        public string Name
        {
            get => _name;
            set => SetPropertyValue(nameof(Name), ref _name, value);
        }

        /// <inheritdoc cref="ITenant.DatabaseName"/>
        public string DatabaseName
        {
            get => _databaseName;
            set => SetPropertyValue(nameof(DatabaseName), ref _databaseName, value);
        }

        /// <inheritdoc cref="ITenant.InsertedAt"/> 
        public DateTime? InsertedAt
            => _insertedAt;

        /// <inheritdoc cref="ITenant.InsertedBy"/>   
        public string InsertedBy
            => _insertedBy;

        #endregion

        #region Methods

        /// <summary>
        /// Exports entity data to DTO (variation of memento pattern)
        /// </summary>
        /// <returns>a DTO that contains the data of the Tenant instance to persist</returns>
        public TenantDto ToDto()
        {
            return new TenantDto(this);
        }

        /// <summary>
        /// Merges business data coming from an untrusted source into the entity;
        /// subject to <paramref name="validate"/> param throws an exception if the entity becomes invalid
        /// </summary>
        /// <param name="entity">business data coming from an untrusted source</param>
        /// <param name="validate">whether to throw an exception if the entity becomes invalid</param>
        /// <exception cref="ValidationException">if the entity becomes invalid after merge</exception>
        /// <exception cref="ArgumentNullException">if the <paramref name="entity"/> is null</exception>
        public void Merge(ITenant entity, bool validate = true)
        {
            if (entity.IsNull()) throw new ArgumentNullException(nameof(entity));

            using (SuspendBindings())
            {
                using (SuspendValidation())
                {
                    Name = entity.Name;
                    DatabaseName = entity.DatabaseName;
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
        /// DTO for <see cref="Tenant"/> persistence (memento pattern variation).
        /// </summary>
        [Serializable]
        public class TenantDto : DomainEntityDto
        {
            /// <inheritdoc />
            public TenantDto() : base() { }

            /// <inheritdoc />
            public TenantDto(Tenant entity) : base(entity)
            {

                Name = entity.Name;
                DatabaseName = entity.DatabaseName;
                InsertedAt = entity.InsertedAt ?? DateTime.MaxValue;
                InsertedBy = entity.InsertedBy;
            }


            /// <inheritdoc cref="ITenant.Name"/>
            public string Name { get; set; }

            /// <inheritdoc cref="ITenant.DatabaseName"/>
            public string DatabaseName { get; set; }

            /// <inheritdoc cref="ITenant.InsertedAt"/>
            public DateTime InsertedAt { get; set; }

            /// <inheritdoc cref="ITenant.InsertedBy"/>
            public string InsertedBy { get; set; }

        }

        #endregion
    }
}
