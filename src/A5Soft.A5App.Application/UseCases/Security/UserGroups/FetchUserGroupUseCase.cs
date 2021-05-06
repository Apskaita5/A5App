using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.CARMA.Domain.Metadata;
using System.Threading;

namespace A5Soft.A5App.Application.UseCases.Security.UserGroups
{
    /// <inheritdoc cref="IFetchUserGroupUseCase"/>
    [DefaultServiceImplementation(typeof(IFetchUserGroupUseCase))]
    public class FetchUserGroupUseCase : FetchDomainEntityUseCaseBase<UserGroup>, IFetchUserGroupUseCase
    {
        private readonly IUserGroupRepository _repository;


        /// <inheritdoc />
        public FetchUserGroupUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, IUserGroupRepository repository) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
                   

        protected override async Task<UserGroup> DoFetchAsync(IDomainEntityIdentity id, CancellationToken ct)
        {
            var currentGroupDto = await _repository.FetchAsync(id, ct);

            if (null == currentGroupDto) throw new NotFoundException(typeof(UserGroup),
                id.ToString(), MetadataProvider);

            return new UserGroup(currentGroupDto, ValidationProvider);
        }

    }
}
