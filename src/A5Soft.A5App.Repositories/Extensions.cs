using System;
using System.Collections.Generic;
using System.Text;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Repositories
{
    internal static class Extensions
    {

        internal static void EnsureValidIdentityFor<T>(this IDomainEntityIdentity id)
        {
            if (null == id) throw new ArgumentNullException(nameof(id));
            if (id.IsNew) throw new ArgumentException(
                $"Cannot fetch entity for non existent (IsNew) identity.", nameof(id));
            if (id.DomainEntityType != typeof(T)) throw new InvalidOperationException(
                $"Required identity for {typeof(T).FullName} while received {id.DomainEntityType.FullName}.");
        }

        internal static bool IsNullOrNew(this IDomainEntityIdentity id)
        {
            return (id?.IsNew ?? true);
        }

        internal static IDomainEntityIdentity ToIdentity<T>(this Guid value)
        {
            if (value == Guid.Empty) return new GuidDomainEntityIdentity(typeof(T));
            return new GuidDomainEntityIdentity(value, typeof(T));
        }

        internal static IDomainEntityIdentity ToIdentity<T>(this Guid? value)
        {
            if (!value.HasValue || value.Value == Guid.Empty) return new GuidDomainEntityIdentity(typeof(T));
            return new GuidDomainEntityIdentity(value.Value, typeof(T));
        }

        internal static IDomainEntityIdentity ToIdentity<T>(this int value)
        {
            if (!value.IsValidKey()) return new IntDomainEntityIdentity(typeof(T));
            return new IntDomainEntityIdentity(value, typeof(T));
        }

        internal static IDomainEntityIdentity ToIdentity<T>(this int? value)
        {
            if (!value.IsValidKey()) return new IntDomainEntityIdentity(typeof(T));
            return new IntDomainEntityIdentity(value.Value, typeof(T));
        }

    }
}
