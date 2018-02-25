﻿using System;

namespace MarkSFrancis.Extensions.Numeric
{
    /// <summary>
    /// Extension methods for the base type <see cref="double"/>
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Display the <paramref name="value"/> as money
        /// </summary>
        /// <param name="value">The value to display as money</param>
        /// <param name="currencySymbol">The currency symbol of the currency format to display</param>
        /// <param name="symbolBeforeValue">Whether the symbol should be placed before the value (such as $1.23 in the USA), or after (such as 1.23 € in France)</param>
        /// <returns></returns>
        public static string ToMoney(this double value, string currencySymbol, bool symbolBeforeValue = true)
        {
            return symbolBeforeValue ? currencySymbol + Math.Round(value, 2) :
                Math.Round(value, 2) + currencySymbol;
        }
        
        /// <summary>
        /// Round to the nearest value
        /// </summary>
        /// <param name="valueToRound">The value to be rounded</param>
        /// <param name="toNearest">The value to round to</param>
        /// <returns></returns>
        public static double RoundToNearest(this double valueToRound, double toNearest)
        {
            return Math.Round(valueToRound / toNearest, MidpointRounding.AwayFromZero) * toNearest;
        }
    }
}