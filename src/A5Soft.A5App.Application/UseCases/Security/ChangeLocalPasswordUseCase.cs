using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using System;
using System.Threading.Tasks;
using A5Soft.A5App.Domain;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// implementation of <see cref="IChangePasswordUseCase"/> for local (desktop) app
    /// </summary> 
    [DefaultServiceImplementation(typeof(IChangePasswordUseCase), BuildConfiguration.Client)]
    public class ChangeLocalPasswordUseCase : CommandWithParameterUseCaseBase<ChangePasswordRequest>, IChangePasswordUseCase
    {
        private readonly ILocalSecurityRepository _repository;


        /// <inheritdoc />
        public ChangeLocalPasswordUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, ILocalSecurityRepository repository) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override async Task ExecuteAsync(ChangePasswordRequest parameter)
        {
            if (null == parameter) throw new ArgumentNullException(nameof(parameter));

            var identity = await GetIdentityAsync();
            var tenantId = identity.TenantSid();

            if (!tenantId.HasValue) throw new ValidationException(
                "User is not logged in to any database.");

            parameter.NewPassword = parameter.NewPassword?.Trim() ?? string.Empty;
            parameter.CurrentPassword = parameter.CurrentPassword?.Trim() ?? string.Empty;
            parameter.RepeatedPassword = parameter.RepeatedPassword?.Trim() ?? string.Empty;

            if (parameter.NewPassword != parameter.RepeatedPassword) throw new ValidationException(
                "Repeated password does not match new password.");

            var loginResult = await _repository.FetchLoginResponseAsync(tenantId.Value, parameter.CurrentPassword);
           
            if (loginResult.RequirePassword) throw new ValidationException("Current password is not specified.");
            if (!loginResult.Success) throw new ValidationException(loginResult.ErrorMessage);
                             
            _repository.UpdatePassword(parameter.NewPassword);
        }

    }

}