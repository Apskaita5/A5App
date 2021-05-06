using A5Soft.CARMA.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Application.UseCases.Security.ClaimsIdentityExtensions;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IValidateUserIdentityUseCase"/>
    [DefaultServiceImplementation(typeof(IValidateUserIdentityUseCase))]
    public class ValidateUserIdentityUseCase : UnauthenticatedCommandWithParameterUseCaseBase<ClaimsIdentity>, IValidateUserIdentityUseCase
    {
        private readonly IUserRepository _repository;
        private readonly ISecurityTokenProvider _securityTokenProvider;
        private readonly ICacheProvider _cache;


        /// <inheritdoc />
        public ValidateUserIdentityUseCase(IClientDataPortal dataPortal, IValidationEngineProvider validationProvider,
            IMetadataProvider metadataProvider, ILogger logger, IUserRepository repository,
            ISecurityTokenProvider securityTokenProvider, ICacheProvider cache) : base(dataPortal, validationProvider,
            metadataProvider, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _securityTokenProvider =
                securityTokenProvider ?? throw new ArgumentNullException(nameof(securityTokenProvider));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        protected override async Task ExecuteAsync(ClaimsIdentity identity)
        {
            if (! await _securityTokenProvider.IsValidAsync(identity))
            {
                Logger.LogSecurityIssue("Attempt to forge identity.", identity);
                throw new UnauthenticatedException();
            }

            if (identity.Expiration() < DateTime.UtcNow) throw new UnauthenticatedException();

            var users = await _cache.GetOrCreate<List<UserLookup>>(
                async () => await _repository.FetchLookupAsync());

            var currentUser = users.FirstOrDefault(u => (Guid)u.Id.IdentityValue == identity.Sid());

            if (null == currentUser)
            {
                Logger.LogSecurityIssue("Attempt to use identity of a deleted user.", identity);
                throw new UnauthenticatedException();
            }

            if (currentUser.OccHash != identity.OccHash()) throw new UnauthenticatedException();
        }
    }
}
