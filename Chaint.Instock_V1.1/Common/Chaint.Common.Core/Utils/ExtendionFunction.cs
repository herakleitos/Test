using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chaint.Common.Core.Log;
using DevExpress.DataAccess.Sql.DataApi;
namespace Chaint.Common.Core.Utils
{
    public static class ExtendionFunction
    {
        public static bool IsNullOrEmptyOrWhiteSpace(this string value)
        {
            if (value == null) return true;
            if (value.Trim().Length <= 0) return true;
            return false;
        }
        public static bool IsEmpty<TValue>(this IEnumerable<TValue> value)
        {
            if (value == null) return true;
            if (value.Count() <= 0) return true;
            return false;
        }
        public static bool IsEmpty<TValue>(this ICollection<TValue> value)
        {
            if (value == null) return true;
            if (value.Count <= 0) return true;
            return false;
        }
        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> value)
        {
            if (value == null) return true;
            if (value.Count <= 0) return true;
            return false;
        }
        public static bool IsEmpty(this DataTable value)
        {
            if (value == null) return true;
            if (value.Rows.Count <= 0) return true;
            return false;
        }
        public static bool IsEmpty<TValue>(this IList<TValue> value)
        {
            if (value == null) return true;
            if (value.Count <= 0) return true;
            return false;
        }
        public static bool EqualIgnorCase(this string item1, string item2)
        {
            return item1.ToUpperInvariant().Equals(item2.ToUpperInvariant());
        }
        public static bool IsEmpty(this ITable table)
        {
            if (table == null || table.Count() <= 0) return true;
            return false;
        }
        public static T Clone<T>(this T value)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, value);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)bf.Deserialize(ms);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return default(T);
            }
        }
        public static T GetValue<T>(this DataRow row, string name)
        {
            try
            {
                object value = row[name];
                if (value == DBNull.Value)
                {
                    return default(T);
                }
                if (value is T)
                {
                    return (T)value;
                }
                else
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return default(T);

            }
        }
        public static DataRow FindRow(this DataRowCollection rows, string columnName, object value)
        {
            foreach (DataRow item in rows)
            {
                if (item[columnName].Equals(value)) return item;
            }
            return null;
        }
    }
}
