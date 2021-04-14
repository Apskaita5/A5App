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

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IDeleteUserUseCase"/>
    [DefaultServiceImplementation(typeof(IDeleteUserUseCase))]
    public class DeleteUserUseCase : CommandWithParameterUseCaseBase<IDomainEntityIdentity>, IDeleteUserUseCase
    {
        private readonly ICacheProvider _cache;
        private readonly IUserRepository _repository;
        private readonly IFetchTenantLookupUseCase _tenantLookup;
        private readonly IUserGroupLookupService _userGroupLookup;
        private readonly IUserRoleLookupService _userRoleLookup;


        /// <inheritdoc />
        public DeleteUserUseCase(ClaimsIdentity user, IUseCaseAuthorizer authorizer,
            IClientDataPortal dataPortal, IValidationEngineProvider validationProvider,
            IMetadataProvider metadataProvider, ILogger logger, ICacheProvider cache,
            IUserRepository repository) : base(user, authorizer, dataPortal,
            validationProvider, metadataProvider, logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        /// <inheritdoc cref="IDeleteUserGroupUseCase.Validate"/>
        public override BrokenRulesCollection Validate(IDomainEntityIdentity parameter)
        {
            if (parameter.IsNullOrNew()) return new BrokenRulesCollection(string.Format(
                Resources.No_Id_For_Deleted_Entity, MetadataProvider.GetEntityMetadata<UserRole>().GetDisplayNameForOld()));
            return new BrokenRulesCollection();
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(IDomainEntityIdentity parameter)
        {
            Validate(parameter).ThrowOnError();

            if (parameter.IsSameIdentityAs(User.Sid().ToEntityIdentity<User>()))
            {
                Logger.LogSecurityIssue($"User {User.Name} attempted harakiri.");
                throw new ValidationException(Resources.Security_DeleteUserUseCase_No_Harakiri);
            }

            var tenantLookup = await _tenantLookup.QueryAsync();
            var userGroupLookup = await _userGroupLookup.FetchAsync(this.GetType());
            var roleLookup = await _userRoleLookup.FetchAsync(this.GetType());

            var dto = await _repository.FetchAsync(parameter, tenantLookup);

            if (null == dto) throw new NotFoundException(typeof(User),
                parameter.ToString(), MetadataProvider);

            var userToDelete = new User(dto, tenantLookup, roleLookup, userGroupLookup, ValidationProvider);

            if (User.IsGroupAdmin())
            {
                if (null == userToDelete.UserGroup)
                {
                    Logger.LogSecurityIssue($"User {User.Name} attempted to delete user {userToDelete.Email} " +
                        $"who does not belong to any group.");
                    throw new ValidationException(Resources.Security_DeleteUserUseCase_Cannot_Delete_Null_Group);
                }

                if (!userToDelete.UserGroup.Id.IsSameIdentityAs(User.GroupSid().ToEntityIdentity<UserGroup>()))
                {
                    Logger.LogSecurityIssue($"User {User.Name} attempted to delete user {userToDelete.Email} " +
                        $"who belongs to other group ({userToDelete.UserGroup.GroupName}).");
                    throw new ValidationException(Resources.Security_DeleteUserUseCase_Cannot_Delete_Other_Group);
                }
            }

            await _repository.DeleteAsync(parameter);

            _cache.Clear(typeof(List<UserLookup>));
        }

    }
}
