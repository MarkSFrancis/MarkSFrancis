﻿using System;

namespace Phnx
{
    /// <summary>
    /// Extensions for <see cref="byte"/>
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Round to the nearest value
        /// </summary>
        /// <param name="valueToRound">The value to be rounded</param>
        /// <param name="toNearest">The value to round to</param>
        /// <returns></returns>
        public static byte RoundToNearest(this byte valueToRound, byte toNearest)
        {
            return (byte)(Math.Round((decimal)valueToRound / toNearest, MidpointRounding.AwayFromZero) * toNearest);
        }

        /// <summary>
        /// Converts the byte to a hex code equivalent
        /// </summary>
        /// <param name="b">The byte to convert to hex</param>
        /// <returns>The equivalent hex code</returns>
        public static string ToHex(this byte b)
        {
            return BitConverter.ToString(new[] { b });
        }

        /// <summary>
        /// Converts the bytes to a hex code equivalent
        /// </summary>
        /// <param name="b">The bytes to convert to hex</param>
        /// <param name="removeTrailingZeros">Whether to automatically remove trailing zeroes from the resulting hex</param>
        /// <returns>The equivalent hex code</returns>
        public static string ToHex(this byte[] b, bool removeTrailingZeros = false)
        {
            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var converted = BitConverter.ToString(b).Replace("-", string.Empty);

            return removeTrailingZeros ? converted.TrimEnd('0') : converted;
        }
    }
}