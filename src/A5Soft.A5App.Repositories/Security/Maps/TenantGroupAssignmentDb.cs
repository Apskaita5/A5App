using A5Soft.A5App.Domain.Security;
using A5Soft.DAL.Core.MicroOrm;
using System;
using A5Soft.A5App.Application;
using static A5Soft.A5App.Domain.Security.TenantGroupAssignment;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class TenantGroupAssignmentDb : TenantGroupAssignmentDto
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapChildGuid<TenantGroupAssignmentDto> _identityMap =
            new IdentityMapChildGuid<TenantGroupAssignmentDto>("user_group_tenants", 
            "id", nameof(Id), "tenant_id", 
            () => new TenantGroupAssignmentDb(), 
            (c) => (Guid?)c.Id.IdentityValue,
            (c, v) => c.Id = v.ToIdentity<TenantGroupAssignment>());
        private static readonly FieldMapGuid<TenantGroupAssignmentDto> _userGroupIdMap = 
            new FieldMapGuid<TenantGroupAssignmentDto>("user_group_id", nameof(GroupId), 
            (c, v) => c.GroupId = v.ToIdentity<UserGroup>(),
            (c) => (Guid)c.GroupId.IdentityValue, 
            FieldPersistenceType.Insert | FieldPersistenceType.Read);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
