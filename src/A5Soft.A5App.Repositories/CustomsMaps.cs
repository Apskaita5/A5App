using System;
using System.Collections.Generic;
using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.A5App.Repositories.Security.Maps;

namespace A5Soft.A5App.Repositories
{
    internal static class CustomsMaps
    {
        public static Dictionary<Type, Type> CustomMaps { get; }
            = new Dictionary<Type, Type>()
            {
                { typeof(UserGroup.UserGroupDto), typeof(UserGroupDb) },
                { typeof(UserGroupQueryResult), typeof(UserGroupQueryResultDb) },
                { typeof(UserGroupLookup), typeof(UserGroupLookupDb) },
                { typeof(UserGroupQueryResult), typeof(UserGroupQueryResultDb) },
                { typeof(UserRole.UserRoleDto), typeof(UserRoleDb) },
                { typeof(UserRoleLookup), typeof(UserRoleLookupDb) },
                { typeof(UserRoleQueryResult), typeof(UserRoleQueryResultDb) },
                { typeof(Tenant.TenantDto), typeof(TenantDb) },
                { typeof(TenantQueryResult), typeof(TenantQueryResultDb) },
                { typeof(TenantLookup), typeof(TenantLookupDb) },
                { typeof(UserTenant.UserTenantDto), typeof(UserTenantDb) },
                { typeof(User.UserDto), typeof(UserDb) },
                { typeof(UserQueryResult), typeof(UserQueryResultDb) },
                { typeof(TenantGroupAssignment), typeof(TenantGroupAssignmentDb) },
                { typeof(TenantGroupAssignments.TenantGroupAssignmentsDto), typeof(TenantGroupAssignmentsDb) }



            };

    }
}
