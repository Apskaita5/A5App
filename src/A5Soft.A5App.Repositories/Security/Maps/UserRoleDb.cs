using A5Soft.A5App.Domain.Security;
using A5Soft.DAL.Core.MicroOrm;
using System;
using static A5Soft.A5App.Domain.Security.UserRole;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserRoleDb : UserRoleDto
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentGuid<UserRoleDto> _identityMap =
            new IdentityMapParentGuid<UserRoleDto>("roles", "id",
            nameof(Id), () => new UserRoleDb(), 
            (c) => (Guid)c.Id.IdentityValue,
            (c, v) => c.Id = v.ToIdentity<UserRole>(), 
            "FetchUserRole");
        private static readonly FieldMapString<UserRoleDto> _roleNameMap = 
            new FieldMapString<UserRoleDto>("role_name", nameof(Name), 
            (c, v) => c.Name = v ?? string.Empty,
            (c) => c.Name, FieldPersistenceType.CRUD);
        private static readonly FieldMapString<UserRoleDto> _roleDescriptionMap = 
            new FieldMapString<UserRoleDto>("role_description", nameof(Description), 
            (c, v) => c.Description = v ?? string.Empty,
            (c) => c.Description, FieldPersistenceType.CRUD);
        private static readonly FieldMapInsertedAt<UserRoleDto> _insertedAtMap = 
            new FieldMapInsertedAt<UserRoleDto>("inserted_at", nameof(InsertedAt), 
                (c, v) => c.InsertedAt = v, (c) => c.InsertedAt);
        private static readonly FieldMapInsertedBy<UserRoleDto> _insertedByMap = 
            new FieldMapInsertedBy<UserRoleDto>("inserted_by", nameof(InsertedBy), 
                (c, v) => c.InsertedBy = v ?? string.Empty, 
                (c) => c.InsertedBy);
        private static readonly FieldMapUpdatedAt<UserRoleDto> _updatedAtMap = 
            new FieldMapUpdatedAt<UserRoleDto>("updated_at", nameof(UpdatedAt), 
                (c, v) => c.UpdatedAt = v, (c) => c.UpdatedAt);
        private static readonly FieldMapUpdatedBy<UserRoleDto> _updatedByMap = 
            new FieldMapUpdatedBy<UserRoleDto>("updated_by", nameof(UpdatedBy), 
                (c, v) => c.UpdatedBy = v ?? string.Empty,
                (c) => c.UpdatedBy);
        private static readonly FieldMapInt32<UserRoleDto> _userCountMap =
            new FieldMapInt32<UserRoleDto>(nameof(UserCount), (c, v) => c.UserCount = v);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
