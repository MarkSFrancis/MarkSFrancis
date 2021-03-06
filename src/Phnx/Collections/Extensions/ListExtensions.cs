﻿using System;
using System.Collections.Generic;

namespace Phnx.Collections
{
    /// <summary>
    /// Extensions for <see cref="List{T}"/>
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Insert a range of values
        /// </summary>
        /// <typeparam name="T">The type of values in the collection</typeparam>
        /// <param name="source">The values in the original collection</param>
        /// <param name="startIndex">The index at which inserting into <paramref name="source"/> begins</param>
        /// <param name="valuesToInsert">The values to insert into the collection</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="valuesToInsert"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than or equal to the size of <paramref name="source"/></exception>
        public static void InsertRange<T>(this List<T> source, int startIndex, IReadOnlyList<T> valuesToInsert)
        {
            if (valuesToInsert is null)
            {
                throw new ArgumentNullException(nameof(valuesToInsert));
            }

            InsertRange(source, startIndex, valuesToInsert, valuesToInsert.Count);
        }

        /// <summary>
        /// Insert a range of values
        /// </summary>
        /// <typeparam name="T">The type of values in the collection</typeparam>
        /// <param name="source">The values in the original collection</param>
        /// <param name="startIndex">The index at which inserting into <paramref name="source"/> begins</param>
        /// <param name="valuesToInsert">The values to insert into the collection</param>
        /// <param name="count">The number of records from <paramref name="valuesToInsert"/> to insert</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="valuesToInsert"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than or equal to the size of <paramref name="source"/></exception>
        public static void InsertRange<T>(this List<T> source, int startIndex, IReadOnlyList<T> valuesToInsert, int count)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (valuesToInsert is null)
            {
                throw new ArgumentNullException(nameof(valuesToInsert));
            }
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }
            if (startIndex > source.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), $"{nameof(startIndex)} cannot be greater than or equal to the size of {nameof(source)}. {nameof(source)} has a size of {source.Count}");
            }

            if (source.Capacity < count + source.Count)
            {
                // source needs to grow
                int newCapacity;
                int newCapacityMinimum = count + source.Count;

                do
                {
                    newCapacity = source.Capacity *= 2;
                } while (newCapacity < newCapacityMinimum);

                source.Capacity = newCapacity;
            }

            var replacedValues = new Queue<T>();

            for (int valuesToInsertIndex = 0; valuesToInsertIndex < count; ++valuesToInsertIndex)
            {
                var insertToIndex = startIndex + valuesToInsertIndex;

                if (insertToIndex < source.Count)
                {
                    replacedValues.Enqueue(source[insertToIndex]);
                    source[insertToIndex] = valuesToInsert[valuesToInsertIndex];
                }
                else
                {
                    source.Add(valuesToInsert[valuesToInsertIndex]);
                }
            }

            for (var index = startIndex + count; index < source.Count; ++index)
            {
                // Restore replaced values at the end of the inserted values
                replacedValues.Enqueue(source[index]);
                source[index] = replacedValues.Dequeue();
            }

            while (replacedValues.Count > 0)
            {
                // Restore replaced values at the end of the collection
                source.Add(replacedValues.Dequeue());
            }
        }
    }
}
