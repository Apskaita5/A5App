using System;
using System.Collections.Generic;
using System.Security.Claims;
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

namespace A5Soft.A5App.Application.UseCases.Security.UserRoles
{
    /// <inheritdoc cref="IDeleteUserRoleUseCase"/>
    [DefaultServiceImplementation(typeof(IDeleteUserRoleUseCase))]
    public class DeleteUserRoleUseCase : CommandWithParameterUseCaseBase<IDomainEntityIdentity>, IDeleteUserRoleUseCase
    {
        private readonly ICacheProvider _cache;
        private readonly IUserRoleRepository _repository;


        /// <inheritdoc />
        public DeleteUserRoleUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, ICacheProvider cache, IUserRoleRepository repository) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        /// <inheritdoc cref="IDeleteUserRoleUseCase.Validate"/>
        public override BrokenRulesCollection Validate(IDomainEntityIdentity roleId)
        {
            if (roleId.IsNullOrNew()) return new BrokenRulesCollection(string.Format(
                Resources.No_Id_For_Deleted_Entity, MetadataProvider.GetEntityMetadata<UserRole>().GetDisplayNameForOld()));
            return new BrokenRulesCollection();
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(IDomainEntityIdentity roleId)
        {
            Validate(roleId).ThrowOnError();

            var current = await _repository.FetchAsync(roleId);

            if (null == current) throw new NotFoundException(typeof(UserRole),
                roleId.ToString(), MetadataProvider);

            if (current.UserCount > 0) throw new ValidationException(
                Resources.Security_DeleteUserRoleUseCase_Cannot_Delete_Role_In_Use);

            await _repository.DeleteAsync(roleId);

            _cache.Clear(typeof(List<UserRoleLookup>));
        }

    }
}
