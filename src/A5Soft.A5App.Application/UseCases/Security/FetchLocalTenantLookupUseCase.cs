using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Domain.Metadata;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <summary>
    /// implementation of <see cref="IFetchTenantLookupUseCase"/> for local (desktop) app
    /// </summary> 
    [DefaultServiceImplementation(typeof(IFetchTenantLookupUseCase), BuildConfiguration.Client)]
    public class FetchLocalTenantLookupUseCase : QueryListUseCaseBase<TenantLookup>, IFetchTenantLookupUseCase
    {
        private readonly ILocalSecurityRepository _repository;


        /// <inheritdoc />
        public FetchLocalTenantLookupUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, ILocalSecurityRepository repository) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override Task<List<TenantLookup>> DoQueryAsync(CancellationToken ct)
        {
            return _repository.FetchTenantLookupAsync();
        }
    }
}
