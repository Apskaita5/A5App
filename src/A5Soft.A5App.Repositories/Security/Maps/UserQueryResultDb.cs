using A5Soft.A5App.Application;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.DAL.Core.MicroOrm;
using A5Soft.A5App.Domain.Security;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserQueryResultDb : UserQueryResult
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapQueryResult<UserQueryResult> _identityMap =
            new IdentityMapQueryResult<UserQueryResult>(() => new UserQueryResultDb(),
            "FetchUserQueryResult");
        private static readonly FieldMapGuid<UserQueryResult> _idMap =
            new FieldMapGuid<UserQueryResult>(nameof(Id),
            (c, v) => ((UserQueryResultDb)c)._id = v.ToIdentity<User>());
        private static readonly FieldMapNullableGuid<UserQueryResult> _groupIdMap =
            new FieldMapNullableGuid<UserQueryResult>(nameof(GroupId),
            (c, v) => ((UserQueryResultDb)c)._groupId = v?.ToIdentity<UserGroup>());
        private static readonly FieldMapString<UserQueryResult> _groupNameMap =
            new FieldMapString<UserQueryResult>(nameof(GroupName),
            (c, v) => ((UserQueryResultDb)c)._groupName = v ?? string.Empty);
        private static readonly FieldMapString<UserQueryResult> _nameMap =
            new FieldMapString<UserQueryResult>(nameof(Name),
            (c, v) => ((UserQueryResultDb)c)._name = v ?? string.Empty);
        private static readonly FieldMapString<UserQueryResult> _emailMap =
            new FieldMapString<UserQueryResult>(nameof(Email),
            (c, v) => ((UserQueryResultDb)c)._email = v ?? string.Empty);
        private static readonly FieldMapString<UserQueryResult> _phoneMap =
            new FieldMapString<UserQueryResult>(nameof(Phone),
            (c, v) => ((UserQueryResultDb)c)._phone = v ?? string.Empty);
        private static readonly FieldMapBool<UserQueryResult> _isSuspendedMap =
            new FieldMapBool<UserQueryResult>(nameof(IsSuspended),
            (c, v) => ((UserQueryResultDb)c)._isSuspended = v);
        private static readonly FieldMapBool<UserQueryResult> _isDisabledMap =
            new FieldMapBool<UserQueryResult>(nameof(IsDisabled),
            (c, v) => ((UserQueryResultDb)c)._isDisabled = v);
        private static readonly FieldMapBool<UserQueryResult> _twoFactorEnabledMap =
            new FieldMapBool<UserQueryResult>(nameof(TwoFactorEnabled),
            (c, v) => ((UserQueryResultDb)c)._twoFactorEnabled = v);
        private static readonly FieldMapEnum<UserQueryResult, AdministrativeRole> _adminRoleMap =
            new FieldMapEnum<UserQueryResult, AdministrativeRole>(nameof(AdminRole),
            (c, v) => ((UserQueryResultDb)c)._adminRole = v);
        private static readonly FieldMapString<UserQueryResult> _rolesMap =
            new FieldMapString<UserQueryResult>(nameof(Roles),
            (c, v) => ((UserQueryResultDb)c)._roles = v ?? string.Empty);
        private static readonly FieldMapInt32<UserQueryResult> _tenantCountMap =
            new FieldMapInt32<UserQueryResult>(nameof(TenantCount),
            (c, v) => ((UserQueryResultDb)c)._tenantCount = v);
        private static readonly FieldMapDateTime<UserQueryResult> _insertedAtMap =
            new FieldMapDateTime<UserQueryResult>(nameof(InsertedAt),
            (c, v) => ((UserQueryResultDb)c)._insertedAt = v);
        private static readonly FieldMapString<UserQueryResult> _insertedByMap =
            new FieldMapString<UserQueryResult>(nameof(InsertedBy),
            (c, v) => ((UserQueryResultDb)c)._insertedBy = v ?? string.Empty);
        private static readonly FieldMapDateTime<UserQueryResult> _updatedAtMap =
            new FieldMapDateTime<UserQueryResult>(nameof(UpdatedAt),
            (c, v) => ((UserQueryResultDb)c)._updatedAt = v);
        private static readonly FieldMapString<UserQueryResult> _updatedByMap =
            new FieldMapString<UserQueryResult>(nameof(UpdatedBy),
            (c, v) => ((UserQueryResultDb)c)._updatedBy = v ?? string.Empty);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
