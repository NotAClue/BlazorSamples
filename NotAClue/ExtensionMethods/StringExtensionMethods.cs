using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using Inflector;

namespace NotAClue
{
    public static class StringExtensionMethods
    {
        static Random _random = new Random();

        #region Properties
        private static Dictionary<string, string> _pluralOverrides = new Dictionary<string, string>();
        /// <summary>
        /// Gets or sets the replaceable words.
        /// </summary>
        /// <value>The replaceable words.</value>
        public static Dictionary<string, string> PluralOverrides
        {
            get { return _pluralOverrides; }
            set { _pluralOverrides = value; }
        }

        private static Dictionary<string, string> _singularOverrides = new Dictionary<string, string>();
        /// <summary>
        /// Gets or sets the replaceable words.
        /// </summary>
        /// <value>The replaceable words.</value>
        public static Dictionary<string, string> SingularOverrides
        {
            get { return _singularOverrides; }
            set { _singularOverrides = value; }
        }

        private static Dictionary<string, string> _replaceableWords = new Dictionary<string, string>();
        /// <summary>
        /// Gets or sets the replaceable words.
        /// </summary>
        /// <value>The replaceable words.</value>
        public static Dictionary<string, string> ReplaceableWords
        {
            get { return _replaceableWords; }
            set { _replaceableWords = value; }
        }

        private static List<string> _removableWords = new List<string>();
        /// <summary>
        /// Gets or sets the removable words.
        /// </summary>
        /// <value>The removable words.</value>
        public static List<string> RemovableWords
        {
            get { return _removableWords; }
            set { _removableWords = value; }
        }
        #endregion

        /// <summary>
        /// Pluralize the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>System.String.</returns>
        public static string ToPlural(this string text)
        {
            if (_pluralOverrides.ContainsKey(text))
                return _pluralOverrides[text];

            return text.Pluralize();
        }

        /// <summary>
        /// Singularize the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>System.String.</returns>
        public static string ToSingular(this string text)
        {
            if (_singularOverrides.ContainsKey(text))
                return _singularOverrides[text];

            return text.Singularize();
        }

        /// <summary>
        /// Gets the string as <typeparam name="T"></typeparam>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>T</returns>
        public static T GetValueAs<T>(this string value)
        {
            var tempValue = (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return tempValue;
        }

        public static string RandomizeString(this string text)
        {
            var arr = text.ToCharArray();
            var list = new List<KeyValuePair<int, char>>();

            // Add all chars from array
            // Add new random int each time
            foreach (char s in arr)
            {
                list.Add(new KeyValuePair<int, char>(_random.Next(), s));
            }
            // Sort the list by the random number
            var sorted = from item in list
                         orderby item.Key
                         select item;

            // Allocate new char array
            var result = new char[arr.Length];
            // Copy values to array
            int index = 0;
            foreach (KeyValuePair<int, char> pair in sorted)
            {
                result[index] = pair.Value;
                index++;
            }
            // Return copied array
            return new string(result);
        }

        /// <summary>
        /// To the specified <T> type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The text.</param>
        /// <returns></returns>
        public static T To<T>(this string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Gets the right most 'n' characters from the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The index.</param>
        /// <returns></returns>
        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length, length);
        }

        /// <summary>
        /// Gets the left most 'n' characters from the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The index.</param>
        /// <returns></returns>
        public static string Left(this string value, int length)
        {
            return value.Substring(0, length);
        }

        /// <summary>
        /// Gets the Mid string specified by the parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Mid(this string value, int start, int length)
        {
            return value.Substring(start, length);
        }

        /// <summary>
        /// Truncates at last occurrence of separator.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="minEntries">The min entries before truncate occurs.</param>
        /// <returns></returns>
        public static string TruncateAtLast(this string value, char separator, int minEntries = 1)
        {
            if (value.Count(c => c == separator) > minEntries)
            {
                var remanent = value.Substring(0, value.LastIndexOf(separator));
                return remanent;
            }
            return value;
        }

