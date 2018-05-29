using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace NotAClue
{
    public static class ObjectExtensionMethods
    {
        /// <summary>
        /// Calculates the lenght in bytes of an object 
        /// and returns the size 
        /// </summary>
        /// <param name="theObject"></param>
        /// <returns></returns>
        public static int SizeInBytes(this Object theObject)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                byte[] byteArray;
                bf.Serialize(ms, theObject);
                byteArray = ms.ToArray();
                return byteArray.Length;
            }
        }
    }
}
