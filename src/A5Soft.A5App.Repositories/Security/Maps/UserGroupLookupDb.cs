using A5Soft.A5App.Domain.Security;
using A5Soft.DAL.Core.MicroOrm;
using System;
using A5Soft.A5App.Domain.Security.Lookups;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    class UserGroupLookupDb : UserGroupLookup
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentGuid<UserGroupLookup> _identityMap =
            new IdentityMapParentGuid<UserGroupLookup>("user_groups", "id",
                nameof(Id), () => new UserGroupLookupDb(),
                (c) => (Guid?)((UserGroupLookupDb)c)._id.IdentityValue,
                (c, v) => ((UserGroupLookupDb)c)._id = v.ToIdentity<UserGroup>());
        private static readonly FieldMapString<UserGroupLookup> _groupNameMap = 
            new FieldMapString<UserGroupLookup>("group_name", nameof(GroupName), 
                (c, v) => ((UserGroupLookupDb)c)._groupName = v);
        private static readonly FieldMapInt32<UserGroupLookup> _maxUsersMap = 
            new FieldMapInt32<UserGroupLookup>("max_users", nameof(MaxUsers), 
                (c, v) => ((UserGroupLookupDb)c)._maxUsers = v);
        private static readonly FieldMapInt32<UserGroupLookup> _maxTenantsMap = 
            new FieldMapInt32<UserGroupLookup>("max_tenants", nameof(MaxTenants), 
                (c, v) => ((UserGroupLookupDb)c)._maxTenants = v);
#pragma warning restore IDE0052 // Remove unread private members

    }
}
