using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Application;
using A5Soft.CARMA.Application.Authorization;
using A5Soft.CARMA.Application.DataPortal;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Security
{
    /// <inheritdoc cref="IUserGroupLookupService"/>
    [DefaultServiceImplementation(typeof(IUserGroupLookupService))]
    public class UserGroupLookupService : LookupServiceBase<List<UserGroupLookup>>, IUserGroupLookupService
    {
        private readonly IUserGroupRepository _repository;
        private readonly ICacheProvider _cache;


        /// <inheritdoc />
        public UserGroupLookupService(IUserGroupRepository repository, ICacheProvider cache, 
            IAuthorizationProvider authorizationProvider, IClientDataPortal dataPortal,
            ClaimsIdentity userIdentity, ILogger logger) 
            : base(authorizationProvider, dataPortal, userIdentity, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        protected override async Task<List<UserGroupLookup>> DoFetchAsync()
        {
            var result = await _cache.GetOrCreate(async () => await _repository.FetchLookupAsync());
            if (User.IsGroupAdmin())
            {
                var currentGroupId = User.GroupSid().ToEntityIdentity<UserGroup>();
                return result.Where(g => g.Id.IsSameIdentityAs(currentGroupId)).ToList();
            }

            return result;
        }

        protected override List<UserGroupLookup> GetValueFromLocalCache()
        {
            // TODO: implement local cache?
            return null;
        }

        protected override void SetLocalCacheValue(List<UserGroupLookup> value)
        {
            // TODO: implement local cache?
        }
    }
}
