using System;
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
    /// <inheritdoc cref="IResetPasswordUseCase"/>
    [DefaultServiceImplementation(typeof(IResetPasswordUseCase))]
    public class ResetPasswordUseCase : UnauthenticatedCommandWithParameterUseCaseBase<ResetPasswordRequest>, 
        IResetPasswordUseCase
    {
        private readonly IUserRepository _repository;
        private readonly ISecurityPolicy _policy;
        private readonly ISecureRandomProvider _secureRandomProvider;
        private readonly IEmailProvider _emailProvider;
        private readonly IBaseUrlProvider _urlProvider;


        /// <inheritdoc />
        public ResetPasswordUseCase(IClientDataPortal dataPortal, IValidationEngineProvider validationProvider,
            IMetadataProvider metadataProvider, ILogger logger, IUserRepository repository, ISecurityPolicy policy,
            ISecureRandomProvider secureRandomProvider, IEmailProvider emailProvider, IBaseUrlProvider urlProvider) :
            base(dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _secureRandomProvider =
                secureRandomProvider ?? throw new ArgumentNullException(nameof(secureRandomProvider));
            _emailProvider = emailProvider ?? throw new ArgumentNullException(nameof(emailProvider));
            _urlProvider = urlProvider ?? throw new ArgumentNullException(nameof(urlProvider));
        }


        protected override async Task ExecuteAsync(ResetPasswordRequest parameter)
        {
            if (null == parameter) throw new ArgumentNullException(nameof(parameter));

            Validate(parameter).ThrowOnError();

            var loginCredentials = await _repository.FetchLoginInfoAsync(parameter.Email);

            if (!loginCredentials.Id.HasValue)
            {
                Logger.LogSecurityIssue($"Person identified by email {parameter.Email} " +
                    $"attempted to reset password even though he is not an app user.");
                return; // pretend its ok
            }

            var userDetails = await _repository.FetchUserIdentityAsync(
                loginCredentials.Id.Value, null);

            if (null == userDetails) throw new InvalidOperationException(
                $"Failed to fetch user details (id = {loginCredentials.Id}).");

            if (userDetails.IsDisabled)
            {
                Logger.LogSecurityIssue($"User {parameter.Email} " +
                    $"attempted to reset password even though his account is disabled.");
                throw new AuthorizationException(
                    Resources.Security_ResetPasswordUseCase_Cannot_Reset_For_Disabled_Account);
            }

            if (_urlProvider.BaseUrl.IsNullOrWhiteSpace()) throw new InvalidOperationException(
                "Request URL cannot be resolved.");

            var resetUrl = $"{_urlProvider.BaseUrl.Trim().TrimEnd('/')}/{_policy.ConfirmPasswordResetUrl.Trim().TrimStart('/')}";

            var result = ResetPasswordRequestManager.RegisterRequest(
                loginCredentials.Id.Value, _secureRandomProvider, _policy);

            Logger.LogSecurityIssue($"User {parameter.Email} " +
                $"request to reset password has been registered.");

            await _emailProvider.SendAsync(new EmailMessage()
            {
                Email = userDetails.Email,
                AllowMultipleEmails = false,
                Subject = _policy.ConfirmPasswordResetMessageSubject,
                Content = string.Format(_policy.ConfirmPasswordResetMessageContent,
                    userDetails.Name, result.NewPassword,
                    string.Format(resetUrl, result.UrlToken)),
                IsHtml = true
            });

            Logger.LogSecurityIssue($"A message for password reset confirmation " +
                $"has been sent to user {parameter.Email}.");
        }
    }
}
