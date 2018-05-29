using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotAClue
{
	public static class NullableTypes_ExtensionMethods
	{
		/// <summary>
		/// Converts the value fo the current System.DateTime to its equivalent short string representation.
		/// </summary>
		/// <param name="time">The time.</param>
		/// <returns>String.</returns>
		public static String ToShortTime(this DateTime? time)
		{
			if (time.HasValue)
				return ((DateTime)time).ToShortTimeString();

			return "";
		}

		/// <summary>
		/// Determines whether [contains] [the specified values].
		/// </summary>
		/// <param name="values">The values.</param>
		/// <param name="value">The value.</param>
		public static Boolean Contains(this List<int> values, int? value)
		{
			if (value.HasValue)
				return values.Contains((int)value);

			return false;
		}
	}
}
