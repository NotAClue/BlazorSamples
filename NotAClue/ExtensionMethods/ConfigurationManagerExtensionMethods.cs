using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;

namespace NotAClue
{
	public static class ConfigurationManagerExtensionMethods
	{
		/// <summary>
		/// Gets the value as type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="nameValuePairs">The name value pairs collection.</param>
		/// <param name="configKey">The configuration key.</param>
		/// <param name="defaultValue">The default value (optional).</param>
		/// <returns>T.</returns>
		public static T GetValueAs<T>(this NameValueCollection config, String configKey, T defaultValue = default(T)) where T : IConvertible
		{
			if (!String.IsNullOrEmpty(config[configKey]))
			{
				if (typeof(T).IsEnum)
				{
					T value = config[configKey].ToEnum<T>();
					return value;
				}
				else
				{
					var value = config[configKey].To<T>();
					if (value != null)
						return value;
				}
			}

			return defaultValue;
		}

		///// <summary>
		///// Gets the value as type T.
		///// </summary>
		///// <typeparam name="T"></typeparam>
		///// <param name="nameValuePairs">The name value pairs collection.</param>
		///// <param name="configKey">The configuration key.</param>
		///// <param name="defaultValue">The default value (optional).</param>
		///// <returns>T.</returns>
		//public static T GetValueAs<T>(this NameValueCollection nameValuePairs, string configKey, T defaultValue = default(T)) where T : IConvertible
		//{
		//	T retValue = defaultValue;

		//	if (nameValuePairs.AllKeys.Contains(configKey) && !String.IsNullOrEmpty(nameValuePairs[configKey]))
		//	{
		//		var tmpValue = nameValuePairs[configKey];

		//		retValue = (T)Convert.ChangeType(tmpValue, typeof(T));
		//	}

		//	return retValue;
		//}
	}
}