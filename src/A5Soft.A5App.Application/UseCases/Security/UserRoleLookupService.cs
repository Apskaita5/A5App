using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IUserRoleLookupService"/>
    [DefaultServiceImplementation(typeof(IUserRoleLookupService))]
    public class UserRoleLookupService : LookupServiceBase<List<UserRoleLookup>>, IUserRoleLookupService
    {
        private readonly IUserRoleRepository _repository;
        private readonly ICacheProvider _cache;


        /// <inheritdoc />
        public UserRoleLookupService(IUserRoleRepository repository, ICacheProvider cache,
            IAuthorizationProvider authorizationProvider, IClientDataPortal dataPortal,
            ClaimsIdentity userIdentity, ILogger logger)
            : base(authorizationProvider, dataPortal, userIdentity, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        protected override Task<List<UserRoleLookup>> DoFetchAsync()
        {
            return _cache.GetOrCreate(async () => await _repository.FetchLookupAsync());
        }

        protected override List<UserRoleLookup> GetValueFromLocalCache()
        {
            // TODO: implement local cache
            return null;
        }

        protected override void SetLocalCacheValue(List<UserRoleLookup> value)
        {
            // TODO: implement local cache
        }
    }
}
