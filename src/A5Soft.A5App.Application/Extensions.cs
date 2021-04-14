using System;
using System.Collections.Generic;
using System.Text;
using A5Soft.A5App.Domain;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application
{
    public static class Extensions
    {

        public static bool IsNullOrNew(this IDomainEntityIdentity entityIdentity)
        {
            return (entityIdentity?.IsNew ?? true);
        }

        public static void ThrowOnError(this BrokenRulesCollection brokenRules)
        {
            if (null == brokenRules) throw new ArgumentNullException(nameof(brokenRules));

            if (brokenRules.ErrorCount > 0) throw new ValidationException(brokenRules);
        }

        public static IDomainEntityIdentity ToEntityIdentity<T>(this Guid? id)
        {
            if (id.HasValue && id.Value != Guid.Empty) return new GuidDomainEntityIdentity(id.Value, typeof(T));
            return new GuidDomainEntityIdentity(typeof(T));
        }

        public static IDomainEntityIdentity ToEntityIdentity<T>(this Guid id)
        {
            if (id != Guid.Empty) return new GuidDomainEntityIdentity(id, typeof(T));
            return new GuidDomainEntityIdentity(typeof(T));
        }

        public static void ValidateAsReference<T>(this IDomainEntityIdentity id)
        {
            if (id.IsNullOrNew()) throw new ArgumentNullException(nameof(id));
            if (id.DomainEntityType != typeof(T)) throw new ArgumentException(
                $"Invalid entity type, expected {typeof(T).FullName}, received {id.DomainEntityType.FullName}.");
        }

    }
}
