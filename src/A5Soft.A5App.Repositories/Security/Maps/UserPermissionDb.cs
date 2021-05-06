using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core.MicroOrm;
using System;
using A5Soft.A5App.Application;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserPermissionDb
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentGuid<UserPermissionDb> _identityMap =
            new IdentityMapParentGuid<UserPermissionDb>("role_permissions", "id",
                nameof(Id), () => new UserPermissionDb(),
                (c) => (Guid?)c.Id.IdentityValue,
                (c, v) => c.Id = v.ToIdentity<UserPermission>());
        private static readonly FieldMapGuid<UserPermissionDb> _userIdMap =
            new FieldMapGuid<UserPermissionDb>("user_id", nameof(UserId),
                (c, v) => c.UserId = v,
                (c) => c.UserId, FieldPersistenceType.Insert);
        private static readonly FieldMapGuid<UserPermissionDb> _tenantIdMap =
            new FieldMapGuid<UserPermissionDb>("tenant_id", nameof(TenantId),
                (c, v) => c.TenantId = v,
                (c) => c.TenantId, FieldPersistenceType.Insert);
        private static readonly FieldMapGuid<UserPermissionDb> _permissionIdMap =
            new FieldMapGuid<UserPermissionDb>("permission_id", nameof(PermissionId),
                (c, v) => c.PermissionId = v,
                (c) => c.PermissionId, FieldPersistenceType.Insert);
#pragma warning restore IDE0052 // Remove unread private members

        public IDomainEntityIdentity Id { get; set; }

        public Guid UserId { get; set; }

        public Guid TenantId { get; set; }

        public Guid PermissionId { get; set; }
    }
}
