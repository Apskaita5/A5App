using System;
using System.Runtime.Serialization;

namespace A5Soft.A5App.Domain.Core
{
    /// <summary>
    /// a base class for business logic exceptions, i.e. exceptions that are related to
    /// business data or state and should be handled by the user.
    /// </summary>
    [Serializable]
    public abstract class BusinessException : Exception
    {
        /// <inheritdoc />
        protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <inheritdoc />
        protected BusinessException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        protected BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
