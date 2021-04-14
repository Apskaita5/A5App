using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IFetchCustomUserRoleUseCase"/>
    [DefaultServiceImplementation(typeof(IFetchCustomUserRoleUseCase))]
    public class FetchCustomUserRoleUseCase : FetchDomainEntityUseCaseBase<CustomUserRole>, IFetchCustomUserRoleUseCase
    {
        private readonly ICustomUserRoleRepository _repository;


        /// <inheritdoc />
        public FetchCustomUserRoleUseCase(ClaimsIdentity user, IUseCaseAuthorizer authorizer,
            IClientDataPortal dataPortal, IValidationEngineProvider validationProvider,
            IMetadataProvider metadataProvider, ILogger logger, ICustomUserRoleRepository repository)
            : base(user, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override async Task<CustomUserRole> DoFetchAsync(IDomainEntityIdentity id, CancellationToken ct)
        {
            if (!(id is CustomUserRoleIdentity rid)) throw new InvalidOperationException(
                $"Identity {id} is not assignable to CustomUserRoleIdentity.");

            if (rid.UserId.IsSameIdentityAs(User.Sid().ToEntityIdentity<User>()))
            {
                Logger.LogSecurityIssue($"User {User.Name} attempted to fetch a custom role for self.");
                throw new AuthorizationException(Resources.Security_FetchCustomUserRoleUseCase_No_Self_Fetch);
            }

            var dto = await _repository.FetchAsync(rid.UserId, rid.TenantId, ct);

            if (null == dto) throw new NotFoundException(typeof(CustomUserRole),
                id.ToString(), MetadataProvider);

            if (User.IsGroupAdmin())
            {
                if (!User.GroupSid().ToEntityIdentity<UserGroup>().IsSameIdentityAs(dto.UserGroupId))
                {
                    Logger.LogSecurityIssue($"User {User.Name} attempted to fetch a custom role for the user " +
                        $"outside of his group - {dto.UserName}.");
                    throw new AuthorizationException(Resources.Security_FetchCustomUserRoleUseCase_No_Fetch_For_Other_UserGroups);
                }
            }

            return new CustomUserRole(dto, ValidationProvider);
        }

    }
}
