using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using System.ComponentModel;

namespace System.Data
{
    public static class LinqExtensionMethods
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            if (data != null) // || data.Count == 0)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
                DataTable table = new DataTable(typeof(T).Name);
                foreach (PropertyDescriptor prop in properties)
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

                foreach (T item in data)
                {
                    DataRow row = table.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
                return table;
            }

            return null;
        }

        /// <summary>
        /// To the data table.
        /// </summary>
        /// <typeparam name="Ts">The type of the source collection.</typeparam>
        /// <typeparam name="Td">The type of the destination collection.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="map">The property name mappings map&ltnSource,Destination&gtr.</param>
        /// <returns>DataTable.</returns>
        public static DataTable ToDataTable<Ts,Td>(this IList<Ts> data, Dictionary<String,String> map)
        {
            if (data != null) // || data.Count == 0)
            {
                var desctProperties = TypeDescriptor.GetProperties(typeof(Td));
                var table = new DataTable(typeof(Td).Name);
                foreach (PropertyDescriptor prop in desctProperties)
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }

                var sourceProperties = TypeDescriptor.GetProperties(typeof(Ts));
                foreach (Ts item in data)
                {
                    var row = table.NewRow();
                    foreach (PropertyDescriptor prop in sourceProperties)
                    {
                        row[map[prop.Name]] = prop.GetValue(item) ?? DBNull.Value;
                    }

                    table.Rows.Add(row);
                }
                return table;
            }

            return null;
        }
    }
}