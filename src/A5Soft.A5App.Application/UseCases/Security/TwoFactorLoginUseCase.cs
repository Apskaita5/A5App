using System;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.UseCases.Security.ClaimsIdentityExtensions;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="ITwoFactorLoginUseCase"/>
    [DefaultServiceImplementation(typeof(ITwoFactorLoginUseCase))]
    public class TwoFactorLoginUseCase : UnauthenticatedQueryWithCriteriaUseCaseBase<LoginResponse, 
        TwoFactorLoginRequest>, ITwoFactorLoginUseCase
    {
        private readonly ISecurityTokenProvider _securityTokenProvider;
        private readonly ISecurityPolicy _policy;

        /// <inheritdoc />
        public TwoFactorLoginUseCase(IClientDataPortal dataPortal, IValidationEngineProvider validationProvider,
            IMetadataProvider metadataProvider, ILogger logger, ISecurityTokenProvider securityTokenProvider,
            ISecurityPolicy policy) : base(dataPortal, validationProvider, metadataProvider, logger)
        {
            _securityTokenProvider =
                securityTokenProvider ?? throw new ArgumentNullException(nameof(securityTokenProvider));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }


        protected override async Task<LoginResponse> QueryIntAsync(TwoFactorLoginRequest criteria, CancellationToken ct)
        {
            if (null == criteria) throw new ArgumentNullException(nameof(criteria));

            Validate(criteria).ThrowOnError();

            var user = TwoFactorAuthenticationManager.ConsumeRequest(criteria.Token);

            if (null == user)
            {
                Logger.LogSecurityIssue($"Attempt to login with an invalid two factor authentication token {criteria.Token}.");
                return new LoginResponse("Token is invalid or expired.");
            }

            Logger.LogSecurityIssue($"User {user.Email} has successfully logged in after two factor authentication.");

            return new LoginResponse(await user.ToClaimsIdentityAsync(_policy, _securityTokenProvider));
        }
    }
}
