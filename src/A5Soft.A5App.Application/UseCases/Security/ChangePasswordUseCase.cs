using System;
using System.Security.Claims;
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
    [DefaultServiceImplementation(typeof(IChangePasswordUseCase))]
    public class ChangePasswordUseCase : CommandWithParameterUseCaseBase<ChangePasswordRequest>, IChangePasswordUseCase
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;

        /// <inheritdoc />
        public ChangePasswordUseCase(IUserRepository repository, IPasswordHasher passwordHasher, ClaimsIdentity user, 
            IUseCaseAuthorizer authorizer, IClientDataPortal dataPortal, IValidationEngineProvider validationProvider, 
            IMetadataProvider metadataProvider, ILogger logger) 
            : base(user, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        protected override async Task ExecuteAsync(ChangePasswordRequest parameter)
        {
            if (null == parameter) throw new ArgumentNullException(nameof(parameter));

            Validate(parameter).ThrowOnError();

            var (userId, passwordHash) = await _repository.FetchLoginInfoAsync(User.Email());

            if (!userId.HasValue) throw new InvalidOperationException(
                $"User {User.Name} (id = {User.Sid()}) not found for a valid ClaimsIdentity.");

            if (!_passwordHasher.VerifyHashedPassword(passwordHash, parameter.CurrentPassword))
            {
                Logger.LogSecurityIssue($"User {User.Email()} attempted to change his password with an invalid current password.");
                throw new ValidationException(Resources.Security_ChangePasswordUseCase_Password_Invalid);
            }

            await _repository.UpdatePasswordAsync(userId.ToEntityIdentity<User>(), 
                _passwordHasher.HashPassword(parameter.NewPassword));
        }

    }
}
