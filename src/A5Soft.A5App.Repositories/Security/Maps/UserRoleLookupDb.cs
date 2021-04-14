using A5Soft.A5App.Domain.Security;
using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.DAL.Core.MicroOrm;
using System;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserRoleLookupDb : UserRoleLookup
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentGuid<UserRoleLookup> _identityMap =
            new IdentityMapParentGuid<UserRoleLookup>("roles", "id",
            nameof(Id), () => new UserRoleLookupDb(),
            (c) => (Guid)((UserRoleLookupDb)c)._id.IdentityValue,
            (c, v) => ((UserRoleLookupDb)c)._id = v.ToIdentity<UserRole>());
        private static readonly FieldMapString<UserRoleLookup> _roleNameMap =
            new FieldMapString<UserRoleLookup>("role_name", nameof(Name),
            (c, v) => ((UserRoleLookupDb)c)._name = v);
        private static readonly FieldMapString<UserRoleLookup> _roleDescriptionMap =
            new FieldMapString<UserRoleLookup>("role_description", nameof(Description),
            (c, v) => ((UserRoleLookupDb)c)._description = v);
#pragma warning restore IDE0052 // Remove unread private members
    }
}
