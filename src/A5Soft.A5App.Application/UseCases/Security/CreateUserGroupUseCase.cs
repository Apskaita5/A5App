using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
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
    /// <inheritdoc cref="ICreateUserGroupUseCase"/>
    [DefaultServiceImplementation(typeof(ICreateUserGroupUseCase))]
    public class CreateUserGroupUseCase : SaveUseCaseBase<UserGroup, IUserGroup>, ICreateUserGroupUseCase
    {
        private readonly ICacheProvider _cache;
        private readonly IUserGroupRepository _repository;


        /// <inheritdoc />
        public CreateUserGroupUseCase(ClaimsIdentity user, IUseCaseAuthorizer authorizer, 
            IClientDataPortal dataPortal, IValidationEngineProvider validationProvider, 
            IMetadataProvider metadataProvider, ILogger logger, ICacheProvider cache, 
            IUserGroupRepository repository) : base(user, authorizer, dataPortal,
            validationProvider, metadataProvider, logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override async Task<UserGroup> DoSaveAsync(IUserGroup domainDto)
        {
            var newGroup = new UserGroup(ValidationProvider);

            newGroup.Merge(domainDto, true);

            var result = await _repository.InsertAsync(newGroup.ToDto(), User.Email());

            _cache.Clear(typeof(List<UserGroupLookup>));

            return new UserGroup(result, ValidationProvider);
        }

    }
}
