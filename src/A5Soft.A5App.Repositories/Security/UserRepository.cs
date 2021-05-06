using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application;
using A5Soft.A5App.Domain.Security.Lookups;
using static A5Soft.A5App.Domain.Security.User;
using static A5Soft.A5App.Domain.Security.UserTenant;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.A5App.Application.Repositories.Security;
using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.A5App.Repositories.Security.Maps;

namespace A5Soft.A5App.Repositories.Security
{
    /// <summary>
    /// a native repository implementation for <see cref="User"/>
    /// </summary> 
    [DefaultServiceImplementation(typeof(IUserRepository))]
    public class UserRepository : IUserRepository
    {
        private readonly ISecurityOrmServiceProvider _ormServiceProvider;
       

        public UserRepository(ISecurityOrmServiceProvider securityOrmService)
        {
            _ormServiceProvider = securityOrmService ?? throw new ArgumentNullException(nameof(securityOrmService));
        }


        /// <inheritdoc cref="IUserRepository.FetchAsync"/>
        public async Task<UserDto> FetchAsync(IDomainEntityIdentity id, IEnumerable<TenantLookup> tenants, 
            CancellationToken ct = default)
        {
            id.EnsureValidIdentityFor<User>();

            var result = await _ormServiceProvider.GetService().FetchEntityAsync<UserDto>(id.IdentityValue, ct);

            if (result.IsNull()) return result;

            result.RolesForTenants = await _ormServiceProvider.GetService().FetchChildEntitiesAsync<UserTenantDto>(
                result.Id.IdentityValue, ct);

            foreach (var tenant in tenants)
            {
                if (!result.RolesForTenants.Any(t => t.TenantId.IsSameIdentityAs(tenant.Id)))
                {
                    result.RolesForTenants.Add(new UserTenantDto() { TenantId = tenant.Id });
                }
            }

            return result;
        }

        /// <inheritdoc cref="IUserRepository.FetchLookupAsync"/>
        public Task<List<UserLookup>> FetchLookupAsync(CancellationToken ct = default)
        {
            return _ormServiceProvider.GetService().FetchAllEntitiesAsync<UserLookup>(ct);
        }

        /// <inheritdoc cref="IUserRepository.QueryAsync"/>
        public Task<List<UserQueryResult>> QueryAsync(CancellationToken ct = default)
        {
            return _ormServiceProvider.GetService().QueryAsync<UserQueryResult>(null, ct);
        }

        /// <inheritdoc cref="IUserRepository.InsertAsync"/>
        public async Task<UserDto> InsertAsync(UserDto dto, string userId)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            await _ormServiceProvider.GetService().Agent.ExecuteInTransactionAsync(async () =>
            {
                await _ormServiceProvider.GetService().ExecuteInsertAsync(dto, userId);
                foreach (var role in dto.RolesForTenants
                    .Where(p => p.Id.IsNullOrNew() && !p.RoleId.IsNullOrNew()))
                {
                    await _ormServiceProvider.GetService().ExecuteInsertChildAsync(role, dto.Id.IdentityValue);
                }
            });

            return dto;
        }

        /// <inheritdoc cref="IUserRepository.UpdateAsync"/>
        public async Task<UserDto> UpdateAsync(UserDto dto, string userId)
        {
            if (null == dto) throw new ArgumentNullException(nameof(dto));

            await _ormServiceProvider.GetService().Agent.ExecuteInTransactionAsync(async () =>
            {
                await _ormServiceProvider.GetService().ExecuteUpdateAsync(dto, userId);
                if (dto.PromotedToAdminRole)
                {
                    await _ormServiceProvider.GetService().Agent.ExecuteCommandAsync("DeleteCustomUserRoles",
                        new SqlParam[] { SqlParam.Create("UD", dto.Id.IdentityValue) });
                    await _ormServiceProvider.GetService().Agent.ExecuteCommandAsync("DeleteUserRoles",
                        new SqlParam[] { SqlParam.Create("UD", dto.Id.IdentityValue) });
                }
                if (dto.AdminRole == AdministrativeRole.None)
                {
                    foreach (var roleForTenant in dto.RolesForTenants
                        .Where(p => (p.Id?.IsNew ?? true) && !(p.RoleId?.IsNew ?? true)))
                    {
                        if (roleForTenant.Id.IsNullOrNew() && !roleForTenant.RoleId.IsNullOrNew())
                        {
                            await _ormServiceProvider.GetService().ExecuteInsertChildAsync(roleForTenant, dto.Id.IdentityValue);
                        }
                        else if (!roleForTenant.Id.IsNullOrNew() && roleForTenant.RoleId.IsNullOrNew())
                        {
                            await _ormServiceProvider.GetService().ExecuteDeleteAsync<UserTenantDto>(roleForTenant.Id.IdentityValue);
                        }
                        else if (!roleForTenant.Id.IsNullOrNew() && !roleForTenant.RoleId.IsNullOrNew())
                        {
                            await _ormServiceProvider.GetService().ExecuteUpdateAsync(roleForTenant);
                            await DeleteCustomUserRoleAsync(dto.Id, roleForTenant.TenantId);
                        }
                        else if (roleForTenant.Id.IsNullOrNew() && roleForTenant.RoleId.IsNullOrNew() 
                            && roleForTenant.HadCustomRole)
                        {
                            await DeleteCustomUserRoleAsync(dto.Id, roleForTenant.TenantId);
                        }
                    }
                }
            });

            return dto;
        }

