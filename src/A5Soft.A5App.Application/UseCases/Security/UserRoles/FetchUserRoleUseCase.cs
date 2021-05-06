using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security.UserRoles
{
    /// <inheritdoc cref="IFetchUserRoleUseCase"/>
    [DefaultServiceImplementation(typeof(IFetchUserRoleUseCase))]
    public class FetchUserRoleUseCase : FetchDomainEntityUseCaseBase<UserRole>, IFetchUserRoleUseCase
    {
        private readonly IUserRoleRepository _repository;


        /// <inheritdoc />
        public FetchUserRoleUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, IUserRoleRepository repository)
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override async Task<UserRole> DoFetchAsync(IDomainEntityIdentity id, CancellationToken ct)
        {
            var dto = await _repository.FetchAsync(id, ct);

            if (null == dto) throw new NotFoundException(typeof(UserRole),
                id.ToString(), MetadataProvider);

            return new UserRole(dto, ValidationProvider);
        }

    }
}
