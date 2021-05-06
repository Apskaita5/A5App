using A5Soft.A5App.Application.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Infrastructure
{
    /// <summary>
    /// <see cref="ICacheProvider"/> implementation using <see cref="ConcurrentDictionary{}"/>.
    /// </summary>
    [DefaultServiceImplementation(typeof(ICacheProvider))]
    public class ConcurrentDictionaryCacheProvider : ICacheProvider
    {
        private readonly ConcurrentDictionary<string, Task<object>> _dict
            = new ConcurrentDictionary<string, Task<object>>();


        public ConcurrentDictionaryCacheProvider() { }


        /// <inheritdoc cref="ICacheProvider.Clear{T}()"/>
        public void Clear<T>()
            => _dict.TryRemove(GetItemKey<T>(Guid.Empty), out Task<object> _);

        /// <inheritdoc cref="ICacheProvider.Clear{T}()"/>
        public void Clear(Type cachedItemType)
            => _dict.TryRemove(GetItemKey(cachedItemType, Guid.Empty), out Task<object> _);

        /// <inheritdoc cref="ICacheProvider.Clear{T}()"/>
        public void Clear<T>(Guid tenantId)
        {
            if (tenantId == Guid.Empty) throw new ArgumentNullException(nameof(tenantId));
            _dict.TryRemove(GetItemKey<T>(tenantId), out Task<object> _);
        }

        /// <inheritdoc cref="ICacheProvider.Clear{T}()"/>
        public void Clear(Guid tenantId, Type cachedItemType)
        {
            if (tenantId == Guid.Empty) throw new ArgumentNullException(nameof(tenantId));
            _dict.TryRemove(GetItemKey(cachedItemType, tenantId), out Task<object> _);
        }

        /// <inheritdoc cref="ICacheProvider.GetOrCreate{T}()"/>
        public async Task<T> GetOrCreate<T>(Func<Task<T>> factory)
            => await GetOrCreateInt<T>(Guid.Empty, factory);

        /// <inheritdoc cref="ICacheProvider.GetOrCreate{T}()"/>
        public async Task<T> GetOrCreate<T>(Guid tenantId, Func<Task<T>> factory)
        {
            if (tenantId == Guid.Empty) throw new ArgumentNullException(nameof(tenantId));
            return await GetOrCreateInt<T>(tenantId, factory);
        }


        private async Task<T> GetOrCreateInt<T>(Guid tenantId, Func<Task<T>> factory)
        {
            if (null == factory) throw new ArgumentNullException(nameof(factory));
            var result = _dict.GetOrAdd(GetItemKey<T>(tenantId), k => factory().ContinueWith<object>(
                t => t.Result, TaskContinuationOptions.OnlyOnRanToCompletion));
            return (T)(await result);
        }

        private string GetItemKey<T>(Guid tenantId)
        {
            return GetItemKey(typeof(T), tenantId);
        }

        private string GetItemKey(Type cachedItemType, Guid tenantId)
        {
            return tenantId == Guid.Empty ? cachedItemType.FullName :
                $"{tenantId:N}:{cachedItemType.FullName}";
        }
         
    }
}
