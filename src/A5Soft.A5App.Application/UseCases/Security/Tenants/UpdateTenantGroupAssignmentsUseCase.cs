using System;
using System.Threading.Tasks;
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

namespace A5Soft.A5App.Application.UseCases.Security.Tenants
{
    /// <inheritdoc cref="IUpdateTenantGroupAssignmentsUseCase"/>
    [DefaultServiceImplementation(typeof(IUpdateTenantGroupAssignmentsUseCase))]
    public class UpdateTenantGroupAssignmentsUseCase : SaveUseCaseBase<TenantGroupAssignments, ITenantGroupAssignments>, 
        IUpdateTenantGroupAssignmentsUseCase
    {
        private readonly IUserGroupLookupService _lookupService;
        private readonly ITenantRepository _repository;


        /// <inheritdoc />
        public UpdateTenantGroupAssignmentsUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, IUserGroupLookupService lookupService, ITenantRepository repository) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override async Task<TenantGroupAssignments> DoSaveAsync(ITenantGroupAssignments domainDto)
        {
            if (domainDto.Id.IsNullOrNew()) throw new ValidationException(
                new BrokenRulesCollection(string.Format(Resources.No_Id_For_Updated_Entity,
                    MetadataProvider.GetEntityMetadata<TenantGroupAssignment>().GetDisplayNameForOld())));

            var currentDto = await _repository.FetchAssignmentsAsync(domainDto.Id);

            if (null == currentDto) throw new NotFoundException(typeof(TenantGroupAssignment),
                domainDto.Id.ToString(), MetadataProvider);

            var lookup = await _lookupService.FetchAsync(this.GetType());

            var current = new TenantGroupAssignments(currentDto, lookup, ValidationProvider);

            current.Merge(domainDto, lookup, ValidationProvider);

            var result = await _repository.UpdateAssignmentsAsync(current.ToDto());

            return new TenantGroupAssignments(result, lookup, ValidationProvider);
        }

    }
}
