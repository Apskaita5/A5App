using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Repositories.Security.Maps;
using static A5Soft.A5App.Domain.Security.UserRole;
using static A5Soft.A5App.Domain.Security.UserRolePermission;
using A5Soft.A5App.Application;
using A5Soft.A5App.Application.UseCases.Security;

namespace A5Soft.A5App.Repositories.Security
{
    /// <summary>
    /// a native repository implementation for <see cref="UserRole"/>
    /// </summary>
    [DefaultServiceImplementation(typeof(IUserRoleRepository))]
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly ISecurityOrmServiceProvider _ormServiceProvider;
        private readonly IPluginProvider _pluginProvider;


        public UserRoleRepository(ISecurityOrmServiceProvider securityOrmService, IPluginProvider pluginProvider)
        {
            _ormServiceProvider = securityOrmService ?? throw new ArgumentNullException(nameof(securityOrmService));
            _pluginProvider = pluginProvider ?? throw new ArgumentNullException(nameof(pluginProvider));
        }


        /// <inheritdoc cref="IUserRoleRepository.FetchAsync"/>
        public async Task<UserRoleDto> FetchAsync(IDomainEntityIdentity id, CancellationToken ct = default)
        {
            id.EnsureValidIdentityFor<UserRole>();

            var result = await _ormServiceProvider.GetService().FetchEntityAsync<UserRoleDto>(id.IdentityValue, ct);

            if (result.IsNull()) return result;

            var currentPermissionsTable = await _ormServiceProvider.GetService().Agent.FetchTableAsync(
                "FetchUserRolePermissions", new SqlParam[] 
                    {SqlParam.Create("CD", result.Id.IdentityValue)}, ct);
            var currentPermissions = currentPermissionsTable.Rows
                .Select(t => new { Key = t.GetGuid(1), Value = t.GetGuid(0) })
                .ToDictionary(t => t.Key, t => t.Value);

            var allPermissions = new List<Permission>(DefaultPermissions.AllPermissions);
            allPermissions.AddRange(_pluginProvider.GetPermissions());
            result.Permissions = allPermissions.Select(p => new UserRolePermissionDto(p))
                .OrderBy(p => p.ModuleName)
                .ThenBy(p => p.GroupName)
                .ThenBy(p => p.Order)
                .ToList();

            foreach (var permission in result.Permissions
                .Where(p => currentPermissions.ContainsKey(p.PermissionId)))
            {
                permission.Id = new GuidDomainEntityIdentity(currentPermissions[permission.PermissionId],
                    typeof(UserRolePermission));
                permission.Assigned = true;
            }

            return result;
        }

        /// <inheritdoc cref="IUserRoleRepository.FetchLookupAsync"/>
        public Task<List<UserRoleLookup>> FetchLookupAsync(CancellationToken ct = default)
        {
            return _ormServiceProvider.GetService().FetchAllEntitiesAsync<UserRoleLookup>(ct);
        }

        /// <inheritdoc cref="IUserRoleRepository.QueryAsync"/>
        public Task<List<UserRoleQueryResult>> QueryAsync(CancellationToken ct = default)
        {
            return _ormServiceProvider.GetService().QueryAsync<UserRoleQueryResult>(null, ct);
        }

        /// <inheritdoc cref="IUserRoleRepository.InsertAsync"/>
        public async Task<UserRoleDto> InsertAsync(UserRoleDto dto, string userId)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            await _ormServiceProvider.GetService().Agent.ExecuteInTransactionAsync(async () =>
            {
                await _ormServiceProvider.GetService().ExecuteInsertAsync(dto, userId);
                await InsertNewPermissionsAsync(dto.Permissions, dto.Id, userId);
            });

            return dto;
        }

        /// <inheritdoc cref="IUserRoleRepository.UpdateAsync"/>
        public async Task<UserRoleDto> UpdateAsync(UserRoleDto dto, string userId)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            await _ormServiceProvider.GetService().Agent.ExecuteInTransactionAsync(async () =>
            {
                await _ormServiceProvider.GetService().ExecuteUpdateAsync(dto, userId);
                await InsertNewPermissionsAsync(dto.Permissions, dto.Id, userId);
                foreach (var permission in dto.Permissions
                    .Where(p => !p.Id.IsNullOrNew() && !p.Assigned))
                {
                    await _ormServiceProvider.GetService().ExecuteDeleteAsync<UserRolePermissionDb>(permission.Id.IdentityValue);
                    permission.Id = new GuidDomainEntityIdentity(typeof(UserRolePermission));
                }
            });

            return dto;
        }

        /// <inheritdoc cref="IUserRoleRepository.DeleteAsync"/>
        public async Task DeleteAsync(IDomainEntityIdentity id)
        {
            id.EnsureValidIdentityFor<UserRole>();

            await _ormServiceProvider.GetService().ExecuteDeleteAsync<UserRoleDto>(id.IdentityValue);
        }


        private async Task InsertNewPermissionsAsync(List<UserRolePermissionDto> permissions,
            IDomainEntityIdentity roleId, string userId)
        {
            foreach (var permission in permissions
                .Where(p => (p.Id?.IsNew ?? true) && p.Assigned))
            {
                var newPermission = new UserRolePermissionDb()
                {
                    PermissionId = permission.PermissionId,
                    RoleId = roleId
                };
                await _ormServiceProvider.GetService().ExecuteInsertAsync(newPermission, userId);
                permission.Id = newPermission.Id;
            }
        }

    }
}