        /// <inheritdoc cref="IUserRepository.DeleteAsync"/>
        public async Task DeleteAsync(IDomainEntityIdentity id)
        {
            id.EnsureValidIdentityFor<User>();

            await _ormServiceProvider.GetService().ExecuteDeleteAsync<UserDto>(id.IdentityValue);
        }

        /// <inheritdoc cref="IUserRepository.UpdatePasswordAsync"/>
        public async Task UpdatePasswordAsync(IDomainEntityIdentity id, string hashedPassword)
        {
            id.EnsureValidIdentityFor<User>();

            await _ormServiceProvider.GetService().Agent.ExecuteCommandAsync("UpdateUserPassword", 
                new SqlParam[]
                {
                    SqlParam.Create("UD", id.IdentityValue), 
                    SqlParam.Create("PS", hashedPassword) 
                });
        }

        /// <inheritdoc cref="IUserRepository.FetchLoginInfoAsync"/>
        public async Task<(Guid? Id, string PasswordHash)> FetchLoginInfoAsync(string userEmail)
        {
            if (userEmail.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(userEmail));

            var result = await _ormServiceProvider.GetService().Agent.FetchTableAsync("FetchLoginCredentials",
                new SqlParam[] {SqlParam.Create("CD", userEmail.Trim())});

            if (result.Rows.Count < 1) return (Id: null, PasswordHash: string.Empty);

            return (Id: result.Rows[0].GetGuid(0), PasswordHash: result.Rows[0].GetString(1));
        }

        /// <inheritdoc cref="IUserRepository.FetchUserIdentityAsync"/>
        public async Task<UserIdentity> FetchUserIdentityAsync(Guid userId, Guid? tenantId, 
            CancellationToken ct = default)
        {
            var resultList = await _ormServiceProvider.GetService().QueryAsync<UserIdentityDb>(
                new SqlParam[]{ SqlParam.Create("CD", userId) }, ct);
            
            if (null == resultList || !resultList.Any()) return null;

            var result = resultList[0];

            result.SetTenant(tenantId);

            if (!tenantId.HasValue || result.AdminRole != AdministrativeRole.None)
            {
                result.SetPermissions(new List<Guid>());
                return result;
            }

            var reader = await _ormServiceProvider.GetService().Agent.GetReaderAsync(
                "FetchUserPermissionsForTenant", new SqlParam[]
                {
                    SqlParam.Create("UD", userId),
                    SqlParam.Create("TD", tenantId)
                }, ct);

            var permissions = new List<Guid>();

            try
            {
                while (await reader.ReadAsync(ct).ConfigureAwait(false))
                {
                    permissions.Add(reader.GetGuid(0));
                }
            }
            finally
            {
                await reader.CloseAsync();
            }

            result.SetPermissions(permissions);

            return result;
        }

        /// <inheritdoc cref="IUserRepository.CountUsersInGroupAsync"/>
        public async Task<int> CountUsersInGroupAsync(IDomainEntityIdentity groupId)
        {
            groupId.EnsureValidIdentityFor<UserGroup>();

            var result = await _ormServiceProvider.GetService().Agent.FetchTableAsync("FetchUserCountInGroup",
                new SqlParam[] { SqlParam.Create("CD", groupId.IdentityValue) });

            if (!result.Rows.Any()) return 0;

            return result.Rows[0].GetInt32OrDefault(0, 0);
        }


        private async Task DeleteCustomUserRoleAsync(IDomainEntityIdentity userId, IDomainEntityIdentity tenantId)
        {
            await _ormServiceProvider.GetService().Agent.ExecuteCommandAsync("DeleteCustomUserRole",
                new SqlParam[]
                {
                    SqlParam.Create("UD", userId.IdentityValue),
                    SqlParam.Create("TD", tenantId.IdentityValue)
                });
        }
          
    }
}
