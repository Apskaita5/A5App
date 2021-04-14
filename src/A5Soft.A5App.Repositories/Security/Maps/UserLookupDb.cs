using A5Soft.A5App.Domain.Security.Lookups;
using A5Soft.DAL.Core.MicroOrm;
using System;

namespace A5Soft.A5App.Repositories.Security.Maps
{
    internal class UserLookupDb : UserLookup
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentGuid<UserLookup> _identityMap =
            new IdentityMapParentGuid<UserLookup>("users", "id",
            nameof(Id), () => new UserLookupDb(),
            (c) => c.Id,
            (c, v) => ((UserLookupDb)c)._id = v ??
            throw new InvalidOperationException("Lookup query returned null identity for user."));
        private static readonly FieldMapBool<UserLookup> _isSuspendedMap =
            new FieldMapBool<UserLookup>("is_suspended", nameof(IsSuspended),
            (c, v) => ((UserLookupDb)c)._isSuspended = v);
        private static readonly FieldMapBool<UserLookup> _isDisabledMap =
            new FieldMapBool<UserLookup>("is_disabled", nameof(IsDisabled),
            (c, v) => ((UserLookupDb)c)._isDisabled = v);
        private static readonly FieldMapDateTime<UserLookup> _updatedAtMap =
            new FieldMapDateTime<UserLookup>("updated_at", "UpdatedAt",
                (c, v) =>
                {
                    ((UserLookupDb) c)._updatedAt = v;
                    ((UserLookupDb)c).SetOccHash();
                });
#pragma warning restore IDE0052 // Remove unread private members
    }
}
