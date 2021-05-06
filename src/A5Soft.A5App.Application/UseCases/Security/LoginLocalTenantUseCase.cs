using System;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="ILoginLocalTenantUseCase"/> 
    [DefaultServiceImplementation(typeof(ILoginUseCase), BuildConfiguration.Client)]
    public class LoginLocalTenantUseCase : IUseCase, ILoginLocalTenantUseCase
    {
        private readonly ILocalSecurityRepository _repository;


        public LoginLocalTenantUseCase(ILocalSecurityRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        /// <inheritdoc cref="ILoginLocalTenantUseCase.Invoke"/> 
        public Task<LocalLoginResponse> Invoke(Guid tenantId, string password)
        {
            return _repository.FetchLoginResponseAsync(tenantId, password);
        }

    }
}