        /// <summary>
        /// STRs to byte array.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>string as an array of bytes</returns>
        public static byte[] StrToByteArray(this string str)
        {
            var encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// Toes the title from pascal.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string ToTitleFromPascal(this string text)
        {
            // remove name space
            string s0 = Regex.Replace(text, "(.*\\.)(.*)", "$2");
            // add space before Capital letter
            string s1 = Regex.Replace(s0, "[A-Z]", " $&");

            // replace '_' with space
            string s2 = Regex.Replace(s1, "[_]", " ");

            // replace double space with single space
            string s3 = Regex.Replace(s2, "  ", " ");

            // remove and double capitals with inserted space
            string s4 = Regex.Replace(s3, "(?<before>[A-Z])\\s(?<after>[A-Z])", "${before}${after}");

            // remove and double capitals with inserted space
            string sf = Regex.Replace(s4, "^\\s", "");

            var wordsRemoved = sf.RemoveWords();

            var titleCased = wordsRemoved.ToTitleCase();

            var wordsReplaced = titleCased.ReplaceWords();

            return wordsReplaced;
        }

        /// <summary>Changes the texts case from Pascal Case to Lisp/Kebab Case.</summary>
        /// <param name="text">The text.</param>
        /// <returns>string.</returns>
        public static string ToSnakeCase(this string text)
        {
            var titleCased = text.ToTitleFromPascal();
            return titleCased.Replace(" ", "_").ToLower();
        }

        /// <summary>Changes the texts case from Pascal Case to Kebab/Lisp Case.</summary>
        /// <param name="text">The text.</param>
        /// <returns>string.</returns>
        public static string ToKebabCase(this string text)
        {
            var titleCased = text.ToTitleFromPascal();
            return titleCased.Replace(" ", "-").ToLower();
        }

        /// <summary>Changes the texts case from Pascal Case to Lisp/Kebab Case.</summary>
        /// <param name="text">The text.</param>
        /// <returns>string.</returns>
        public static string ToLispCase(this string text)
        {
            return text.ToKebabCase();
        }

        /// <summary>Changes the texts case from Pascal Case to Camel Case.</summary>
        /// <param name="text">The text.</param>
        /// <returns>string.</returns>
        public static string ToCamelCase(this string text)
        {
            if (!text.ContainsAny(new List<string>() { "-", "_", " " }))
                text = text.SplitByUppercaseLetter();

            return Char.ToLowerInvariant(text[0]) + text.Substring(1);
        }

        /// <summary>
        /// To Pascal Case from Snake or Kebab Case.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>string.</returns>
        public static string ToPascalCase(this string text)
        {
            if (!text.ContainsAny(new List<string>() { "-", "_", " " }))
                text = text.SplitByUppercaseLetter();

            var pascalText = new StringBuilder();
            var words = text.Split(new char[] { '-', '_', ' ' });
            foreach (var word in words)
            {
                var cleanedWord = word; //.Replace("\"", "").Trim();
                pascalText.Append(Char.ToUpperInvariant(cleanedWord[0]) + cleanedWord.Substring(1));
            }
            return pascalText.ToString();
        }

        public static string GetPascalCaps(this string text)
        {
            var words = text.SplitByUppercaseLetter().Split(new char[] { '-', '_', ' ' });

            var pascalCaps = new StringBuilder();
            foreach (var word in words)
            {
                pascalCaps.Append(Char.ToUpperInvariant(word[0]));
            }
            return pascalCaps.ToString();
        }


        public static string SplitByUppercaseLetter(this string s)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return r.Replace(s, "_").ToLower();
        }

