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
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, object>> Cache =
            new ConcurrentDictionary<Type, ConcurrentDictionary<object, object>>();

        private static readonly object SyncContext = new object();

        private static ConcurrentDictionary<object, object> GetTable(Type type)
        {
            return Cache.GetOrAdd(type, new ConcurrentDictionary<object, object>());
        }

        /// <summary>
        /// Lazy load an entry from the cache
        /// </summary>
        /// <typeparam name="TId">The type of id for the object to load</typeparam>
        /// <typeparam name="TEntry">The type of entry to load</typeparam>
        /// <param name="id">The id of the entry to load</param>
        /// <param name="load">The method to call if the value is not already in the cache</param>
        /// <returns>The lazy loaded value with the primary key of <paramref name="id"/></returns>
        public static TEntry Get<TId, TEntry>(TId id, Func<TId, TEntry> load)
        {
            lock (SyncContext)
            {
                var table = GetTable(typeof(TEntry));

                return (TEntry)table.GetOrAdd(id, load);
            }
        }

        /// <summary>
        /// Add or update an entry in the cache
        /// </summary>
        /// <typeparam name="TId">The type of id for the object to add or update</typeparam>
        /// <typeparam name="TEntry">The type of entry to add or update</typeparam>
        /// <param name="id">The id of the entry to add or update</param>
        /// <param name="value">The value to add or update</param>
        public static void AddOrUpdate<TId, TEntry>(TId id, TEntry value)
        {
            lock (SyncContext)
            {
                var table = GetTable(typeof(TEntry));

                table.AddOrUpdate(id, value, (a, b) => value);
            }
        }

        /// <summary>
        /// Remove an entry from the cache
        /// </summary>
        /// <typeparam name="TId">The type of id for the object to remove</typeparam>
        /// <typeparam name="TEntry">The type of entry to remove</typeparam>
        /// <param name="id">The id of the entry to remove</param>
        public static void Remove<TId, TEntry>(TId id)
        {
            lock (SyncContext)
            {
                var table = GetTable(typeof(TEntry));

                table.TryRemove(id, out _);
            }
        }

        /// <summary>
        /// Clear a table from the cache
        /// </summary>
        /// <param name="tableType">The type of the table to clear</param>
        public static void Clear(Type tableType)
        {
            lock (SyncContext)
            {
                Cache.TryRemove(tableType, out _);
            }
        }

        /// <summary>
        /// Clear the entire cache
        /// </summary>
        public static void Clear()
        {
            lock (SyncContext)
            {
                Cache.Clear();
            }
        }

        /// <summary>
        /// Get the total number of items cached
        /// </summary>
        public static long TotalItemsCachedCount
        {
            get
            {
                lock (SyncContext)
                {
                    return Cache.Sum(c => (long)c.Value.Count);
                }
            }
        }

        /// <summary>
        /// Get the number of items cached in a specific table
        /// </summary>
        /// <param name="tableType"></param>
        /// <returns></returns>
        public static int TableItemsCachedCount(Type tableType)
        {
            lock (SyncContext)
            {
                var table = GetTable(tableType);

                return table.Count;
            }
        }
    }
}
