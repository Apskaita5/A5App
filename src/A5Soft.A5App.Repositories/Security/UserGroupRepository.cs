using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;
using static A5Soft.A5App.Domain.Security.UserGroup;

namespace A5Soft.A5App.Repositories.Security
{
    /// <summary>
    /// a native repository implementation for <see cref="UserGroup"/>
    /// </summary>
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly IOrmService _ormService;


        /// <summary>
        /// creates a new instance of UserGroupRepository.
        /// </summary>
        /// <param name="ormService">an ORM service provider for DI</param>
        public UserGroupRepository(IOrmServiceProvider ormService)
        {
            _ormService = ormService?.GetServiceForSecurity() ?? 
                throw new ArgumentNullException(nameof(ormService));
        }


        /// <inheritdoc cref="IUserGroupRepository.FetchAsync"/>
        public async Task<UserGroupDto> FetchAsync(IDomainEntityIdentity id, 
            CancellationToken ct = default)
        {
            id.EnsureValidIdentityFor<UserGroup>();

            return await _ormService.FetchEntityAsync<UserGroupDto>(id.IdentityValue, ct);
        }

        /// <inheritdoc cref="IUserGroupRepository.QueryAsync"/>
        public Task<List<UserGroupQueryResult>> QueryAsync(CancellationToken ct = default)
        {
            return _ormService.QueryAsync<UserGroupQueryResult>(null, ct);
        }

        /// <inheritdoc cref="IUserGroupRepository.FetchLookupAsync"/>
        public Task<List<UserGroupLookup>> FetchLookupAsync(CancellationToken ct = default)
        {
            return _ormService.FetchAllEntitiesAsync<UserGroupLookup>(ct);
        }

        /// <inheritdoc cref="IUserGroupRepository.InsertAsync"/>
        public async Task<UserGroupDto> InsertAsync(UserGroupDto dto, string userId)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            await _ormService.ExecuteInsertAsync(dto, userId);

            return dto;
        }

        /// <inheritdoc cref="IUserGroupRepository.UpdateAsync"/>
        public async Task<UserGroupDto> UpdateAsync(UserGroupDto dto, string userId)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            await _ormService.ExecuteUpdateAsync(dto, userId);

            return dto;
        }

        /// <inheritdoc cref="IUserGroupRepository.DeleteAsync"/>
        public async Task DeleteAsync(IDomainEntityIdentity id)
        {
            id.EnsureValidIdentityFor<UserGroup>();

            await _ormService.ExecuteDeleteAsync<UserGroupDto>(id.IdentityValue);
        }

    }
}
