using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;
using System.Web;
//using System.Data.SqlClient;

namespace NotAClue
{
	public static class SystemIOExtensionMethods
	{
	   /// <summary>
		/// Saves Byte array to file.
		/// </summary>
		/// <param name="byteArray">The byte array.</param>
		/// <param name="fileName">Name of the _ file.</param>
		/// <returns></returns>
		public static bool SaveToFile(this Byte[] byteArray, String fileName)
		{
			try
			{
				// Open file for reading
				var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);

				// Writes a block of bytes to this stream using data from a byte array.
				fileStream.Write(byteArray, 0, byteArray.Length);

				// close file stream
				fileStream.Close();

				return true;
			}
			catch
			{
				// error occurred, return false
				return false;
			}
		}

		/// <summary>
		/// Loads file into byte array.
		/// </summary>
		/// <param name="fileName">Name of the file.</param>
		/// <returns>Byte[].</returns>
		public static Byte[] LoadFromFile(this string fileName)
		{
			Byte[] buffer = null;

			try
			{
				// Open file for reading
				var fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

				// attach file stream to binary reader
				var binaryReader = new System.IO.BinaryReader(fileStream);

				// get total byte length of the file
				var totalBytes = new System.IO.FileInfo(fileName).Length;

				// read entire file into buffer
				buffer = binaryReader.ReadBytes((Int32) totalBytes);

				// close file reader
				fileStream.Close();
				fileStream.Dispose();
				binaryReader.Close();
			}
			catch (Exception exception)
			{
				var a = exception;
			}

			return buffer;
		}


		/// <summary>
		/// Deletes all files in directory.
		/// </summary>
		/// <param name="directoryInfo">The directory info.</param>
		/// <returns></returns>
		public static Boolean DeleteAllFilesInDirectory(this DirectoryInfo directoryInfo)
		{
			// delete files in upload folder
			var files = directoryInfo.GetFiles();
			if (files.Count() > 0)
			{
				try
				{
					foreach (var file in files)
					{
						File.Delete(file.FullName);
					}
				}
				catch (Exception)
				{
					return false;
				}
			}
			return true;
		}
	}
}
