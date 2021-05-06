using A5Soft.A5App.Application;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.DAL.Core.MicroOrm;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserGroupQueryResultDb : UserGroupQueryResult
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapQueryResult<UserGroupQueryResult> _identityMap =
            new IdentityMapQueryResult<UserGroupQueryResult>(() => new UserGroupQueryResultDb(),
                "QueryUserGroups");
        private static readonly FieldMapGuid<UserGroupQueryResult> _idMap = 
            new FieldMapGuid<UserGroupQueryResult>("Id", 
                (c, v) => ((UserGroupQueryResultDb)c)._id = 
                    v.ToIdentity<UserGroup>());
        private static readonly FieldMapString<UserGroupQueryResult> _groupNameMap = 
            new FieldMapString<UserGroupQueryResult>("GroupName", 
                (c, v) => ((UserGroupQueryResultDb)c)._groupName = v);
        private static readonly FieldMapInt32<UserGroupQueryResult> _maxUsersMap = 
            new FieldMapInt32<UserGroupQueryResult>("MaxUsers", 
                (c, v) => ((UserGroupQueryResultDb)c)._maxUsers = v);
        private static readonly FieldMapInt32<UserGroupQueryResult> _maxTenantsMap = 
            new FieldMapInt32<UserGroupQueryResult>("MaxTenants", 
                (c, v) => ((UserGroupQueryResultDb)c)._maxTenants = v);
        private static readonly FieldMapDate<UserGroupQueryResult> _insertedAtMap = 
            new FieldMapDate<UserGroupQueryResult>("InsertedAt", 
                (c, v) => ((UserGroupQueryResultDb)c)._insertedAt = v);
        private static readonly FieldMapString<UserGroupQueryResult> _insertedByMap = 
            new FieldMapString<UserGroupQueryResult>("InsertedBy", 
                (c, v) => ((UserGroupQueryResultDb)c)._insertedBy = v);
        private static readonly FieldMapDate<UserGroupQueryResult> _updatedAtMap = 
            new FieldMapDate<UserGroupQueryResult>("UpdatedAt", 
                (c, v) => ((UserGroupQueryResultDb)c)._updatedAt = v);
        private static readonly FieldMapString<UserGroupQueryResult> _updatedByMap = 
            new FieldMapString<UserGroupQueryResult>("UpdatedBy", 
                (c, v) => ((UserGroupQueryResultDb)c)._updatedBy = v);
        private static readonly FieldMapNullableInt32<UserGroupQueryResult> _tenantCountMap = 
            new FieldMapNullableInt32<UserGroupQueryResult>("TenantCount", 
                (c, v) => ((UserGroupQueryResultDb)c)._tenantCount = v ?? 0);
        private static readonly FieldMapNullableInt32<UserGroupQueryResult> _userCountMap = 
            new FieldMapNullableInt32<UserGroupQueryResult>("UserCount", 
                (c, v) => ((UserGroupQueryResultDb)c)._userCount = v ?? 0);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
