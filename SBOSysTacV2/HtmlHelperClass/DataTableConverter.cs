using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SBOSysTacV2.HtmlHelperClass
{
    public static class DataTableConverter
    {
        public static DataTable ToDataTableList<T>(this IList<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {

                Type type = GetCoreType(prop.PropertyType);

                tb.Columns.Add(prop.Name, type);

            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        public static DataTable ToDataTable<T>(this T item)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {

                Type type = GetCoreType(prop.PropertyType);

                tb.Columns.Add(prop.Name, type);

            }

            var values = new object[props.Length];

            for (int i = 0; i < props.Length; i++)
            {
                values[i] = props[i].GetValue(item, null);
            }

            tb.Rows.Add(values);

            return tb;
        }


        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>

        public static Type GetCoreType(Type propertyType)
        {
            if (propertyType != null && IsNullableType(propertyType))
            {
                return !propertyType.IsValueType ? propertyType : Nullable.GetUnderlyingType(propertyType);
            }
            else
            {
                return propertyType;
            }

        }

        /// <summary>
        ///  Determine of specified type is nullable
        /// </summary>
        public static bool IsNullableType(Type propertyType)
        {
            return !propertyType.IsValueType || propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}