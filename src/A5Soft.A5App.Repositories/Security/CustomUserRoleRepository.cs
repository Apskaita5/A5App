using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Repositories.Security.Maps;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;
using static A5Soft.A5App.Domain.Security.CustomUserRole;
using static A5Soft.A5App.Domain.Security.UserPermission;

namespace A5Soft.A5App.Repositories.Security
{
    /// <summary>
    /// a native repository implementation for <see cref="CustomUserRole"/>
    /// </summary>
    public class CustomUserRoleRepository : ICustomUserRoleRepository
    {
        private readonly IOrmService _ormService;
        private readonly IPluginProvider _pluginProvider;


        public CustomUserRoleRepository(IOrmServiceProvider ormService, IPluginProvider pluginProvider)
        {
            _ormService = ormService?.GetServiceForSecurity() ?? throw new ArgumentNullException(nameof(ormService));
            _pluginProvider = pluginProvider ?? throw new ArgumentNullException(nameof(pluginProvider));
        }


        /// <inheritdoc cref="ICustomUserRoleRepository.FetchAsync"/>
        public async Task<CustomUserRoleDto> FetchAsync(IDomainEntityIdentity userId, 
            IDomainEntityIdentity tenantId, CancellationToken ct = default)
        {
            userId.EnsureValidIdentityFor<User>();
            tenantId.EnsureValidIdentityFor<Tenant>();

            var userTable = await _ormService.Agent.FetchTableAsync("FetchCustomUserRole",
                new SqlParam[]
                {
                    SqlParam.Create("UD", userId.IdentityValue),
                    SqlParam.Create("TD", tenantId.IdentityValue)
                }, ct);
            if (!userTable.Rows.Any() || userTable.Rows[0].IsNull(3)) return null;

            var adminRole = userTable.Rows[0].GetEnum<AdministrativeRole>(2);
            if (adminRole != AdministrativeRole.None) return null; // admins do not have any roles tenants 

            var result = new CustomUserRoleDto()
            {
                Id = new GuidDomainEntityIdentity(Guid.NewGuid(), typeof(CustomUserRole)),
                UserName = userTable.Rows[0].GetStringOrDefault(0),
                UserGroupId = userTable.Rows[0].GetGuidNullable(1).ToEntityIdentity<UserGroup>(),
                UpdatedAt = userTable.Rows[0].GetDateTime(3),
                UpdatedBy = userTable.Rows[0].GetStringOrDefault(4),
                TenantName = userTable.Rows[0].GetStringOrDefault(5),
                TenantId = tenantId,
                UserId = userId
            };

            var currentPermissionsTable = await _ormService.Agent.FetchTableAsync(
                "FetchCustomUserRolePermissions", new SqlParam[]
                {
                    SqlParam.Create("UD", userId.IdentityValue),
                    SqlParam.Create("TD", tenantId.IdentityValue)
                }, ct);
            var currentPermissions = currentPermissionsTable.Rows
                .Select(t => new { Key = t.GetGuid(1), Value = t.GetGuid(0) })
                .ToDictionary(t => t.Key, t => t.Value);

            var allPermissions = new List<Permission>(DefaultPermissions.AllPermissions);
            allPermissions.AddRange(_pluginProvider.GetPermissions());
            result.Permissions = allPermissions.Select(p => new UserPermissionDto(p))
                .OrderBy(p => p.ModuleName)
                .ThenBy(p => p.GroupName)
                .ThenBy(p => p.Order)
                .ToList();

            foreach (var permission in result.Permissions
                .Where(p => currentPermissions.ContainsKey(p.PermissionId)))
            {
                permission.Id = new GuidDomainEntityIdentity(currentPermissions[permission.PermissionId],
                    typeof(UserPermission));
                permission.Assigned = true;
            }

            return result;
        }
                                
        /// <inheritdoc cref="ICustomUserRoleRepository.UpdateAsync"/>
        public async Task<CustomUserRoleDto> UpdateAsync(CustomUserRoleDto dto, string userId)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            await _ormService.Agent.ExecuteInTransactionAsync(async () =>
            {
                foreach (var permission in dto.Permissions
                    .Where(p => p.Id.IsNullOrNew() && p.Assigned))
                {
                    var newPermission = new UserPermissionDb()
                    {
                        PermissionId = permission.PermissionId,
                        UserId = (Guid)dto.UserId.IdentityValue,
                        TenantId = (Guid)dto.TenantId.IdentityValue
                    };
                    await _ormService.ExecuteInsertAsync(newPermission, userId);
                    permission.Id = newPermission.Id;
                }
                foreach (var permission in dto.Permissions
                    .Where(p => !p.Id.IsNullOrNew() && !p.Assigned))
                {
                    await _ormService.ExecuteDeleteAsync<UserPermissionDb>(permission.Id.IdentityValue);
                    permission.Id = new GuidDomainEntityIdentity(typeof(UserPermission));
                }
            });

            return dto;
        }

    }
}
