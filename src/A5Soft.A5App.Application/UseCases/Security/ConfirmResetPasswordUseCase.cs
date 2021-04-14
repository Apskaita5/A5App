using System;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IConfirmResetPasswordUseCase"/>
    [DefaultServiceImplementation(typeof(IConfirmResetPasswordUseCase))]
    public class ConfirmResetPasswordUseCase : UnauthenticatedQueryWithCriteriaUseCaseBase<LoginResponse, string>, 
        IConfirmResetPasswordUseCase
    {
        private readonly IUserRepository _repository;
        private readonly ISecurityPolicy _policy;
        private readonly IPasswordHasher _passwordHasher;


        /// <inheritdoc />
        public ConfirmResetPasswordUseCase(IUserRepository repository, ISecurityPolicy policy,
            IPasswordHasher passwordHasher, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger) : base(dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        
        protected override async Task<LoginResponse> QueryIntAsync(string token, CancellationToken ct)
        {
            if (token.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(token));

            var (UserId, NewPassword) = ResetPasswordRequestManager.ConsumeRequest(token);

            if (!UserId.HasValue)
            {
                Logger.LogSecurityIssue($"Attempted password reset confirmation with an invalid " +
                    $"or expired token ({token}).");
                return new LoginResponse("Invalid reset password request token or it has expired."); 
            }

            var userDetails = await _repository.FetchUserIdentityAsync(
                UserId.Value, null, ct);

            if (null == userDetails) throw new InvalidOperationException(
                $"Failed to fetch user details for valid reset password request (id = {UserId.ToString()}).");

            if (userDetails.IsDisabled)
            {
                Logger.LogSecurityIssue($"User {userDetails.Email} " +
                    $"attempted to reset password even though his account is disabled.");
                return new LoginResponse(Resources.Security_ConfirmResetPasswordUseCase_Account_Disabled);
            }

            await _repository.UpdatePasswordAsync(UserId.ToEntityIdentity<User>(), 
                _passwordHasher.HashPassword(NewPassword));

            Logger.LogSecurityIssue($"User {userDetails.Email} has successfully logged in " +
                $"by confirming password reset.");

            return new LoginResponse(userDetails.ToClaimsIdentity(_policy));
        }
    }
}
