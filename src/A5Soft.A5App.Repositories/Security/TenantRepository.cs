using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application;
using A5Soft.A5App.Repositories.Security.Maps;
using static A5Soft.A5App.Domain.Security.Tenant;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.A5App.Application.Repositories.Security;
using static A5Soft.A5App.Domain.Security.TenantGroupAssignment;
using static A5Soft.A5App.Domain.Security.TenantGroupAssignments;

namespace A5Soft.A5App.Repositories.Security
{
    /// <summary>
    /// a native repository implementation for <see cref="Tenant"/>
    /// </summary> 
    [DefaultServiceImplementation(typeof(ITenantRepository))]
    public class TenantRepository : ITenantRepository
    {
        private readonly ISecurityOrmServiceProvider _ormServiceProvider;


        /// <summary>
        /// creates a new instance of TenantRepository.
        /// </summary>
        /// <param name="securityOrmService">an ORM service provider for DI</param>
        public TenantRepository(ISecurityOrmServiceProvider securityOrmService)
        {
            _ormServiceProvider = securityOrmService ?? throw new ArgumentNullException(nameof(securityOrmService));
        }


        /// <inheritdoc cref="ITenantRepository.FetchAsync"/>
        public async Task<TenantDto> FetchAsync(IDomainEntityIdentity id,
            CancellationToken ct = default)
        {
            id.EnsureValidIdentityFor<Tenant>();

            return await _ormServiceProvider.GetService().FetchEntityAsync<TenantDto>(id.IdentityValue, ct);
        }

        /// <inheritdoc cref="ITenantRepository.FetchAssignmentsAsync"/>
        public async Task<TenantGroupAssignmentsDto> FetchAssignmentsAsync(IDomainEntityIdentity id,
            CancellationToken ct = default)
        {
            id.EnsureValidIdentityFor<Tenant>();

            return await _ormServiceProvider.GetService().FetchEntityAsync<TenantGroupAssignmentsDto>(id.IdentityValue, ct);
        }

        /// <inheritdoc cref="ITenantRepository.FetchLookupAsync"/>
        public Task<List<TenantLookup>> FetchLookupAsync(CancellationToken ct = default)
        {
            return _ormServiceProvider.GetService().FetchAllEntitiesAsync<TenantLookup>(ct);
        }

        /// <inheritdoc cref="ITenantRepository.FetchLookupForUserAsync"/>
        public async Task<List<TenantLookup>> FetchLookupForUserAsync(IDomainEntityIdentity userId, 
            CancellationToken ct = default)
        {
            var reader = await _ormServiceProvider.GetService().Agent.GetReaderAsync("FetchTenantLookupForUser", 
                new SqlParam[] {SqlParam.Create("UD", userId.IdentityValue) }, ct)
                .ConfigureAwait(false);

            var result = new List<TenantLookup>();

            try
            {
                while (await reader.ReadAsync(ct).ConfigureAwait(false))
                {
                    TenantLookup newItem = new TenantLookupDb();
                    _ormServiceProvider.GetService().LoadObjectFields(newItem, reader);
                    result.Add(newItem);
                }
            }
            finally
            {
                await reader.CloseAsync();
            }

            return result;
        }

        /// <inheritdoc cref="ITenantRepository.QueryAsync"/>
        public Task<List<TenantQueryResult>> QueryAsync(CancellationToken ct = default)
        {
            return _ormServiceProvider.GetService().QueryAsync<TenantQueryResult>(null, ct);
        }

        /// <inheritdoc cref="ITenantRepository.InsertAsync"/>
        public async Task<TenantDto> InsertAsync(TenantDto dto, string userId)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            await _ormServiceProvider.GetService().ExecuteInsertAsync(dto, userId);

            return dto;
        }

        /// <inheritdoc cref="ITenantRepository.UpdateAsync"/>
        public async Task<TenantDto> UpdateAsync(TenantDto dto)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            await _ormServiceProvider.GetService().ExecuteUpdateAsync(dto);

            return dto;
        }

        /// <inheritdoc cref="ITenantRepository.UpdateAssignmentsAsync"/>
        public async Task<TenantGroupAssignmentsDto> UpdateAssignmentsAsync(TenantGroupAssignmentsDto dto)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            foreach (var deletedAssignment in dto.DeletedAssignments.Where(a => !a.Id.IsNullOrNew()))
            {
                await _ormServiceProvider.GetService().ExecuteDeleteAsync(deletedAssignment);
            }
            foreach (var newAssignment in dto.Assignments.Where(a => a.Id.IsNullOrNew()))
            {
                await _ormServiceProvider.GetService().ExecuteInsertChildAsync(newAssignment, dto.Id.IdentityValue);
            }

            return dto;
        }

        /// <inheritdoc cref="ITenantRepository.DeleteAsync"/>
        public async Task DeleteAsync(IDomainEntityIdentity id)
        {
            id.EnsureValidIdentityFor<Tenant>();

            await _ormServiceProvider.GetService().ExecuteDeleteAsync<TenantDto>(id.IdentityValue);
        }

        /// <inheritdoc cref="ITenantRepository.AssignToUserGroupAsync"/>
        public async Task AssignToUserGroupAsync(IDomainEntityIdentity id,
            IDomainEntityIdentity groupId)
        {
            id.EnsureValidIdentityFor<Tenant>();
            groupId.EnsureValidIdentityFor<UserGroup>();

            var current = await _ormServiceProvider.GetService()
                .FetchChildEntitiesAsync<TenantGroupAssignmentDto>(id.IdentityValue);
            if (!current.Any(c => c.GroupId.IsSameIdentityAs(groupId)))
            {
                await _ormServiceProvider.GetService().ExecuteInsertChildAsync<TenantGroupAssignmentDto>(
                    new TenantGroupAssignmentDb() { GroupId = groupId }, id);
            }
        }

        /// <inheritdoc cref="ITenantRepository.CanAssignToUserGroup"/>
        public async Task<bool> CanAssignToUserGroup(IDomainEntityIdentity groupId)
        {
            groupId.EnsureValidIdentityFor<UserGroup>();

            var result = await _ormServiceProvider.GetService().Agent.FetchTableAsync("FetchCanAssignToUserGroup",
                new SqlParam[] { SqlParam.Create("CD", groupId.IdentityValue) });
            
            if (result.Rows.Count < 1) throw new InvalidOperationException(
                $"No such user group (id = {groupId}).");

            return (result.Rows[0].GetInt32(0) > result.Rows[0].GetInt32(1));
        }

    }
}
