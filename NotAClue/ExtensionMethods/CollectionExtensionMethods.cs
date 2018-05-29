using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;

namespace NotAClue
{
    public static class CollectionExtensionMethods
    {
        public static IDictionary<String, Object> AddUpdateValue(this IDictionary<String, Object> values, String key, Object value)
        {
            if (values.ContainsKey(key))
                values[key] = value;
            else
                values.Add(key, value);

            return values;
        }

        /// <summary>
        /// Determines whether [has any role] [the specified user roles].
        /// </summary>
        /// <param name="userRoles">The user roles.</param>
        /// <param name="roles">The roles.</param>
        public static Boolean HasAnyRole(this String[] userRoles, String[] roles)
        {
            // call extension method to convert array to lower case for compare
            var lowerCaseUserRoles = userRoles.AllToLower();
            foreach (String role in roles)
            {
                if (lowerCaseUserRoles.Contains(role.ToLower()))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether [has any role] [the specified user roles].
        /// </summary>
        /// <param name="userRoles">The user roles.</param>
        /// <param name="roles">The roles.</param>
        public static Boolean HasAnyRole(this List<String> userRoles, String[] roles)
        {
            return userRoles.ToArray().HasAnyRole(roles);
        }

        /// <summary>
        /// Determines whether [has any role] [the specified user roles].
        /// </summary>
        /// <param name="userRoles">The user roles.</param>
        /// <param name="roles">The roles.</param>
        public static Boolean HasAnyRole(this String[] userRoles, List<String> roles)
        {
            return userRoles.HasAnyRole(roles.ToArray());
        }

        /// <summary>
        /// Determines whether [has any role] [the specified user roles].
        /// </summary>
        /// <param name="userRoles">The user roles.</param>
        /// <param name="roles">The roles.</param>
        public static Boolean HasAnyRole(this List<String> userRoles, List<String> roles)
        {
            return userRoles.ToArray().HasAnyRole(roles.ToArray());
        }

        /// <summary>
        /// Determines whether the specified list1 contains any items from list2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1">The list1.</param>
        /// <param name="list2">The list2.</param>
        /// <returns><c>true</c> if the specified list1 contains any; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool ContainsAny<T>(this List<T> list1, List<T> list2)
        {
            if ((list1 != null) && (list1.Count != 0))
            {
                if ((list2 == null) || (list2.Count == 0))
                {
                    return false;
                }
                foreach (T local in list2)
                {
                    if (list1.Contains(local))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Adds an entry to the dictionary.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="WizardValues">The wizard values.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <remarks></remarks>
        public static void AddEntry<K, T>(this Dictionary<K, T> WizardValues, K key, T value)
        {
            if (WizardValues == null)
                WizardValues = new Dictionary<K, T>();

            if (WizardValues.ContainsKey(key))
                WizardValues[key] = value;
            else
                WizardValues.Add(key, value);
        }

        /// <summary>
        /// Toes the comma separated string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static String ToCommaSeparatedString<T>(this List<T> list)
        {
            var toString = new StringBuilder();
            foreach (var item in list)
            {
                toString.Append(item.ToString() + ",");
            }
            return toString.ToString().Substring(0, toString.Length - 1);
        }

        /// <summary>
        /// Toes the comma separated string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static String ToCommaSeparatedString<T>(this ReadOnlyCollection<T> list)
        {
            var toString = new StringBuilder();
            foreach (var item in list)
            {
                toString.Append(item.ToString() + ",");
            }
            return toString.ToString().Substring(0, toString.Length - 1);
        }

        public static String ToCSV<T>(this IEnumerable<T> list)
        {
            var toString = new StringBuilder();
            foreach (var item in list)
            {
                toString.Append(item.ToString() + ",");
            }
            return toString.ToString().Substring(0, toString.Length - 1);
        }
    }
}
