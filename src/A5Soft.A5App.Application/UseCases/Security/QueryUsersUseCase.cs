using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IQueryUsersUseCase"/>
    [DefaultServiceImplementation(typeof(IQueryUsersUseCase))]
    public class QueryUsersUseCase : QueryListUseCaseBase<UserQueryResult>, IQueryUsersUseCase
    {
        private readonly IUserRepository _repository;


        /// <inheritdoc />
        public QueryUsersUseCase(ClaimsIdentity user, IUseCaseAuthorizer authorizer,
            IClientDataPortal dataPortal, IValidationEngineProvider validationProvider,
            IMetadataProvider metadataProvider, ILogger logger, IUserRepository repository)
            : base(user, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override async Task<List<UserQueryResult>> DoQueryAsync(CancellationToken ct)
        {
            var result = await _repository.QueryAsync(ct);
            var currentUserId = User.Sid().ToEntityIdentity<User>();
            if (User.IsGroupAdmin())
            {
                var currentUserGroupId = User.GroupSid().ToEntityIdentity<UserGroup>();
                return result.Where(u => currentUserGroupId.IsSameIdentityAs(u.GroupId)
                    && !currentUserId.IsSameIdentityAs(u.Id)).ToList();
            }
            return result.Where(u => !currentUserId.IsSameIdentityAs(u.Id)).ToList();
        }
    }
}
