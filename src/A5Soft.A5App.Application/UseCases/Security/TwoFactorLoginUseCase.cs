using System;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly ISecurityPolicy _policy;

        /// <inheritdoc />
        public TwoFactorLoginUseCase(ISecurityPolicy policy, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger) : base(dataPortal, validationProvider, metadataProvider, logger)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        protected override Task<LoginResponse> QueryIntAsync(TwoFactorLoginRequest criteria, CancellationToken ct)
        {
            if (null == criteria) throw new ArgumentNullException(nameof(criteria));

            Validate(criteria).ThrowOnError();

            var user = TwoFactorAuthenticationManager.ConsumeRequest(criteria.Token);

            if (null == user)
            {
                Logger.LogSecurityIssue($"Attempt to login with an invalid two factor authentication token {criteria.Token}.");
                return Task.FromResult(new LoginResponse("Token is invalid or expired."));
            }

            Logger.LogSecurityIssue($"User {user.Email} has successfully logged in after two factor authentication.");

            return Task.FromResult(new LoginResponse(user.ToClaimsIdentity(_policy)));
        }
    }
}
