using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;

namespace NotAClue
{
	public static class DictionaryExtensionMethods
	{
		/// <summary>
		/// Sets the value wheather is already exists or not.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="key">The key.</param>
		public static void SetValue(this IDictionary<String, Object> dictionary, String key, Object value)
		{
			if (dictionary.ContainsKey(key))
				dictionary[key] = value;
			else
				dictionary.Add(key, value);
		}

		/// <summary>
		/// Gets the field value value as.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dictionary">The dictionary.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <returns>T.</returns>
		public static T GetFieldValueValueAs<T>(this IOrderedDictionary dictionary, String parameterName)
		{
			// get and convert value
			var stringValue = (String)dictionary[parameterName];
			if (!String.IsNullOrEmpty(stringValue))
			{
				var value = (T)Convert.ChangeType(stringValue, typeof(T));
				return value;
			}
			return default(T);
		}
	}
}
