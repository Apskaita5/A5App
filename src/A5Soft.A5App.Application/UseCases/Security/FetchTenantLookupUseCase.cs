using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IFetchTenantLookupUseCase"/>
    [DefaultServiceImplementation(typeof(IFetchTenantLookupUseCase))]
    public class FetchTenantLookupUseCase : QueryListUseCaseBase<TenantLookup>, IFetchTenantLookupUseCase
    {
        private readonly ITenantRepository _repository;

        /// <inheritdoc />
        public FetchTenantLookupUseCase(ITenantRepository repository, ClaimsIdentity user, IUseCaseAuthorizer authorizer, 
            IClientDataPortal dataPortal, IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger) : base(user, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        protected override Task<List<TenantLookup>> DoQueryAsync(CancellationToken ct)
        {
            return _repository.FetchLookupForUserAsync(User.Sid().ToEntityIdentity<User>(), ct);
        }
    }
}
