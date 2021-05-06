using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security.UserGroups
{
    /// <inheritdoc cref="IDeleteUserGroupUseCase"/>
    [DefaultServiceImplementation(typeof(IDeleteUserGroupUseCase))]
    public class DeleteUserGroupUseCase : CommandWithParameterUseCaseBase<IDomainEntityIdentity>, IDeleteUserGroupUseCase
    {
        private readonly ICacheProvider _cache;
        private readonly IUserGroupRepository _repository;


        /// <inheritdoc />
        public DeleteUserGroupUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, ICacheProvider cache, IUserGroupRepository repository) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        /// <inheritdoc cref="IDeleteUserGroupUseCase.Validate"/>
        public override BrokenRulesCollection Validate(IDomainEntityIdentity groupId)
        {
            if (groupId.IsNullOrNew()) return new BrokenRulesCollection(string.Format(
                Resources.No_Id_For_Deleted_Entity, MetadataProvider.GetEntityMetadata<UserGroup>().GetDisplayNameForOld()));

            return new BrokenRulesCollection();
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(IDomainEntityIdentity groupId)
        {
            Validate(groupId).ThrowOnError();
            
            var currentGroupDto = await _repository.FetchAsync(groupId);

            if (null == currentGroupDto) throw new NotFoundException(typeof(UserGroup),
                groupId.ToString(), MetadataProvider);

            if (currentGroupDto.UserCount > 0) throw new ValidationException(
                Resources.Security_DeleteUserGroupUseCase_Cannot_Delete_Non_Empty_UserGroup);

            await _repository.DeleteAsync(groupId);

            _cache.Clear(typeof(List<UserGroupLookup>));
        }

    }
}
