using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using A5Soft.A5App.Domain;
using A5Soft.CARMA.Domain.Metadata;

namespace A5Soft.A5App.Application.UseCases.Security.UserGroups
{
    /// <inheritdoc cref="IUpdateUserGroupUseCase"/>
    [DefaultServiceImplementation(typeof(IUpdateUserGroupUseCase))]
    public class UpdateUserGroupUseCase : SaveUseCaseBase<UserGroup, IUserGroup>, IUpdateUserGroupUseCase
    {
        private readonly ICacheProvider _cache;
        private readonly IUserGroupRepository _repository;


        /// <inheritdoc />
        public UpdateUserGroupUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, ICacheProvider cache, IUserGroupRepository repository) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        

        protected override async Task<UserGroup> DoSaveAsync(IUserGroup domainDto)
        {
            if (domainDto.Id.IsNullOrNew()) throw new ValidationException(
                new BrokenRulesCollection(string.Format(Resources.No_Id_For_Updated_Entity, 
                    MetadataProvider.GetEntityMetadata<UserGroup>().GetDisplayNameForOld())));

            var currentGroupDto = await _repository.FetchAsync(domainDto.Id);

            if (null == currentGroupDto) throw new NotFoundException(typeof(UserGroup), 
                domainDto.Id.ToString(), MetadataProvider);

            var currentGroup = new UserGroup(currentGroupDto, ValidationProvider);

            currentGroup.Merge(domainDto, true);

            var identity = await GetIdentityAsync();

            var result = await _repository.UpdateAsync(currentGroup.ToDto(), identity.Email());

            _cache.Clear(typeof(List<UserGroupLookup>));

            return new UserGroup(result, ValidationProvider);
        }

    }
}
