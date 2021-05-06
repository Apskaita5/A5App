using System;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IChangePasswordUseCase"/>
    [DefaultServiceImplementation(typeof(IChangePasswordUseCase), BuildConfiguration.Server)]
    public class ChangePasswordUseCase : CommandWithParameterUseCaseBase<ChangePasswordRequest>, IChangePasswordUseCase
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        /// <inheritdoc />
        public ChangePasswordUseCase(IUserRepository repository, IPasswordHasher passwordHasher,
            IAuthenticationStateProvider authenticationStateProvider, IAuthorizationProvider authorizer, 
            IClientDataPortal dataPortal, IValidationEngineProvider validationProvider, 
            IMetadataProvider metadataProvider, ILogger logger) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        protected override async Task ExecuteAsync(ChangePasswordRequest parameter)
        {
            if (null == parameter) throw new ArgumentNullException(nameof(parameter));

            Validate(parameter).ThrowOnError();

            var identity = await GetIdentityAsync();

            var (userId, passwordHash) = await _repository.FetchLoginInfoAsync(identity.Email());

            if (!userId.HasValue) throw new InvalidOperationException(
                $"User {identity.Name} (id = {identity.Sid()}) not found for a valid ClaimsIdentity.");

            if (!_passwordHasher.VerifyHashedPassword(passwordHash, parameter.CurrentPassword))
            {
                Logger.LogSecurityIssue($"User {identity.Email()} attempted to change his password with an invalid current password.");
                throw new ValidationException(Resources.Security_ChangePasswordUseCase_Password_Invalid);
            }

            await _repository.UpdatePasswordAsync(userId.ToIdentity<User>(), 
                _passwordHasher.HashPassword(parameter.NewPassword));
        }

    }
}
