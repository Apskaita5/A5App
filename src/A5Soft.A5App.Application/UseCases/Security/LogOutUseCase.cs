using A5Soft.CARMA.Application;
using System;
using System.Security.Claims;
using A5Soft.A5App.Application.Infrastructure;
using System.Threading.Tasks;
using System.Threading;
using A5Soft.A5App.Application.UseCases.Security.ClaimsIdentityExtensions;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="ILogOutUseCase"/>
    [DefaultServiceImplementation(typeof(ILogOutUseCase))]
    public class LogOutUseCase : QueryUseCaseBase<ClaimsIdentity>, ILogOutUseCase
    {
        private readonly ISecurityTokenProvider _securityTokenProvider;


        /// <inheritdoc />
        public LogOutUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal,
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, ISecurityTokenProvider securityTokenProvider) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _securityTokenProvider = securityTokenProvider ?? throw new ArgumentNullException(nameof(securityTokenProvider));
        }


        protected override async Task<ClaimsIdentity> DoQueryAsync(CancellationToken ct)
        {
            var identity = await GetIdentityAsync();
            return await identity.ToLoggedOutClaimsIdentityAsync(_securityTokenProvider);
        }
    }
}
