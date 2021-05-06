using A5Soft.A5App.Application;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.DAL.Core.MicroOrm;
using A5Soft.CARMA.Domain;
using A5Soft.A5App.Domain.Security;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class TenantQueryResultDb : TenantQueryResult
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapQueryResult<TenantQueryResult> _identityMap =
            new IdentityMapQueryResult<TenantQueryResult>(() => new TenantQueryResultDb(),
                "FetchTenantQueryResult");
        private static readonly FieldMapGuid<TenantQueryResult> _idMap =
            new FieldMapGuid<TenantQueryResult>(nameof(Id),
            (c, v) => ((TenantQueryResultDb)c)._id = v.ToIdentity<Tenant>());
        private static readonly FieldMapString<TenantQueryResult> _nameMap =
            new FieldMapString<TenantQueryResult>(nameof(Name),
                (c, v) => ((TenantQueryResultDb)c)._name = v ?? string.Empty);
        private static readonly FieldMapString<TenantQueryResult> _databaseNameMap =
            new FieldMapString<TenantQueryResult>(nameof(DatabaseName),
                (c, v) => ((TenantQueryResultDb)c)._databaseName = v ?? string.Empty);
        private static readonly FieldMapDateTime<TenantQueryResult> _insertedAtMap =
            new FieldMapDateTime<TenantQueryResult>(nameof(InsertedAt),
                (c, v) => ((TenantQueryResultDb)c)._insertedAt = v);
        private static readonly FieldMapString<TenantQueryResult> _insertedByMap =
            new FieldMapString<TenantQueryResult>(nameof(InsertedBy),
                (c, v) => ((TenantQueryResultDb)c)._insertedBy = v ?? string.Empty);
        private static readonly FieldMapString<TenantQueryResult> _userGroupsMap =
            new FieldMapString<TenantQueryResult>(nameof(UserGroups),
                (c, v) => ((TenantQueryResultDb)c)._userGroups = v ?? string.Empty);
        private static readonly FieldMapInt32<TenantQueryResult> _userCountMap =
            new FieldMapInt32<TenantQueryResult>(nameof(UserCount),
                (c, v) => ((TenantQueryResultDb)c)._userCount = v);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
