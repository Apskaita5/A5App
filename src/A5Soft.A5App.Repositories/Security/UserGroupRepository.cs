using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Domain;
using static A5Soft.A5App.Domain.Security.UserGroup;

namespace A5Soft.A5App.Repositories.Security
{
    /// <summary>
    /// a native repository implementation for <see cref="UserGroup"/>
    /// </summary> 
    [DefaultServiceImplementation(typeof(IUserGroupRepository))]
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly ISecurityOrmServiceProvider _ormServiceProvider;


        /// <summary>
        /// creates a new instance of UserGroupRepository.
        /// </summary>
        /// <param name="securityOrmService">an ORM service provider for DI</param>
        public UserGroupRepository(ISecurityOrmServiceProvider securityOrmService)
        {
            _ormServiceProvider = securityOrmService ?? throw new ArgumentNullException(nameof(securityOrmService));
        }


        /// <inheritdoc cref="IUserGroupRepository.FetchAsync"/>
        public async Task<UserGroupDto> FetchAsync(IDomainEntityIdentity id, 
            CancellationToken ct = default)
        {
            id.EnsureValidIdentityFor<UserGroup>();

            return await _ormServiceProvider.GetService().FetchEntityAsync<UserGroupDto>(id.IdentityValue, ct);
        }

        /// <inheritdoc cref="IUserGroupRepository.QueryAsync"/>
        public Task<List<UserGroupQueryResult>> QueryAsync(CancellationToken ct = default)
        {
            return _ormServiceProvider.GetService().QueryAsync<UserGroupQueryResult>(null, ct);
        }

        /// <inheritdoc cref="IUserGroupRepository.FetchLookupAsync"/>
        public Task<List<UserGroupLookup>> FetchLookupAsync(CancellationToken ct = default)
        {
            return _ormServiceProvider.GetService().FetchAllEntitiesAsync<UserGroupLookup>(ct);
        }

        /// <inheritdoc cref="IUserGroupRepository.InsertAsync"/>
        public async Task<UserGroupDto> InsertAsync(UserGroupDto dto, string userId)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            await _ormServiceProvider.GetService().ExecuteInsertAsync(dto, userId);

            return dto;
        }

        /// <inheritdoc cref="IUserGroupRepository.UpdateAsync"/>
        public async Task<UserGroupDto> UpdateAsync(UserGroupDto dto, string userId)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            await _ormServiceProvider.GetService().ExecuteUpdateAsync(dto, userId);

            return dto;
        }

        /// <inheritdoc cref="IUserGroupRepository.DeleteAsync"/>
        public async Task DeleteAsync(IDomainEntityIdentity id)
        {
            id.EnsureValidIdentityFor<UserGroup>();

            await _ormServiceProvider.GetService().ExecuteDeleteAsync<UserGroupDto>(id.IdentityValue);
        }

    }
}
