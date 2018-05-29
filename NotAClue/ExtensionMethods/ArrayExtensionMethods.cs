using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace NotAClue
{
	public static class ArrayExtensionMethods
	{
		public static IDictionary<String, T> ToDictionary<T>(this Object[] array) // where T : ValueType
		{
			var dictionary = new Dictionary<String, T>();
			if ((array != null) && (array.Length != 0))
			{
				if ((array.Length % 2) != 0)
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Need even number of control parameters.", new T[0]));

				for (int i = 0; i < array.Length; i += 2)
				{
					var key = array[i] as String;
					var value = (T)array[i + 1];

					if (String.IsNullOrEmpty(key))
						throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Control parameter key '{0}' is not a string.", key));

					if (dictionary.ContainsKey(key))
						throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Control parameter key '{0}' occurs more than once.", key));

					dictionary[key] = value;
				}
			}
			return dictionary;
		}

		/// <summary>
		/// Converts a CSV to a List of type int.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		public static List<int> ToIntList(this String list)
		{
			var invoices = new List<int>();

			// split string
			var csvList = list.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

			// extract ints to of List<int>
			foreach (var invoiceId in csvList)
			{
				int value;
				if (int.TryParse(invoiceId, out value))
					invoices.Add(value);
			}
			return invoices;
		}

		/// <summary>
		/// Toes the CSV string.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		public static String ToCsvString<T>(this List<T> list)
		{
			var toString = new StringBuilder();
			foreach (var item in list)
			{
				toString.Append(item.ToString() + ",");
			}
			return toString.ToString().Substring(0, toString.Length - 1);
		}

		/// <summary>
		/// Toes the CSV string.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		public static String ToCsvString<T>(this ReadOnlyCollection<T> list)
		{
			var toString = new StringBuilder();
			foreach (var item in list)
			{
				toString.Append(item.ToString() + ",");
			}
			return toString.ToString().Substring(0, toString.Length - 1);
		}

		/// <summary>
		/// Toes the CSV string.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		public static string ToCsvString<T>(this T[] list, bool addSingleQuotes = false)
		{
			StringBuilder builder = new StringBuilder();
			foreach (T local in list)
			{
				if (addSingleQuotes)
					builder.Append("'");

				builder.Append(local.ToString() + ",");

				if (addSingleQuotes)
					builder.Append("'");
			}
			return builder.ToString().Substring(0, builder.Length - 1);
		}
	}
}
