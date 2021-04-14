using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.DAL.Core.MicroOrm;
using System;
using A5Soft.A5App.Domain.Security;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    [Serializable]
    internal class TenantLookupDb : TenantLookup
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentGuid<TenantLookup> _identityMap =
            new IdentityMapParentGuid<TenantLookup>("tenants", "id",
                nameof(Id), () => new TenantLookupDb(),
                (c) => (Guid)((TenantLookupDb)c)._id.IdentityValue,
                (c, v) => ((TenantLookupDb)c)._id = v.ToIdentity<Tenant>());
        private static readonly FieldMapString<TenantLookup> _roleNameMap =
            new FieldMapString<TenantLookup>("tenant_name", nameof(Name),
                (c, v) => ((TenantLookupDb)c)._name = v);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
