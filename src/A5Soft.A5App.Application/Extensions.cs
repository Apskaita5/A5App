using System;
using System.Collections.Generic;
using System.Text;
using A5Soft.A5App.Domain;
using A5Soft.A5App.Domain.Core;
using A5Soft.CARMA.Domain;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application
{
    public static class Extensions
    {
        /// <summary>
        /// Gets a value indicating whether the identity specified is null or references a new entity.
        /// </summary>
        /// <param name="entityIdentity"></param>
        /// <returns>value indicating whether the identity specified is null or references a new entity</returns>
        public static bool IsNullOrNew(this IDomainEntityIdentity entityIdentity)
        {
            return (entityIdentity?.IsNew ?? true);
        }

        /// <summary>
        /// Throws a <see cref="ValidationException"/> if the broken rule collection contains any errors.
        /// </summary>
        /// <param name="brokenRules"></param>
        public static void ThrowOnError(this BrokenRulesCollection brokenRules)
        {
            if (null == brokenRules) throw new ArgumentNullException(nameof(brokenRules));

            if (brokenRules.ErrorCount > 0) throw new ValidationException(brokenRules);
        }

        /// <summary>
        /// Creates a new <see cref="IDomainEntityIdentity"/> that references an entity of type T.
        /// </summary>
        /// <typeparam name="T">a type of domain entity to reference</typeparam>
        /// <param name="id"></param>
        /// <returns>a new <see cref="IDomainEntityIdentity"/> that references an entity of type T</returns>
        /// <remarks>returns reference for a new entity if the id is null or equals to Guid.Empty</remarks>
        public static IDomainEntityIdentity ToIdentity<T>(this Guid? id)
        {
            if (id.HasValue && id.Value != Guid.Empty) return new GuidDomainEntityIdentity(id.Value, typeof(T));
            return new GuidDomainEntityIdentity(typeof(T));
        }

        /// <summary>
        /// Creates a new <see cref="IDomainEntityIdentity"/> that references an entity of type T.
        /// </summary>
        /// <typeparam name="T">a type of domain entity to reference</typeparam>
        /// <param name="id"></param>
        /// <returns>a new <see cref="IDomainEntityIdentity"/> that references an entity of type T</returns>
        /// <remarks>returns reference for a new entity if the id equals to Guid.Empty</remarks>
        public static IDomainEntityIdentity ToIdentity<T>(this Guid id)
        {
            if (id != Guid.Empty) return new GuidDomainEntityIdentity(id, typeof(T));
            return new GuidDomainEntityIdentity(typeof(T));
        }

        /// <summary>
        /// Throws an exception if the identity specified is null
        /// or does not reference an existing entity of type T.
        /// </summary>
        /// <typeparam name="T">a type of entity that is expected to be referenced</typeparam>
        /// <param name="id">an identity to check</param>
        public static void EnsureValidIdentityFor<T>(this IDomainEntityIdentity id)
        {
            if (id.IsNull()) throw new ArgumentNullException(nameof(id));
            if (id.IsNew) throw new ArgumentException(
                $"Cannot fetch entity for non existent (IsNew) identity.", nameof(id));
            if (id.DomainEntityType != typeof(T)) throw new ArgumentException(
                $"Required identity for {typeof(T).FullName} while received {id.DomainEntityType.FullName}.",
                nameof(id));
        }

        /// <summary>
        /// Traverses the exception hierarchy and returns the first exception of type
        /// <see cref="BusinessException"/>. If no such exception, returns null;
        /// </summary>
        /// <param name="ex"></param>
        public static BusinessException ToBusinessException(this Exception ex)
        {
            if (null == ex) return null;

            if (ex is BusinessException bex) return bex;
            
            if (ex is AggregateException aex)
            {
                foreach (var e in aex.InnerExceptions)
                {
                    var result = e.ToBusinessException();
                    if (null != result) return result;
                }
            }
            else if (null != ex.InnerException)
            {
                return ex.InnerException.ToBusinessException();
            }

            return null;
        }

    }
}
