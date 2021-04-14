using A5Soft.A5App.Domain.Security;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core.MicroOrm;
using System;
using static A5Soft.A5App.Domain.Security.User;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserDb : UserDto
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentGuid<UserDto> _identityMap =
            new IdentityMapParentGuid<UserDto>("users", "id",
            nameof(Id), () => new UserDb(), 
            (c) => (Guid?)c.Id.IdentityValue,
            (c, v) => c.Id = v.ToIdentity<User>());
        private static readonly FieldMapNullableGuid<UserDto> _userGroupIdMap = 
            new FieldMapNullableGuid<UserDto>("user_group_id",
            nameof(UserGroupId), (c, v) => c.UserGroupId = v.ToIdentity<UserGroup>(),
            (c) => (Guid?)c.UserGroupId.IdentityValue, FieldPersistenceType.CRUD);
        private static readonly FieldMapString<UserDto> _userNameMap = new FieldMapString<UserDto>("user_name",
            nameof(Name), (c, v) => c.Name = v ?? string.Empty,
            (c) => c.Name, FieldPersistenceType.CRUD);
        private static readonly FieldMapString<UserDto> _userEmailMap = new FieldMapString<UserDto>("user_email",
            nameof(Email), (c, v) => c.Email = v ?? string.Empty,
            (c) => c.Email, FieldPersistenceType.CRUD);
        private static readonly FieldMapString<UserDto> _userPhoneMap = new FieldMapString<UserDto>("user_phone",
            nameof(Phone), (c, v) => c.Phone = v ?? string.Empty,
            (c) => c.Phone, FieldPersistenceType.CRUD);
        private static readonly FieldMapBool<UserDto> _isSuspendedMap = new FieldMapBool<UserDto>("is_suspended",
            nameof(IsSuspended), (c, v) => c.IsSuspended = v,
            (c) => c.IsSuspended, FieldPersistenceType.CRUD);
        private static readonly FieldMapBool<UserDto> _isDisabledMap = new FieldMapBool<UserDto>("is_disabled",
            nameof(IsDisabled), (c, v) => c.IsDisabled = v,
            (c) => c.IsDisabled, FieldPersistenceType.CRUD);
        private static readonly FieldMapBool<UserDto> _twoFactorEnabledMap = new FieldMapBool<UserDto>("two_factor_enabled",
            nameof(TwoFactorEnabled), (c, v) => c.TwoFactorEnabled = v,
            (c) => c.TwoFactorEnabled, FieldPersistenceType.CRUD);
        private static readonly FieldMapEnum<UserDto, AdministrativeRole> _administrativeRoleIdMap = 
            new FieldMapEnum<UserDto, AdministrativeRole>("administrative_role_id", nameof(AdminRole), 
            (c, v) => c.AdminRole = v,
            (c) => c.AdminRole, FieldPersistenceType.CRUD);
        private static readonly FieldMapInsertedAt<UserDto> _insertedAtMap = 
            new FieldMapInsertedAt<UserDto>("inserted_at", nameof(InsertedAt), 
            (c, v) => c.InsertedAt = v, (c) => c.InsertedAt);
        private static readonly FieldMapInsertedBy<UserDto> _insertedByMap = 
            new FieldMapInsertedBy<UserDto>("inserted_by", nameof(InsertedBy), 
            (c, v) => c.InsertedBy = v, (c) => c.InsertedBy);
        private static readonly FieldMapUpdatedAt<UserDto> _updatedAtMap = 
            new FieldMapUpdatedAt<UserDto>("updated_at", nameof(UpdatedAt), 
            (c, v) => c.UpdatedAt = v, (c) => c.UpdatedAt);
        private static readonly FieldMapUpdatedBy<UserDto> _updatedByMap = 
            new FieldMapUpdatedBy<UserDto>("updated_by", nameof(UpdatedBy), 
            (c, v) => c.UpdatedBy = v, (c) => c.UpdatedBy);
        private static readonly FieldMapString<UserDto> _hashedPasswordMap = 
            new FieldMapString<UserDto>("password_hash", nameof(HashedPassword), 
            (c, v) => c.HashedPassword = v ?? string.Empty,
            (c) => c.HashedPassword, FieldPersistenceType.Insert);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
