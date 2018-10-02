﻿using Microsoft.Extensions.Caching.Memory;

namespace Phnx.AspNetCore.Cache.Interfaces
{
    /// <summary>
    /// A configured <see cref="LazyMemoryCache"/> entry
    /// </summary>
    /// <typeparam name="T">The type of data represented by this cache entry</typeparam>
    public interface ICacheEntry<out T>
    {
        /// <summary>
        /// The unique name or key for this cache entry
        /// </summary>
        string Key { get; }

        /// <summary>
        /// The memory storage options for this cache entry. See <see cref="CacheEntryOptionsHelper"/> for easy configuration of options
        /// </summary>
        MemoryCacheEntryOptions Options { get; }

        /// <summary>
        /// The method used when loading the value for this cache entry from its external source
        /// </summary>
        /// <returns>The value loaded from the cache</returns>
        T LoadFromExternalSource();
    }
}