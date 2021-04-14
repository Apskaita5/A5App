using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Queries;
using A5Soft.CARMA.Domain;
using A5Soft.DAL.Core.MicroOrm;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserRoleQueryResultDb : UserRoleQueryResult
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapQueryResult<UserRoleQueryResult> _identityMap =
            new IdentityMapQueryResult<UserRoleQueryResult>(() => new UserRoleQueryResultDb(),
            "QueryUserRoles");
        private static readonly FieldMapGuid<UserRoleQueryResult> _idMap =
            new FieldMapGuid<UserRoleQueryResult>(nameof(Id),
                (c, v) => ((UserRoleQueryResultDb)c)._id = 
                    v.ToIdentity<UserRole>());
        private static readonly FieldMapString<UserRoleQueryResult> _nameMap =
            new FieldMapString<UserRoleQueryResult>(nameof(Name),
                (c, v) => ((UserRoleQueryResultDb)c)._name = v);
        private static readonly FieldMapString<UserRoleQueryResult> _descriptionMap =
            new FieldMapString<UserRoleQueryResult>(nameof(Description),
                (c, v) => ((UserRoleQueryResultDb)c)._description = v);
        private static readonly FieldMapDateTime<UserRoleQueryResult> _insertedAtMap =
            new FieldMapDateTime<UserRoleQueryResult>(nameof(InsertedAt),
                (c, v) => ((UserRoleQueryResultDb)c)._insertedAt = v);
        private static readonly FieldMapString<UserRoleQueryResult> _insertedByMap =
            new FieldMapString<UserRoleQueryResult>(nameof(InsertedBy),
                (c, v) => ((UserRoleQueryResultDb)c)._insertedBy = v);
        private static readonly FieldMapDateTime<UserRoleQueryResult> _updatedAtMap =
            new FieldMapDateTime<UserRoleQueryResult>(nameof(UpdatedAt),
                (c, v) => ((UserRoleQueryResultDb)c)._updatedAt = v);
        private static readonly FieldMapString<UserRoleQueryResult> _updatedByMap =
            new FieldMapString<UserRoleQueryResult>(nameof(UpdatedBy),
                (c, v) => ((UserRoleQueryResultDb)c)._updatedBy = v);
        private static readonly FieldMapInt32<UserRoleQueryResult> _userCountMap =
            new FieldMapInt32<UserRoleQueryResult>(nameof(UserCount),
                (c, v) => ((UserRoleQueryResultDb)c)._userCount = v);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
