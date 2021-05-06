using A5Soft.DAL.Core.MicroOrm;
using A5Soft.A5App.Domain.Security;
using System;
using A5Soft.A5App.Application;
using static A5Soft.A5App.Domain.Security.UserGroup;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserGroupDb : UserGroupDto
    {
    #pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentGuid<UserGroupDto> _identityMap =
            new IdentityMapParentGuid<UserGroupDto>("user_groups", "id",
            nameof(Id), () => new UserGroupDb(), 
            (c) => (Guid?)c.Id.IdentityValue,
            (c, v) => c.Id = v.ToIdentity<UserGroup>(),
            "FetchUserGroup");

        private static readonly FieldMapString<UserGroupDto> _groupNameMap = 
            new FieldMapString<UserGroupDto>("group_name", nameof(GroupName), 
                (c, v) => c.GroupName = v ?? string.Empty,
                (c) => c.GroupName ?? string.Empty, FieldPersistenceType.CRUD);
        private static readonly FieldMapInt32<UserGroupDto> _maxUsersMap = 
            new FieldMapInt32<UserGroupDto>("max_users", nameof(MaxUsers), 
                (c, v) => c.MaxUsers = v,
                (c) => c.MaxUsers, FieldPersistenceType.CRUD);
        private static readonly FieldMapInt32<UserGroupDto> _maxTenantsMap = 
            new FieldMapInt32<UserGroupDto>("max_tenants", nameof(MaxTenants), 
                (c, v) => c.MaxTenants = v,
                (c) => c.MaxTenants, FieldPersistenceType.CRUD);
        private static readonly FieldMapInt32<UserGroupDto> _userCountMap =
            new FieldMapInt32<UserGroupDto>(nameof(UserCount), (c, v) => c.UserCount = v);
        private static readonly FieldMapInsertedAt<UserGroupDto> _insertedAtMap = 
            new FieldMapInsertedAt<UserGroupDto>("inserted_at", nameof(InsertedAt), 
                (c, v) => c.InsertedAt = v, (c) => c.InsertedAt);
        private static readonly FieldMapInsertedBy<UserGroupDto> _insertedByMap = 
            new FieldMapInsertedBy<UserGroupDto>("inserted_by", nameof(InsertedBy), 
                (c, v) => c.InsertedBy = v, (c) => c.InsertedBy);
        private static readonly FieldMapUpdatedAt<UserGroupDto> _updatedAtMap = 
            new FieldMapUpdatedAt<UserGroupDto>("updated_at", nameof(UpdatedAt), 
                (c, v) => c.UpdatedAt = v, (c) => c.UpdatedAt);
        private static readonly FieldMapUpdatedBy<UserGroupDto> _updatedByMap = 
            new FieldMapUpdatedBy<UserGroupDto>("updated_by", nameof(UpdatedBy),
                (c, v) => c.UpdatedBy = v, (c) => c.UpdatedBy);
     #pragma warning restore IDE0052 // Remove unread private members
    }
}
