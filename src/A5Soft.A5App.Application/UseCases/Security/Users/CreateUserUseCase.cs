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

namespace A5Soft.A5App.Application.UseCases.Security.Users
{
    /// <inheritdoc cref="ICreateUserUseCase"/>
    [DefaultServiceImplementation(typeof(ICreateUserUseCase))]
    public class CreateUserUseCase : SaveUseCaseBase<User, IUser>, ICreateUserUseCase
    {
        private readonly ICacheProvider _cache;
        private readonly IUserRepository _repository;
        private readonly IFetchTenantLookupUseCase _tenantLookup;
        private readonly IUserGroupLookupService _userGroupLookup;
        private readonly IUserRoleLookupService _userRoleLookup;
        private readonly IBaseUrlProvider _urlProvider;
        private readonly IEmailProvider _emailProvider;
        private readonly ISecurityPolicy _securityPolicy;
        private readonly ISecureRandomProvider _randomProvider;
        private readonly IPasswordHasher _passwordHasher;


        /// <inheritdoc />
        public CreateUserUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal,
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, ILogger logger,
            ICacheProvider cache, IUserRepository repository, IFetchTenantLookupUseCase tenantLookup,
            IUserGroupLookupService userGroupLookup, IUserRoleLookupService userRoleLookup,
            IBaseUrlProvider urlProvider) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _tenantLookup = tenantLookup ?? throw new ArgumentNullException(nameof(tenantLookup));
            _userGroupLookup = userGroupLookup ?? throw new ArgumentNullException(nameof(userGroupLookup));
            _userRoleLookup = userRoleLookup ?? throw new ArgumentNullException(nameof(userRoleLookup));
            _urlProvider = urlProvider ?? throw new ArgumentNullException(nameof(urlProvider));
        }


        protected override async Task<User> DoSaveAsync(IUser domainDto)
        {
            var identity = await GetIdentityAsync();
            if (identity.IsGroupAdmin())
            {
                if (domainDto.UserGroupId.IsNullOrNew())
                {
                    Logger.LogSecurityIssue($"User {identity.Name} tried to create a user ({domainDto.Email}) without any group.");
                    throw new ValidationException(Resources.Security_CreateUserUseCase_Cannot_Create_Without_Group);
                }
                if (!domainDto.UserGroupId.IsSameIdentityAs(identity.GroupSid().ToIdentity<UserGroup>()))
                {
                    Logger.LogSecurityIssue($"User {identity.Name} tried to create a user ({domainDto.Email}) for for invalid group.");
                    throw new ValidationException(Resources.Security_CreateUserUseCase_Cannot_Create_For_Other_Group);
                }
                if (domainDto.AdminRole != AdministrativeRole.None)
                {
                    Logger.LogSecurityIssue($"User {identity.Name} tried to create a user ({domainDto.Email}) with admin privileges.");
                    throw new ValidationException(Resources.Security_CreateUserUseCase_Cannot_Create_With_Admin_Privileges);
                }
            }

            var tenantLookup = await _tenantLookup.QueryAsync();
            var userGroupLookup = await _userGroupLookup.FetchAsync(this.GetType());
            var roleLookup = await _userRoleLookup.FetchAsync(this.GetType());

            var current = new User(tenantLookup, ValidationProvider);

            current.Merge(domainDto, userGroupLookup, roleLookup, true);

            if (null != current.UserGroup)
            {
                var currentUserCount = await _repository.CountUsersInGroupAsync(current.UserGroup.Id);
                if (currentUserCount >= current.UserGroup.MaxTenants)
                {
                    Logger.LogSecurityIssue($"User {identity.Name} tried to create a user ({domainDto.Email}) " +
                        $"exceeding max user limit for group {current.UserGroup.GroupName}.");
                    throw new ValidationException(Resources.Security_CreateUserUseCase_User_Group_Already_Full);
                }
            }

            var newPassword = _randomProvider.CreateNewPassword(_securityPolicy.NewPasswordLength);
                
            var dto = current.ToDto();

            dto.HashedPassword = _passwordHasher.HashPassword(newPassword);

            var result = await _repository.InsertAsync(dto, identity.Email());

            _cache.Clear(typeof(List<UserLookup>));

            try
            {
                await _emailProvider.SendAsync(new EmailMessage()
                {
                    Content = string.Format(_securityPolicy.InviteUserMessageContent, result.Name,
                            _urlProvider.BaseUrl, result.Email, newPassword),
                    Email = result.Email,
                    IsHtml = true,
                    Subject = _securityPolicy.InviteUserMessageSubject,
                    OnBehalfOfDisplayName = identity.Name,
                    OnBehalfOfEmail = identity.Email()
                });
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, result.Name, result.Email);
            }

            return new User(result, tenantLookup, roleLookup, userGroupLookup, ValidationProvider);
        }

    }
}
