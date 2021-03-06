﻿using System;
using System.IO;

namespace Phnx.Collections
{
    /// <summary>
    /// Extensions for <see cref="T:byte[]"/>
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Create a new memory stream from an array of bytes
        /// </summary>
        /// <param name="source">The bytes to fill the <see cref="MemoryStream"/> with</param>
        /// <returns>A <see cref="MemoryStream"/> filled with specified bytes</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/></exception>
        public static MemoryStream ToMemoryStream(this byte[] source)
        {
            return new MemoryStream(source);
        }

        /// <summary>
        /// Convert eight bytes at a specified position in a byte array to a 64-bit signed integer
        /// </summary>
        /// <param name="source">An array of bytes</param>
        /// <param name="startIndex">The starting position within value</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="source"/> is less than 8 in length</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than or equal to the length of <paramref name="source"/> minus 8</exception>
        public static long ToLong(this byte[] source, int startIndex = 0)
        {

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return ToLong(source.AsSpan(), startIndex);
        }

        /// <summary>
        /// Convert eight bytes at a specified position in a byte array to a 64-bit signed integer
        /// </summary>
        /// <param name="source">An array of bytes</param>
        /// <param name="startIndex">The starting position within value</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex"/></returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than or equal to the length of <paramref name="source"/> minus 8</exception>
        public static long ToLong(this Span<byte> source, int startIndex = 0)
        {
            return BitConverter.ToInt64(source.Slice(startIndex));
        }

        /// <summary>
        /// Convert four bytes at a specified position in a byte array to a 32-bit signed integer
        /// </summary>
        /// <param name="source">An array of bytes</param>
        /// <param name="startIndex">The starting position within value</param>
        /// <returns>A 32-bit signed integer formed by eight bytes beginning at <paramref name="startIndex"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than or equal to the length of <paramref name="source"/> minus 4</exception>
        public static int ToInt(this byte[] source, int startIndex = 0)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return ToInt(source.AsSpan(), startIndex);
        }

        /// <summary>
        /// Convert four bytes at a specified position in a byte array to a 32-bit signed integer
        /// </summary>
        /// <param name="source">An array of bytes</param>
        /// <param name="startIndex">The starting position within value</param>
        /// <returns>A 32-bit signed integer formed by eight bytes beginning at <paramref name="startIndex"/></returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than or equal to the length of <paramref name="source"/> minus 4</exception>
        public static int ToInt(this Span<byte> source, int startIndex = 0)
        {
            return BitConverter.ToInt32(source.Slice(startIndex));
        }

        /// <summary>
        /// Convert two bytes at a specified position in a byte array to a 32-bit signed integer
        /// </summary>
        /// <param name="source">An array of bytes</param>
        /// <param name="startIndex">The starting position within value</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex"/></returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentException"><paramref name="startIndex"/> is greater than or equal to the length of <paramref name="source"/> minus 1</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="source"/> minus 2</exception>
        public static short ToShort(this byte[] source, int startIndex = 0)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return ToShort(source.AsSpan(), startIndex);
        }

        /// <summary>
        /// Convert two bytes at a specified position in a byte array to a 32-bit signed integer
        /// </summary>
        /// <param name="source">An array of bytes</param>
        /// <param name="startIndex">The starting position within value</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex"/></returns>
        /// <exception cref="ArgumentException"><paramref name="startIndex"/> is greater than or equal to the length of <paramref name="source"/> minus 1</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="source"/> minus 2</exception>
        public static short ToShort(this Span<byte> source, int startIndex = 0)
        {
            return BitConverter.ToInt16(source.Slice(startIndex));
        }
    }
}
