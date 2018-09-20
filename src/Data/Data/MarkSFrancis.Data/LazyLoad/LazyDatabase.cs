﻿using System;
using System.Collections.Concurrent;
using System.Linq;

namespace MarkSFrancis.Data.LazyLoad
{
    /// <summary>
    /// A thread-safe lazy loaded database, with tables seperated by model types
    /// </summary>
    public static class LazyDatabase
    {
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, object>> _cache =
            new ConcurrentDictionary<Type, ConcurrentDictionary<object, object>>();

        private static readonly object _syncContext = new object();

        private static ConcurrentDictionary<object, object> GetTable<T>()
        {
            return _cache.GetOrAdd(typeof(T), new ConcurrentDictionary<object, object>());
        }

        /// <summary>
        /// Lazy load an entry from the cache
        /// </summary>
        /// <typeparam name="TKey">The type of id for the object to load</typeparam>
        /// <typeparam name="TEntry">The type of entry to load</typeparam>
        /// <param name="id">The id of the entry to load</param>
        /// <param name="load">The method to call if the value is not already in the cache</param>
        /// <returns>The lazy loaded value with the primary key of <paramref name="id"/></returns>
        public static TEntry Get<TKey, TEntry>(TKey id, Func<TKey, TEntry> load)
        {
            lock (_syncContext)
            {
                var table = GetTable<TEntry>();

                return (TEntry)table.GetOrAdd(id, load);
            }
        }

        /// <summary>
        /// Add or update an entry in the cache
        /// </summary>
        /// <typeparam name="TKey">The type of id for the object to add or update</typeparam>
        /// <typeparam name="TEntry">The type of entry to add or update</typeparam>
        /// <param name="id">The id of the entry to add or update</param>
        /// <param name="value">The value to add or update</param>
        public static void AddOrUpdate<TKey, TEntry>(TKey id, TEntry value)
        {
            lock (_syncContext)
            {
                var table = GetTable<TEntry>();

                table.AddOrUpdate(id, value, (a, b) => value);
            }
        }

        /// <summary>
        /// Remove an entry from the cache
        /// </summary>
        /// <typeparam name="TKey">The type of id for the object to remove</typeparam>
        /// <typeparam name="TEntry">The type of entry to remove</typeparam>
        /// <param name="id">The id of the entry to remove</param>
        public static void Remove<TKey, TEntry>(TKey id)
        {
            lock (_syncContext)
            {
                var table = GetTable<TEntry>();

                table.TryRemove(id, out _);
            }
        }

        /// <summary>
        /// Clear a table from the cache
        /// </summary>
        /// <typeparam name="T">The type of the table to clear</typeparam>
        public static void Clear<T>()
        {
            lock (_syncContext)
            {
                _cache.TryRemove(typeof(T), out _);
            }
        }

        /// <summary>
        /// Clear the entire cache
        /// </summary>
        public static void Clear()
        {
            lock (_syncContext)
            {
                _cache.Clear();
            }
        }

        /// <summary>
        /// Get the total number of items cached
        /// </summary>
        public static long TotalItemsCachedCount
        {
            get
            {
                lock (_syncContext)
                {
                    return _cache.Sum(c => (long)c.Value.Count);
                }
            }
        }

        /// <summary>
        /// Get the number of items cached in a specific table
        /// </summary>
        /// <typeparam name="T">The type of the table to get the total items cached in</typeparam>
        /// <returns></returns>
        public static int TableItemsCachedCount<T>()
        {
            lock (_syncContext)
            {
                var table = GetTable(typeof(T));

                return table.Count;
            }
        }
    }
}
