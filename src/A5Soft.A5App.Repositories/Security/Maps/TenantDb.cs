using A5Soft.A5App.Domain.Security;
using A5Soft.DAL.Core.MicroOrm;
using System;
using static A5Soft.A5App.Domain.Security.Tenant;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class TenantDb : TenantDto
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentGuid<TenantDto> _identityMap =
            new IdentityMapParentGuid<TenantDto>("tenants", "id",
                nameof(Id), () => new TenantDb(), 
                (c) => (Guid?)c.Id.IdentityValue,
                (c, v) => c.Id = v.ToIdentity<Tenant>());
        private static readonly FieldMapString<TenantDto> _tenantNameMap = 
            new FieldMapString<TenantDto>("tenant_name", nameof(Name), 
            (c, v) => c.Name = v,
            (c) => c.Name, FieldPersistenceType.CRUD);
        private static readonly FieldMapString<TenantDto> _dbNameMap = 
            new FieldMapString<TenantDto>("db_name", nameof(DatabaseName), 
            (c, v) => c.DatabaseName = v,
            (c) => c.DatabaseName, FieldPersistenceType.CRUD);
        private static readonly FieldMapInsertedAt<TenantDto> _insertedAtMap =
            new FieldMapInsertedAt<TenantDto>("inserted_at", nameof(InsertedAt),
                (c, v) => c.InsertedAt = v, (c) => c.InsertedAt);
        private static readonly FieldMapInsertedBy<TenantDto> _insertedByMap =
            new FieldMapInsertedBy<TenantDto>("inserted_by", nameof(InsertedBy),
                (c, v) => c.InsertedBy = v ?? string.Empty,
                (c) => c.InsertedBy);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
