using System;
using A5Soft.A5App.Domain.Core;
using A5Soft.A5App.Domain.Properties;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Domain
{
    /// <summary>
    /// an exception thrown on a concurrency conflict
    /// </summary>
    [Serializable]
    public class ConcurrencyException : BusinessException
    {
        /// <inheritdoc />
        public ConcurrencyException(IAuditable auditableDomainObject) 
            : base(string.Format(Resources.ConcurrencyException_Message, auditableDomainObject?.UpdatedBy))
        {
            if (auditableDomainObject.IsNull()) throw new ArgumentNullException(nameof(auditableDomainObject));
        }

        /// <inheritdoc />
        public ConcurrencyException(string updatedBy)
            : base(string.Format(Resources.ConcurrencyException_Message, updatedBy))
        { }
    }
}
