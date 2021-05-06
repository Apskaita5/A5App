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

namespace A5Soft.A5App.Application.UseCases.Security.UserRoles
{
    /// <inheritdoc cref="IUpdateCustomUserRoleUseCase"/>
    [DefaultServiceImplementation(typeof(IUpdateCustomUserRoleUseCase))]
    public class UpdateCustomUserRoleUseCase : SaveUseCaseBase<CustomUserRole, ICustomUserRole>, IUpdateCustomUserRoleUseCase
    {
        private readonly ICacheProvider _cache;
        private readonly ICustomUserRoleRepository _repository;


        /// <inheritdoc />
        public UpdateCustomUserRoleUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, ICacheProvider cache, ICustomUserRoleRepository repository) 
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override async Task<CustomUserRole> DoSaveAsync(ICustomUserRole domainDto)
        {
            if (domainDto.UserId.IsNullOrNew() || domainDto.TenantId.IsNullOrNew()) 
                throw new ValidationException(new BrokenRulesCollection(
                    string.Format(Resources.No_Id_For_Updated_Entity,
                            MetadataProvider.GetEntityMetadata<CustomUserRole>().GetDisplayNameForOld())));

            var identity = await GetIdentityAsync();

            if (domainDto.UserId.IsSameIdentityAs(identity.Sid().ToIdentity<User>()))
            {
                Logger.LogSecurityIssue($"User {identity.Name} attempted to set a custom role for self.");
                throw new AuthorizationException(Resources.Security_UpdateCustomUserRoleUseCase_Cannot_Set_For_Self);
            }

            var currentDto = await _repository.FetchAsync(domainDto.UserId, domainDto.TenantId);

            if (null == currentDto) throw new NotFoundException(typeof(CustomUserRole),
                domainDto.UserId + ";" + domainDto.TenantId, MetadataProvider);

            var current = new CustomUserRole(currentDto, ValidationProvider);

            if (identity.IsGroupAdmin())
            {
                if (!identity.GroupSid().ToIdentity<UserGroup>().IsSameIdentityAs(current.UserGroupId))
                {
                    Logger.LogSecurityIssue($"User {identity.Name} attempted to set custom role for the user " +
                        $"outside of his group - {current.UserName}.");
                    throw new AuthorizationException(Resources.Security_UpdateCustomUserRoleUseCase_Cannot_Set_For_Other_UserGroups);
                }
            }

            current.Merge(domainDto, true);

            var result = await _repository.UpdateAsync(current.ToDto(), identity.Email());

            _cache.Clear<List<UserLookup>>();

            return new CustomUserRole(result, ValidationProvider);
        }

    }
}
