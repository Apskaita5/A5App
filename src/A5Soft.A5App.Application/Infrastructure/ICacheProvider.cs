using System;
using System.Threading.Tasks;

namespace A5Soft.A5App.Application.Infrastructure
{

    /// <summary>
    /// Represents an interface that should be implemented by a particular caching
    /// implementation, e.g. System.Runtime.Caching.MemoryCache for full .NET
    /// or Microsoft.Extensions.Caching.Memory.IMemoryCache for .NET core.
    /// </summary>
    public interface ICacheProvider
    {

        /// <summary>
        /// Retrieve cached item of type <typeparamref name="T"/> from the shared cache or fetch one by the factory method. 
        /// </summary>
        /// <typeparam name="T">a type of the cached item to retrieve</typeparam>
        /// <param name="factory">a method to fetch the item to cache</param>
        /// <returns></returns>
        Task<T> GetOrCreate<T>(Func<Task<T>> factory);

        /// <summary>
        /// Remove item of type <typeparamref name="T"/> from shared cache.
        /// </summary>
        /// <typeparam name="T">a type of the cached item to clear</typeparam>
        void Clear<T>();

        /// <summary>
        /// Remove item of type specified from shared cache.
        /// </summary>
        /// <param name="cachedItemType">a type of the cached item to clear</param>
        void Clear(Type cachedItemType);

        /// <summary>
        /// Retrieve cached item of type <typeparamref name="T"/> for the tenant or fetch one by the factory method. 
        /// </summary>
        /// <typeparam name="T">a type of the cached item to retrieve</typeparam>
        /// <param name="factory">a method to fetch the item to cache</param>
        /// <param name="tenantId">an id of the tenant to retrieve or create a cached item for</param>
        Task<T> GetOrCreate<T>(string tenantId, Func<Task<T>> factory);

        /// <summary>
        /// Remove item of type <typeparamref name="T"/> from cache for tenant.
        /// </summary>
        /// <typeparam name="T">a type of the cached item to clear</typeparam> 
        /// <param name="tenantId">an id of the tenant to retrieve or create a cached item for</param>
        void Clear<T>(string tenantId);

        /// <summary>
        /// Remove item of type specified from cache for tenant.
        /// </summary>
        /// <param name="cachedItemType">a type of the cached item to clear</param>  
        /// <param name="tenantId">an id of the tenant to retrieve or create a cached item for</param>
        void Clear(string tenantId, Type cachedItemType);

    }
}
