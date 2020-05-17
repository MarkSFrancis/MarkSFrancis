﻿using System;

namespace Phnx
{
    /// <summary>
    /// Extensions for <see cref="decimal"/>
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Display the <paramref name="value"/> as money
        /// </summary>
        /// <param name="value">The value to display as money</param>
        /// <param name="currencySymbol">The currency symbol of the currency format to display</param>
        /// <param name="symbolBeforeValue">Whether the symbol should be placed before the value (such as $1.23 in the USA), or after (such as 1.23 € in France)</param>
        /// 
        public static string ToMoney(this decimal value, string currencySymbol, bool symbolBeforeValue = true)
        {
            if (currencySymbol is null)
            {
                throw new ArgumentNullException(nameof(currencySymbol));
            }

            var roundedValue = Math.Round(value, 2);

            return symbolBeforeValue ? currencySymbol + roundedValue :
                roundedValue + currencySymbol;
        }

        /// <summary>
        /// Round to the nearest value
        /// </summary>
        /// <param name="valueToRound">The value to be rounded</param>
        /// <param name="toNearest">The value to round to</param>
        /// 
        public static decimal RoundToNearest(this decimal valueToRound, decimal toNearest)
        {
            return Math.Round(valueToRound / toNearest, MidpointRounding.AwayFromZero) * toNearest;
        }
    }
}