        /// <summary>
        /// Replaces words in the replaceable words dictionary.
        /// </summary>
        /// <param name="theText">The text.</param>
        /// <returns></returns>
        private static string ReplaceWords(this string value)
        {
            string newValue = value;
            // replace words in friendly name
            if (ReplaceableWords.Count > 0)
            {
                foreach (var word in ReplaceableWords)
                {
                    if (newValue.Contains(word.Key))
                        newValue = newValue.Replace(word.Key, word.Value);
                }
            }
            return newValue;
        }

        ///// <summary>
        ///// Replaces words in the replaceable words dictionary.
        ///// </summary>
        ///// <param name="theText">The text.</param>
        ///// <returns></returns>
        //private static string ReplaceWords(this string value)
        //{
        //    string newValue = value;
        //    // replace words in friendly name
        //    if (ReplaceableWords.Count > 0)
        //    {
        //        foreach (var word in ReplaceableWords)
        //        {
        //            if (newValue.IndexOf(word.Key, StringComparison.Ordinal) > 0)
        //                newValue = newValue.Replace(word.Key, word.Value);
        //        }
        //    }
        //    return newValue;
        //}

        /// <summary>
        /// Removes words that are in the removable words list.
        /// </summary>
        /// <param name="theText">The text.</param>
        /// <returns></returns>
        private static string RemoveWords(this string theText)
        {
            // replace words in friendly name
            if (RemovableWords.Count() > 0)
            {
                foreach (var removableWord in RemovableWords)
                {
                    var word = removableWord + " ";
                    if (theText.Contains(word))
                        theText = theText.Replace(word, string.Empty);
                }
            }
            // return text and replace any double spaces with single
            return theText.ToString().Replace("  ", " ").Trim();
        }

