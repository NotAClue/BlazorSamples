using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotAClue
{
    public static class IntExtensionMethods
    {
        /// <summary>
        /// Determines whether the specified value is odd.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is odd; otherwise, <c>false</c>.</returns>
        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }

        /// <summary>
        /// Determines whether the specified value is odd.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is odd; otherwise, <c>false</c>.</returns>
        public static bool IsOdd(this int? value)
        {
            if (value.HasValue)
                return ((int)value).IsOdd();

            return false;
        }
    }
}
