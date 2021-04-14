using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core.MicroOrm;
using System;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserRolePermissionDb 
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentGuid<UserRolePermissionDb> _identityMap =
            new IdentityMapParentGuid<UserRolePermissionDb>("role_permissions", "id",
                nameof(Id), () => new UserRolePermissionDb(), 
                (c) => (Guid)c.Id.IdentityValue,
                (c, v) => c.Id = v.ToIdentity<UserRolePermission>());
        private static readonly FieldMapGuid<UserRolePermissionDb> _roleIdMap = 
            new FieldMapGuid<UserRolePermissionDb>("role_id", nameof(RoleId), 
                (c, v) => c.RoleId = v.ToIdentity<UserRole>(),
                (c) => (Guid)c.RoleId.IdentityValue, FieldPersistenceType.Insert);
        private static readonly FieldMapGuid<UserRolePermissionDb> _permissionIdMap = 
            new FieldMapGuid<UserRolePermissionDb>("permission_id", nameof(PermissionId), 
                (c, v) => c.PermissionId = v,
                (c) => c.PermissionId, FieldPersistenceType.Insert);
#pragma warning restore IDE0052 // Remove unread private members

        public IDomainEntityIdentity Id { get; set; }

        public IDomainEntityIdentity RoleId { get; set; }

        public Guid PermissionId { get; set; }
    }
}
