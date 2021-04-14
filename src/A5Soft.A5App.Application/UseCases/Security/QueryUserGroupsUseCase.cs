using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IQueryUserGroupsUseCase"/>
    [DefaultServiceImplementation(typeof(IQueryUserGroupsUseCase))]
    public class QueryUserGroupsUseCase : QueryListUseCaseBase<UserGroupQueryResult>, IQueryUserGroupsUseCase
    {
        private readonly IUserGroupRepository _repository;


        /// <inheritdoc />
        public QueryUserGroupsUseCase(ClaimsIdentity user, IUseCaseAuthorizer authorizer, 
            IClientDataPortal dataPortal, IValidationEngineProvider validationProvider, 
            IMetadataProvider metadataProvider, ILogger logger, IUserGroupRepository repository) 
            : base(user, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        protected override Task<List<UserGroupQueryResult>> DoQueryAsync(CancellationToken ct)
        {
            return _repository.QueryAsync(ct);
        }
    }
}
