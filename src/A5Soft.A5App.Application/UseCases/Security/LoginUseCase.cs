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
    /// <inheritdoc cref="ILoginUseCase"/>
    [DefaultServiceImplementation(typeof(ILoginUseCase))]
    public class LoginUseCase : UnauthenticatedQueryWithCriteriaUseCaseBase<LoginResponse, LoginRequest>, 
        ILoginUseCase
    {
        private readonly IUserRepository _repository;
        private readonly ISecurityPolicy _policy;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ISecureRandomProvider _secureRandomProvider;
        private readonly ITwoFactorProvider _twoFactorProvider;


        /// <inheritdoc />
        public LoginUseCase(IUserRepository repository, ISecurityPolicy policy, 
            IPasswordHasher passwordHasher, ISecureRandomProvider secureRandomProvider,
            ITwoFactorProvider twoFactorProvider, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger) : base(dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _secureRandomProvider =
                secureRandomProvider ?? throw new ArgumentNullException(nameof(secureRandomProvider));
            _twoFactorProvider = twoFactorProvider ?? throw new ArgumentNullException(nameof(twoFactorProvider));
        }


        protected override async Task<LoginResponse> QueryIntAsync(LoginRequest criteria, CancellationToken ct)
        {
            if (null == criteria) throw new ArgumentNullException(nameof(criteria));

            Validate(criteria).ThrowOnError();

            var lockoutEndsAt = LockoutManager.LockoutEndsAt(criteria.Email);
            if (lockoutEndsAt.HasValue)
            {
                TimeSpan diff = lockoutEndsAt.Value.Subtract(DateTime.UtcNow);
                return new LoginResponse(string.Format(
                    Resources.Security_LoginUseCase_Account_LockedOut,
                    diff.Minutes, diff.Seconds)); 
            }

            var (userId, passwordHash) = await _repository.FetchLoginInfoAsync(criteria.Email);

            if (!userId.HasValue || !_passwordHasher.VerifyHashedPassword(
                passwordHash, criteria.Password))
            {
                if (userId.HasValue) Logger.LogSecurityIssue(
                    $"User {criteria.Email} attempted login with an invalid password.");
                else Logger.LogSecurityIssue($"User {criteria.Email} attempted login with an invalid email.");

                LockoutManager.RegisterFailedAttempt(criteria.Email, _policy);

                return new LoginResponse(Resources.Security_LoginUseCase_Invalid_Password_Or_Email);
            }

            var userDetails = await _repository.FetchUserIdentityAsync(
                userId.Value, null, ct);

            if (null == userDetails) throw new Exception(
                $"Failed to fetch user details (id = {userId}).");

            LockoutManager.RegisterSuccessfulLogin(criteria.Email);

            if (userDetails.IsDisabled)
            {
                Logger.LogSecurityIssue($"User {criteria.Email} " +
                    $"attempted to login even though his account is disabled.");
                return new LoginResponse(Resources.Security_LoginUseCase_Account_Disabled);
            }

            if (_policy.RequireTwoFactorAuthentication || userDetails.TwoFactorEnabled)
            {
                Logger.LogSecurityIssue($"User {criteria.Email} was requested two factor authentication.");

                var token = TwoFactorAuthenticationManager.RegisterRequest(
                    userDetails, _secureRandomProvider, _policy);

                await _twoFactorProvider.SendSecondFactorRequest(token, userDetails);

                return new LoginResponse();
            }

            Logger.LogSecurityIssue($"User {criteria.Email} has successfully logged in.");

            return new LoginResponse(userDetails.ToClaimsIdentity(_policy));
        }

    }
}
