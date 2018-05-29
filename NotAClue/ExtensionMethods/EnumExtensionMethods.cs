using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
//using NotAClue.ComponentModel.DataAnnotations;

namespace NotAClue
{
    public static class EnumExtensionMethods
    {
        /// <summary>
        /// Enum to int.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.Int32.</returns>
        public static int ToInt<T>(this T enumValue) // where T : struct, IConvertible //where T : Enum
        {
            var value = Convert.ToInt32(enumValue);
            return value;
        }

        /// <summary>
        /// Enum int to string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>String.</returns>
        public static String EnumIntToString<T>(this T enumValue) // where T : struct, IConvertible //where T : Enum
        {
            var value = enumValue.ToInt().ToString();
            return value;
        }

        /// <summary>
        /// Toes the list.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<int> ToList(this Type enumType)
        {
            var result = new List<int>();
            foreach (object enumValue in Enum.GetValues(enumType))
            {
                result.Add((int)enumValue);
            }
            return result;
        }

        /// <summary>
        /// Gets a list of each enum item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>List{``0}.</returns>
        public static List<T> ToList<T>()
        {
            Type enumType = typeof(T);
            var result = new List<T>();
            foreach (T enumValue in Enum.GetValues(enumType))
            {
                result.Add(enumValue);
            }
            return result;
        }

        /// <summary>
        /// Gets the enum names and hex values.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Dictionary<String, String> GetEnumNamesAndHexValues(Type enumType)
        {
            var result = new Dictionary<String, String>();
            foreach (object enumValue in Enum.GetValues(enumType))
            {
                string name = Enum.GetName(enumType, enumValue);
                var value = ("000000" + ((int)enumValue).ToString("X"));
                var hex = "#" + value.Substring(value.Length - 6, 6);
                result.Add(name, hex);
            }
            return result;
        }

        /// <summary>
        /// Gets the enum string from string.
        /// </summary>
        /// <param name="enumString">The enum string.</param>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static String GetEnumStringFromString(this String enumString, Type enumType)
        {
            var value = Enum.Parse(enumType, enumString);
            return value.ToString();
        }

        /// <summary>
        /// Toes the enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString">The enum string.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T ToEnum<T>(this String enumString) // where T : struct, IConvertible //where T : Enum
        {
            var value = Enum.Parse(typeof(T), enumString);
            return (T)value;
        }

        /// <summary>
        /// As the enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString">The enum string.</param>
        /// <returns></returns>
        public static T AsEnum<T>(this String enumString) // where T : struct, IConvertible //where T : Enum
        {
            return enumString.ToEnum<T>();
        }

        /// <summary>
        /// To the enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumInt">The enum int.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T ToEnum<T>(this int enumInt) // where T : struct, IConvertible //where T : Enum
        {
            var value = Enum.Parse(typeof(T), enumInt.ToString());
            return (T)value;
        }

        /// <summary>
        /// As the enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumInt">The enum int.</param>
        /// <returns></returns>
        public static T AsEnum<T>(this int enumInt) // where T : struct, IConvertible //where T : Enum
        {
            return enumInt.ToEnum<T>();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumInt">The enum int.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public static String ToString<T>(this int enumInt) // where T : struct, IConvertible //where T : Enum
        {
            var value = Enum.Parse(typeof(T), enumInt.ToString()).ToString();
            return value;
        }

        /// <summary>
        /// Gets the enum from string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString">The enum string.</param>
        /// <returns></returns>
        public static T GetEnumFromString<T>(this String enumString) // where T : struct, IConvertible //where T : Enum
        {
            var value = Enum.Parse(typeof(T), enumString);
            return (T)value;
        }

        //public static Boolean Contains<T>(this T firstEnum, T secondEnum) //where T : struct
        //{
        //    var value = (firstEnum & secondEnum) == secondEnum;
        //    return value;
        //}

        /// <summary>
        /// Gets the underlying type value string.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="enumValue">The enum value.</param>
        /// <returns></returns>
        public static String GetUnderlyingTypeValueString(Type enumType, object enumValue)
        {
            return Convert.ChangeType(enumValue, Enum.GetUnderlyingType(enumType)).ToString();
        }

        /// <summary>
        /// Gets the enum names and values.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static IOrderedDictionary GetEnumNamesAndValues(this Type enumType)
        {
            OrderedDictionary result = new OrderedDictionary();
            foreach (object enumValue in Enum.GetValues(enumType))
            {
                // TODO: add way to localize the displayed name
                string name = Enum.GetName(enumType, enumValue);
                string value = EnumExtensionMethods.GetUnderlyingTypeValueString(enumType, enumValue);
                result.Add(name, value);
            }
            return result;
        }

        /// <summary>
        /// Gets dictionary of enum names and values.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static Dictionary<String, int> EnumToDictionary(this Type enumType)
        {
            var result = new Dictionary<String, int>();
            foreach (int enumValue in Enum.GetValues(enumType))
            {
                // TODO: add way to localize the displayed name
                string name = Enum.GetName(enumType, enumValue);
                result.Add(name, enumValue);
            }
            return result;
        }

        /// <summary>
        /// Determines whether [is enum type in flags mode] [the specified enum type].
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns>
        /// 	<c>true</c> if [is enum type in flags mode] [the specified enum type]; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsEnumTypeInFlagsMode(this Type enumType)
        {
            return enumType.GetCustomAttributes(typeof(FlagsAttribute), false).Length != 0;
        }

        ///// <summary>
        ///// Gets the enum string from string.
        ///// </summary>
        ///// <param name="enumString">The enum string.</param>
        ///// <param name="enumType">Type of the enum.</param>
        ///// <returns></returns>
        //public static String GetEnumStringFromString(this String enumString, Type enumType)
        //{
        //    var value = Enum.Parse(enumType, enumString);
        //    return value.ToString();
        //}
    }
}
