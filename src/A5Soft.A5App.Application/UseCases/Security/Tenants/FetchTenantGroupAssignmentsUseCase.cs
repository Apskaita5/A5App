using System;
using System.Collections.Generic;
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

namespace A5Soft.A5App.Application.UseCases.Security.Tenants
{
    /// <inheritdoc cref="IFetchTenantGroupAssignmentsUseCase"/>
    [DefaultServiceImplementation(typeof(IFetchTenantGroupAssignmentsUseCase))]
    public class FetchTenantGroupAssignmentsUseCase : FetchDomainEntityUseCaseBase<TenantGroupAssignments>, 
        IFetchTenantGroupAssignmentsUseCase
    {
        private readonly ITenantRepository _repository;
        private readonly IUserGroupLookupService _lookupService;


        /// <inheritdoc />
        public FetchTenantGroupAssignmentsUseCase(ITenantRepository repository, 
            IUserGroupLookupService lookupService, IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, ILogger logger) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
        }


        /// <inheritdoc cref="IFetchTenantGroupAssignmentsUseCase.FetchLookupAsync"/>
        public Task<List<UserGroupLookup>> FetchLookupAsync()
        {
            return _lookupService.FetchAsync(this.GetType());
        }

        protected override async Task<TenantGroupAssignments> DoFetchAsync(IDomainEntityIdentity id, CancellationToken ct)
        {
            var dto = await _repository.FetchAssignmentsAsync(id, ct);

            if (null == dto) throw new NotFoundException(typeof(Tenant), id.ToString(), MetadataProvider);

            var lookup = await _lookupService.FetchAsync(this.GetType());

            return new TenantGroupAssignments(dto, lookup, ValidationProvider);
        }

    }
}
