using A5Soft.CARMA.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IValidateUserIdentityUseCase"/>
    [DefaultServiceImplementation(typeof(IValidateUserIdentityUseCase))]
    public class ValidateUserIdentityUseCase : CommandUseCaseBase, IValidateUserIdentityUseCase
    {
        private readonly IUserRepository _repository;
        private readonly ICacheProvider _cache;
        private readonly ISecurityPolicy _policy;


        /// <inheritdoc />
        public ValidateUserIdentityUseCase(ClaimsIdentity user, IUseCaseAuthorizer authorizer,
            IClientDataPortal dataPortal, IValidationEngineProvider validationProvider,
            IMetadataProvider metadataProvider, ILogger logger, IUserRepository repository, ICacheProvider cache,
            ISecurityPolicy policy) : base(user, authorizer, dataPortal, validationProvider, metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }


        protected override async Task ExecuteAsync()
        {
            if (!User.IsValid(_policy.ServerSecret))
            {
                Logger.LogSecurityIssue("Attempt to forge identity.", User);
                throw new UnauthenticatedException();
            }

            if (User.Expiration() < DateTime.UtcNow) throw new UnauthenticatedException();

            var users = await _cache.GetOrCreate<List<UserLookup>>(
                async () => await _repository.FetchLookupAsync());

            var currentUser = users.FirstOrDefault(u => u.Id == User.Sid());

            if (null == currentUser)
            {
                Logger.LogSecurityIssue("Attempt to use identity of a deleted user.", User);
                throw new UnauthenticatedException();
            }

            if (currentUser.OccHash != User.OccHash()) throw new UnauthenticatedException();
        }
    }
}
