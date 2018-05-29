using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NotAClue
{
	public static class ReflectionExtensionMethods
	{
		/// <summary>
		/// Gets the property value.
		/// </summary>
		/// <param name="sourceObject">The source object.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns>The named properties value.</returns>
		public static Object GetPropertyValue(this Object sourceObject, string propertyName)
		{
			if (sourceObject != null)
			{
				try
				{
					var value = sourceObject.GetType()
						.GetProperty(propertyName)
						.GetValue(sourceObject, null);

					return value;
				}
				catch (Exception)
				{
					return null;
				}
			}
			else
				return null;
		}

		/// <summary>
		/// Gets the value of a non-public field of an object instance. Must have Reflection permission.
		/// </summary>
		/// <param name="container">The object whose field value will be returned.</param>
		/// <param name="fieldName">The name of the data field to get.</param>
		/// <remarks>Code initially provided by LonelyRollingStar.</remarks>
		public static object GetPrivateField(this Object container, String fieldName)
		{
			Type type = container.GetType();
			FieldInfo fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			return (fieldInfo == null ? null : fieldInfo.GetValue(container));
		}

		/// <summary>
		/// Determines whether the specified object has the named method.
		/// </summary>
		/// <param name="objectToCheck">The object to check.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <returns><c>true</c> if the specified object to check has method; otherwise, <c>false</c>.</returns>
		public static bool HasMethod(this object objectToCheck, string methodName)
		{
			var type = objectToCheck.GetType();
			return type.GetMethod(methodName) != null;
		}
	}
}
