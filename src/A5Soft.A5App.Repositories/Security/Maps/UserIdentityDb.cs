using A5Soft.A5App.Application.UseCases.Security;
using A5Soft.A5App.Domain.Security;
using A5Soft.DAL.Core.MicroOrm;
using System;
using System.Collections.Generic;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserIdentityDb : UserIdentity
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapQueryResult<UserIdentityDb> _identityMap =
            new IdentityMapQueryResult<UserIdentityDb>(() => new UserIdentityDb(),
            "FetchUserIdentity");
        private static readonly FieldMapGuid<UserIdentityDb> _idMap =
            new FieldMapGuid<UserIdentityDb>(nameof(Id),
            (c, v) => c._id = v);
        private static readonly FieldMapNullableGuid<UserIdentityDb> _groupIdMap =
            new FieldMapNullableGuid<UserIdentityDb>(nameof(GroupId),
            (c, v) => c._groupId = v);
        private static readonly FieldMapString<UserIdentityDb> _groupNameMap =
            new FieldMapString<UserIdentityDb>(nameof(GroupName),
            (c, v) => c._groupName = v ?? string.Empty);
        private static readonly FieldMapString<UserIdentityDb> _nameMap =
            new FieldMapString<UserIdentityDb>(nameof(Name),
            (c, v) => c._name = v ?? string.Empty);
        private static readonly FieldMapString<UserIdentityDb> _emailMap =
            new FieldMapString<UserIdentityDb>(nameof(Email),
            (c, v) => c._email = v ?? string.Empty);
        private static readonly FieldMapString<UserIdentityDb> _phoneMap =
            new FieldMapString<UserIdentityDb>(nameof(Phone),
            (c, v) => c._phone = v ?? string.Empty);
        private static readonly FieldMapBool<UserIdentityDb> _isSuspendedMap =
            new FieldMapBool<UserIdentityDb>(nameof(IsSuspended),
            (c, v) => c._isSuspended = v);
        private static readonly FieldMapBool<UserIdentityDb> _isDisabledMap =
            new FieldMapBool<UserIdentityDb>(nameof(IsDisabled),
            (c, v) => c._isDisabled = v);
        private static readonly FieldMapBool<UserIdentityDb> _twoFactorEnabledMap =
            new FieldMapBool<UserIdentityDb>(nameof(TwoFactorEnabled),
            (c, v) => c._twoFactorEnabled = v);
        private static readonly FieldMapEnum<UserIdentityDb, AdministrativeRole> _adminRoleMap =
            new FieldMapEnum<UserIdentityDb, AdministrativeRole>(nameof(AdminRole),
            (c, v) => c._adminRole = v);
        private static readonly FieldMapDateTime<UserIdentityDb> _updatedAtMap =
            new FieldMapDateTime<UserIdentityDb>("UpdatedAt",
                (c, v) =>
                {
                    c._updatedAt = v;
                    c.SetOccHash();
                });
#pragma warning restore IDE0052 // Remove unread private members

        public void SetPermissions(List<Guid> permissions)
        {
            _permissions = permissions;
        }

        public void SetTenant(Guid? tenantId)
        {
            _tenantId = tenantId;
        }
    }
}
