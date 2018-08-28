﻿using System;
using System.Collections.Generic;

namespace MarkSFrancis.Collections.Extensions
{
    /// <summary>
    /// Extension for <see cref="Array"/>
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Sets a range of elements in an array to the default value of each element type.
        /// </summary>
        /// <param name="array">The array to clear the elements of</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/></exception>
        public static void Clear(this Array array)
        {
            if (array == null)
            {
                throw ErrorFactory.Default.ArgumentNull(nameof(array));
            }

            Array.Clear(array, 0, array.Length);
        }

        /// <summary>
        /// Gets the index of the first occurance of a value within an array. Returns -1 if the value is not in the array
        /// </summary>
        /// <typeparam name="T">The type of array and value to get</typeparam>
        /// <param name="array">The array to search</param>
        /// <param name="value">The value to search for</param>
        /// <returns>Returns a 0 based index of the first occurance of the value in the array. If it is not found, returns -1</returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/></exception>
        public static int IndexOf<T>(this T[] array, T value)
        {
            return Array.IndexOf(array, value);
        }

        /// <summary>
        /// Gets the index of the last occurance of a value within an array. Returns -1 if the value is not in the array
        /// </summary>
        /// <typeparam name="T">The type of array and value to get</typeparam>
        /// <param name="array">The array to search</param>
        /// <param name="value">The value to search for</param>
        /// <returns>Returns a 0 based index of the first occurance of the value in the array. If it is not found, returns -1</returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/></exception>
        public static int LastIndexOf<T>(this T[] array, T value)
        {
            return Array.LastIndexOf(array, value);
        }

        /// <summary>
        /// Reverse the sequence of the elements within the array
        /// </summary>
        /// <param name="array">The array to reverse</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/></exception>
        public static void Reverse<T>(this T[] array)
        {
            Array.Reverse(array);
        }

        /// <summary>
        /// Reverse the sequence of a range of elements within the array
        /// </summary>
        /// <param name="array">The array to reverse</param>
        /// <param name="startIndex">The starting index of the section to reverse</param>
        /// <param name="length">The number of elements in the section to reverse</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than the lower bound of <paramref name="array"/> or <paramref name="length"/> is less than zero</exception>
        /// <exception cref="ArgumentException"><paramref name="startIndex"/> and <paramref name="length"/> do not specify a valid range in <paramref name="array"/></exception>
        public static void Reverse<T>(this T[] array, int startIndex, int length)
        {
            Array.Reverse(array, startIndex, length);
        }

        /// <summary>
        /// Sorts the elements in an <see cref="Array"/> using the <see cref="IComparable"/> implementation of each element of the <see cref="Array"/>
        /// </summary>
        /// <param name="array">The <see cref="Array"/> to sort</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/></exception>
        /// <exception cref="InvalidOperationException">One or more elements in <paramref name="array"/> do not implement the <see cref="IComparable"/> interface</exception>
        public static void Sort<T>(this T[] array)
        {
            Array.Sort(array);
        }

        /// <summary>
        /// Creates a shallow copy of the given array
        /// </summary>
        /// <typeparam name="T">The type of entries within the array</typeparam>
        /// <param name="array">The array to copy</param>
        /// <returns>A shallow copy of <paramref name="array"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/></exception>
        public static T[] ShallowCopy<T>(this T[] array)
        {
            if (array == null)
            {
                throw ErrorFactory.Default.ArgumentNull(nameof(array));
            }

            T[] copy = new T[array.Length];

            Array.Copy(array, copy, array.Length);

            return copy;
        }

        /// <summary>
        /// Changes the number of elements in the array to the specified new size
        /// </summary>
        /// <param name="array">The array to resize</param>
        /// <param name="newSize">The size of the new array</param>
        /// <typeparam name="T">The type of the elements in the array</typeparam>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="newSize"/> is less than zero</exception>
        public static T[] Resize<T>(this T[] array, int newSize)
        {
            if (array == null)
            {
                throw ErrorFactory.Default.ArgumentNull(nameof(array));
            }

            Array.Resize(ref array, newSize);

            return array;
        }

        /// <summary>
        /// Fill this array with a value
        /// </summary>
        /// <param name="array">The array to fill</param>
        /// <param name="fillWith">The value to fill the array with</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/></exception>
        /// <exception cref="InvalidCastException"><paramref name="fillWith"/> cannot be cast to the element type of <paramref name="array"/></exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> does not have exactly 1 dimension</exception>
        public static void Fill(this Array array, object fillWith)
        {
            if (array == null)
            {
                throw ErrorFactory.Default.ArgumentNull(nameof(array));
            }

            for (long index = 0; index < array.Length; ++index)
            {
                array.SetValue(fillWith, index);
            }
        }

        /// <summary>
        /// Fill this array with a value
        /// </summary>
        /// <param name="array">The array to fill</param>
        /// <param name="fillWith">The value to fill the array with</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/></exception>
        public static void Fill<T>(this T[] array, T fillWith)
        {
            if (array == null)
            {
                throw ErrorFactory.Default.ArgumentNull(nameof(array));
            }

            for (long index = 0; index < array.Length; ++index)
            {
                array[index] = fillWith;
            }
        }

        /// <summary>
        /// Searches a range of elements in a one-dimensional sorted array for a value, using the specified <see cref="IComparer{T}"/> generic interface</summary>
        /// <param name="array">The sorted one-dimensional, zero-based <see cref="Array"/> to search.</param>
        /// <param name="value">The object to search for</param>
        /// <param name="comparer">The <see cref="IComparer{T}"></see> implementation to use when comparing elements or <see langword="null"/> to use the <see cref="IComparable{T}"/> implementation of each element</param>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <returns>The index of the specified <paramref name="value"/> in the specified <paramref name="array"/>, if <paramref name="value"/> is found; otherwise, a negative number. If <paramref name="value"/> is not found and <paramref name="value"/> is less than one or more elements in <paramref name="array"/>, the negative number returned is the bitwise complement of the index of the first element that is larger than <paramref name="value"/>. If <paramref name="value"/> is not found and <paramref name="value"/> is greater than all elements in <paramref name="array"/>, the negative number returned is the bitwise complement of (the index of the last element plus 1). If this method is called with a non-sorted <paramref name="array"/>, the return value can be incorrect and a negative number could be returned, even if <paramref name="value"/> is present in <paramref name="array"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="comparer"/> is <see langword="null"/>, and <paramref name="value"/> is of a type that is not compatible with the elements of <paramref name="array"/></exception>
        /// <exception cref="InvalidOperationException"><paramref name="comparer"/> is <see langword="null"/>, and <typeparamref name="T"/> does not implement the <see cref="IComparable{T}"/> generic interface.</exception>
        public static int BinarySearchBy<T>(this T[] array, T value, IComparer<T> comparer)
        {
            return Array.BinarySearch(array, 0, array.Length, value, comparer);
        }
    }
}
