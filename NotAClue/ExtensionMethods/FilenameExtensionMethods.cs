using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;

namespace System
{
    public static class FilenameExtensionMethods
    {
        /// <summary>
        /// Gets the file extension from a string.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>file extension of the filename</returns>
        public static String GetFileExtension(this String fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
                return fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();

            return string.Empty;
        }

        /// <summary>
        /// Replaces the file extension.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="newExt">The new ext.</param>
        /// <returns></returns>
        public static String ReplaceFileExtension(this String fileName, String newExt)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                var ext = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
                return fileName.Replace(ext, newExt);
            }

            return String.Empty;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static String GetFileName(this String fileName)
        {
            String pathSeperator = fileName.GetPathSeperator();
            return fileName.Substring(fileName.LastIndexOf(pathSeperator) + 1);
        }

        /// <summary>
        /// Gets the path seperator.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static String GetPathSeperator(this String fileName)
        {
            String pathSeperator;

            if (fileName.Contains("\\"))
                pathSeperator = "\\";
            else
                pathSeperator = "/";

            return pathSeperator;
        }
    }
}
