using A5Soft.A5App.Domain.Security;
using A5Soft.DAL.Core.MicroOrm;
using System;
using static A5Soft.A5App.Domain.Security.TenantGroupAssignment;
using static A5Soft.A5App.Domain.Security.TenantGroupAssignments;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class TenantGroupAssignmentsDb : TenantGroupAssignmentsDto
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentGuid<TenantGroupAssignmentsDto> _identityMap =
            new IdentityMapParentGuid<TenantGroupAssignmentsDto>("tenants", "id",
                nameof(Id), () => new TenantGroupAssignmentsDb(),
                (c) => (Guid?)c.Id.IdentityValue,
                (c, v) => c.Id = v.ToIdentity<TenantGroupAssignments>());
        private static readonly FieldMapString<TenantGroupAssignmentsDto> _tenantNameMap =
            new FieldMapString<TenantGroupAssignmentsDto>("tenant_name", nameof(TenantName),
                (c, v) => c.TenantName = v ?? string.Empty,
                (c) => c.TenantName, FieldPersistenceType.Read);
        private static readonly ChildListMap<TenantGroupAssignmentsDto, TenantGroupAssignmentDto> _assignmentsMap =
            new ChildListMap<TenantGroupAssignmentsDto, TenantGroupAssignmentDto>(nameof(Assignments),
                (c) => c.Assignments,
                (c, v) => c.Assignments = v, 
                (c) => c.Id.IsNullOrNew(), FieldPersistenceType.CRUD);
        private static readonly DeletedChildListMap<TenantGroupAssignmentsDto, TenantGroupAssignmentDto> _deletedAssignmentsMap =
            new DeletedChildListMap<TenantGroupAssignmentsDto, TenantGroupAssignmentDto>(
                nameof(DeletedAssignments), (c) => c.DeletedAssignments);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
