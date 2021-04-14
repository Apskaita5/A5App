using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core.MicroOrm;
using System;
using A5Soft.A5App.Domain.Security;
using static A5Soft.A5App.Domain.Security.UserTenant;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserTenantDb : UserTenantDto
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapChildGuid<UserTenantDto> _identityMap =
            new IdentityMapChildGuid<UserTenantDto>("user_roles", "id",
            nameof(Id), "user_id", () => new UserTenantDb(), 
            (c) => (Guid?)c.Id.IdentityValue,
            (c, v) => c.Id = v.ToIdentity<UserTenant>(), 
            fetchByParentIdQueryToken: "FetchUserRolesForTenants");
        private static readonly FieldMapNullableGuid<UserTenantDto> _roleIdMap = 
            new FieldMapNullableGuid<UserTenantDto>("role_id", nameof(RoleId), 
            (c, v) => c.RoleId = v.ToIdentity<UserRole>(),
            (c) => (Guid?)c.RoleId.IdentityValue, FieldPersistenceType.CRUD);
        private static readonly FieldMapGuid<UserTenantDto> _tenantIdMap = 
            new FieldMapGuid<UserTenantDto>("tenant_id", nameof(TenantId), 
            (c, v) => c.TenantId = v.ToIdentity<Tenant>(),
            (c) => (Guid)c.TenantId.IdentityValue, FieldPersistenceType.Insert
            | FieldPersistenceType.Read);
        private static readonly FieldMapBool<UserTenantDto> _isCustomRoleMap =
            new FieldMapBool<UserTenantDto>(nameof(IsCustomRole),
            (c, v) => c.IsCustomRole = v);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
