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

namespace A5Soft.A5App.Application.UseCases.Security.Users
{
    /// <inheritdoc cref="IQueryUsersUseCase"/>
    [DefaultServiceImplementation(typeof(IQueryUsersUseCase))]
    public class QueryUsersUseCase : QueryListUseCaseBase<UserQueryResult>, IQueryUsersUseCase
    {
        private readonly IUserRepository _repository;


        /// <inheritdoc />
        public QueryUsersUseCase(IAuthenticationStateProvider authenticationStateProvider, 
            IAuthorizationProvider authorizer, IClientDataPortal dataPortal, 
            IValidationEngineProvider validationProvider, IMetadataProvider metadataProvider, 
            ILogger logger, IUserRepository repository)
            : base(authenticationStateProvider, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override async Task<List<UserQueryResult>> DoQueryAsync(CancellationToken ct)
        {
            var result = await _repository.QueryAsync(ct);
            var identity = await GetIdentityAsync();
            var currentUserId = identity.Sid().ToIdentity<User>();
            if (identity.IsGroupAdmin())
            {
                var currentUserGroupId = identity.GroupSid().ToIdentity<UserGroup>();
                return result.Where(u => currentUserGroupId.IsSameIdentityAs(u.GroupId)
                    && !currentUserId.IsSameIdentityAs(u.Id)).ToList();
            }
            return result.Where(u => !currentUserId.IsSameIdentityAs(u.Id)).ToList();
        }
    }
}