        /// <summary>
        /// Determines whether the specified STR1 contains any.
        /// </summary>
        /// <param name="str1">The STR1.</param>
        /// <param name="str2">The STR2.</param>
        /// <returns>
        /// 	<c>true</c> if the specified STR1 contains any; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean ContainsAny(this string[] str1, params string[] str2)
        {
            // call extension method to convert array to lower case for compare
            var lowerCaseStr1 = str1.AllToLower();
            foreach (string str in str2)
            {
                if (lowerCaseStr1.Contains(str.ToLower()))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified chars contains any.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="chars">The chars.</param>
        /// <returns><c>true</c> if the specified chars contains any; otherwise, <c>false</c>.</returns>
        public static bool ContainsAny(this string text, Char[] chars)
        {
            foreach (char ch in text.ToCharArray())
            {
                if (chars.Contains<char>(ch))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified list to match contains any.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="listToMatch">The list to match.</param>
        /// <returns><c>true</c> if the specified list to match contains any; otherwise, <c>false</c>.</returns>
        public static bool ContainsAny(this string text, IEnumerable<string> listToMatch)
        {
            foreach (var toMatch in listToMatch)
            {
                if (text.Contains(toMatch))
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Equals any values in the list.
        /// </summary>
        /// <param name="str1">The STR1.</param>
        /// <param name="str2">The STR2.</param>
        /// <returns>True if str1 occurs in the array str2</returns>
        /// <remarks></remarks>
        public static Boolean EqualsAny(this string str1, params string[] str2)
        {
            // call extension method to convert array to lower case for compare
            var lowerCaseStr1 = str1.ToLower();
            foreach (string str in str2)
            {
                if (lowerCaseStr1 == str.ToLower())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Appends the name of to end of file.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="stringToAppend">The string to append.</param>
        /// <returns></returns>
        public static string AppendToEndOfFileName(this string text, string stringToAppend)
        {
            var parts = text.Split(new char[] { '.' });
            if (parts.Length > 2)
                throw new InvalidOperationException("too many '.' in filename");
            return string.Format("{0}{1}.{2}", parts[0], stringToAppend, parts[1]);
        }

        /// <summary>
        /// converts a comma separated values string to array.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A string array</returns>
        public static string[] CsvToArray(this string text)
        {
            return text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Comma separated values (CSV) to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<T> CsvToList<T>(this string text)
        {
            var values = new List<T>();
            var tempValues = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var v in tempValues)
            {
                T val = (T)Convert.ChangeType(v, typeof(T), CultureInfo.InvariantCulture);
                values.Add(val);
            }
            return values;
        }

        /// <summary>
        /// Converts a string of 'comma' separated values to an array.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] ToArray(this string text, char separator = ',')
        {
            if (text.Contains(separator))
                return text.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            else
                return new string[1] { text };
        }

        /// <summary>
        /// To the array.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static List<string> ToList(this string text, char separator = ',')
        {
            if (!string.IsNullOrEmpty(text))
                return text.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return new List<string>();
        }

        /// <summary>
        /// Comma separated values (CSV) to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<T> ToListOfType<T>(this string text, char separator = ',')
        {
            var values = new List<T>();
            if (!string.IsNullOrEmpty(text))
            {
                var tempValues = text.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var v in tempValues)
                {
                    T val = (T)Convert.ChangeType(v, typeof(T), CultureInfo.InvariantCulture);
                    values.Add(val);
                }
            }
            return values;
        }

        /// <summary>
        /// Returns a copy of the array of string 
        /// all in lowercase
        /// </summary>
        /// <param name="strings">Array of strings</param>
        /// <returns>array of string all in lowercase</returns>
        public static string[] AllToLower(this string[] strings)
        {
            string[] temp = new string[strings.Count()];
            for (int i = 0; i < strings.Count(); i++)
            {
                temp[i] = strings[i].ToLower();
            }
            return temp;
        }

        /// <summary>
        /// Sets all characters in a List to lower case.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <returns></returns>
        public static List<string> AllToLower(this List<string> strings)
        {
            var temp = new List<string>();
            foreach (var item in strings)
            {
                temp.Add(item.ToLower());
            }
            return temp;
        }

        /// <summary>
        /// Toes the title case.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string ToTitleCase(this string text)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(text))
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (i > 0)
                    {
                        if (text.Substring(i - 1, 1) == " " ||
                            text.Substring(i - 1, 1) == "\t" ||
                            text.Substring(i - 1, 1) == "/")
                            sb.Append(text.Substring(i, 1).ToUpper());
                        else
                            sb.Append(text.Substring(i, 1).ToLower());
                    }
                    else
                        sb.Append(text.Substring(i, 1).ToUpper());
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Toes the sentence case.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string ToSentenceCase(this string text)
        {
            return text.Substring(0, 1).ToUpper() + text.Substring(1, text.Length - 1).ToLower();
        }

        /// <summary>
        /// Gets the display format.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetDisplayFormat(this Type type)
        {
            string defaultFormat = "{0}";

            if (type == typeof(DateTime) || type == typeof(Nullable<DateTime>))
            {
                defaultFormat = "{0:d}";
            }
            else if (type == typeof(decimal) || type == typeof(Nullable<decimal>))
            {
                defaultFormat = "{0:c}";
            }

            return defaultFormat;
        }

        /// <summary>
        /// Converts a string separated by separators '|' and '=' by default to a Dictionary<string, string>.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="ItemSeparator">The item separator default is '|'.</param>
        /// <param name="ValueSeparator">The value separator default is '='.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Dictionary<string, string> ToDictionary(this string values, char ItemSeparator = '|', char ValueSeparator = '=')
        {
            var dictionary = new Dictionary<string, string>();

            var items = values.Split(new char[] { ItemSeparator });
            if (items.Length > 0)
            {
                foreach (var item in items)
                {
                    var valuesList = item.Split(new char[] { ValueSeparator });

                    if (valuesList.Length == 2)
                        dictionary.Add(valuesList[0], valuesList[1]);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Converts a string to an int, if the string is not an int then in returns 0.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        public static int ToInt(this string value)
        {
            var intValue = 0;
            int.TryParse(value, out intValue);
            return intValue;
        }
    }
}
