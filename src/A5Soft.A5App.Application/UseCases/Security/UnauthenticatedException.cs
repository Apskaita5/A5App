using System;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Core;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// An exception that is thrown when an unauthenticated user attempts to invoke non public use case.
    /// </summary>
    [Serializable]
    public sealed class UnauthenticatedException : BusinessException
    {
        public UnauthenticatedException() : base(Resources.UnAuthenticatedException_Message) { }
    }
}
