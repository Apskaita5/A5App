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

namespace A5Soft.A5App.Application.UseCases.Security.Users
{
    /// <inheritdoc cref="IUpdateUserUseCase"/>
    [DefaultServiceImplementation(typeof(IUpdateUserUseCase))]
    public class UpdateUserUseCase : SaveUseCaseBase<User, IUser>, IUpdateUserUseCase
    {
        private readonly ICacheProvider _cache;
        private readonly IUserRepository _repository;
        private readonly IFetchTenantLookupUseCase _tenantLookup;
        private readonly IUserGroupLookupService _userGroupLookup;
        private readonly IUserRoleLookupService _userRoleLookup;


        /// <inheritdoc />
        public UpdateUserUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal,
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, ILogger logger,
            ICacheProvider cache, IUserRepository repository, IFetchTenantLookupUseCase tenantLookup,
            IUserGroupLookupService userGroupLookup, IUserRoleLookupService userRoleLookup) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _tenantLookup = tenantLookup ?? throw new ArgumentNullException(nameof(tenantLookup));
            _userGroupLookup = userGroupLookup ?? throw new ArgumentNullException(nameof(userGroupLookup));
            _userRoleLookup = userRoleLookup ?? throw new ArgumentNullException(nameof(userRoleLookup));
        }


        protected override async Task<User> DoSaveAsync(IUser domainDto)
        {
            if (domainDto.Id.IsNullOrNew()) throw new ValidationException(
                new BrokenRulesCollection(string.Format(Resources.No_Id_For_Updated_Entity,
                    MetadataProvider.GetEntityMetadata<User>().GetDisplayNameForOld())));

            var identity = await GetIdentityAsync();

            if (domainDto.Id.IsSameIdentityAs(identity.Sid().ToIdentity<User>()))
            {
                Logger.LogSecurityIssue($"User {identity.Name} attempted to update data about self.");
                throw new AuthorizationException(Resources.Security_UpdateUserUseCase_Cannot_Update_Self);
            }

            var tenantLookup = await _tenantLookup.QueryAsync();
            var userGroupLookup = await _userGroupLookup.FetchAsync(this.GetType());
            var roleLookup = await _userRoleLookup.FetchAsync(this.GetType());

            var currentDto = await _repository.FetchAsync(domainDto.Id, tenantLookup);

            if (null == currentDto) throw new NotFoundException(typeof(User),
                domainDto.Id.ToString(), MetadataProvider);

            var current = new User(currentDto, tenantLookup, roleLookup, userGroupLookup, ValidationProvider);

            if (identity.IsGroupAdmin())
            {
                if (null == current.UserGroup ||
                    !identity.GroupSid().ToIdentity<UserGroup>().IsSameIdentityAs(current.UserGroup.Id))
                {
                    Logger.LogSecurityIssue($"User {identity.Name} attempted to update data of the user " +
                        $"outside of his group - {current.Email}.");
                    throw new AuthorizationException(Resources.Security_UpdateUserUseCase_No_Update_For_Other_Groups);
                }
                if (current.AdminRole != AdministrativeRole.None)
                {
                    Logger.LogSecurityIssue($"User {identity.Name} attempted to update data of the admin user " +
                        $"- {current.Email}.");
                    throw new AuthorizationException(Resources.Security_UpdateUserUseCase_No_Update_Admin_Users);
                }

                if (domainDto.UserGroupId.IsNullOrNew())
                {
                    Logger.LogSecurityIssue($"User {identity.Name} tried to set user ({domainDto.Email}) group to null.");
                    throw new ValidationException(Resources.Security_UpdateUserUseCase_Cannot_Update_UserGroup);
                }
                if (!domainDto.UserGroupId.IsSameIdentityAs(identity.GroupSid().ToIdentity<UserGroup>()))
                {
                    Logger.LogSecurityIssue($"User {identity.Name} tried to update user ({domainDto.Email}) group.");
                    throw new ValidationException(Resources.Security_UpdateUserUseCase_Cannot_Update_UserGroup);
                }
                if (domainDto.AdminRole != AdministrativeRole.None)
                {
                    Logger.LogSecurityIssue($"User {identity.Name} tried to grant user ({domainDto.Email}) admin privileges.");
                    throw new ValidationException(Resources.Security_UpdateUserUseCase_Cannot_Grant_Admin);
                }
            }

            current.Merge(domainDto, userGroupLookup, roleLookup, true);

            var result = await _repository.UpdateAsync(current.ToDto(), identity.Email());

            _cache.Clear(typeof(List<UserLookup>));

            return new User(result, tenantLookup, roleLookup, userGroupLookup, ValidationProvider);
        }

    }
}
