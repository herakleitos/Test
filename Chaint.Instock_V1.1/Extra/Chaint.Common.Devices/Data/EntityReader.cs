using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;



/// <summary>  
/// ʵ���Ķ����࣬���Դ�DataTable�л���DbDataReader��ʵ���н�����ת���ɶ�Ӧ��ʾ��  
/// ���ߣ�����ܹ�  
/// ���ڣ�2011-07-17  
/// �޸����ڣ�2011-07-21  
/// ���͵�ַ��http://blog.csdn.net/zhoufoxcn ��http://zhoufoxcn.blog.51cto.com  
/// 
/// ʹ�÷�����List<ʵ����> list6 = EntityReader.GetEntities<ʵ����>(��ʵ�����Ӧ�����ݱ�);  
/// 
/// </summary>  
namespace Chaint.Common.Devices.Data
{

    public sealed class EntityReader
    {
        private const BindingFlags BindingFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        //����������������еĿ�д��δ����������֮�佨��ӳ��  
        private static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyMappings = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        //�洢Nullable<T>��T�Ķ�Ӧ��ϵ  
        private static Dictionary<Type, Type> genericTypeMappings = new Dictionary<Type, Type>();

        static EntityReader()
        {
            genericTypeMappings.Add(typeof(Byte?), typeof(Byte));
            genericTypeMappings.Add(typeof(SByte?), typeof(SByte));
            genericTypeMappings.Add(typeof(Char?), typeof(Char));
            genericTypeMappings.Add(typeof(Boolean?), typeof(Boolean));
            genericTypeMappings.Add(typeof(Guid?), typeof(Guid));
            genericTypeMappings.Add(typeof(Int16), typeof(Int16));
            genericTypeMappings.Add(typeof(UInt16), typeof(UInt16));
            genericTypeMappings.Add(typeof(Int32), typeof(Int32));
            genericTypeMappings.Add(typeof(UInt32), typeof(UInt32));
            genericTypeMappings.Add(typeof(Int64), typeof(Int64));
            genericTypeMappings.Add(typeof(UInt64), typeof(UInt64));
            genericTypeMappings.Add(typeof(Single), typeof(Single));
            genericTypeMappings.Add(typeof(Double), typeof(Double));
            genericTypeMappings.Add(typeof(Decimal), typeof(Decimal));
            genericTypeMappings.Add(typeof(DateTime), typeof(DateTime));
            genericTypeMappings.Add(typeof(TimeSpan), typeof(TimeSpan));
            genericTypeMappings.Add(typeof(Enum), typeof(Enum));

        }
        /// <summary>  
        /// ��DataTable�е���������ת����List&gt;T&lt;����  
        /// </summary>  
        /// <typeparam name="T">DataTable��ÿ�����ݿ���ת������������</typeparam>  
        /// <param name="dataTable">�����п���ת������������T�����ݼ���</param>  
        /// <returns></returns>  
        public static List<T> GetEntities<T>(DataTable dataTable) where T : new()
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException("dataTable");
            }
            //���T���������������������ַ�����ValueType������Nullable<ValueType>  
            if (typeof(T) == typeof(string) || typeof(T).IsValueType)
            {
                return GetSimpleEntities<T>(dataTable);
            }
            else
            {
                return GetComplexEntities<T>(dataTable);
            }
        }
        /// <summary>  
        /// ��DbDataReader�е���������ת����List&gt;T&lt;����  
        /// </summary>  
        /// <typeparam name="T">DbDataReader��ÿ�����ݿ���ת������������</typeparam>  
        /// <param name="dataTable">�����п���ת������������T��DbDataReaderʵ��</param>  
        /// <returns></returns>  
        public static List<T> GetEntities<T>(DbDataReader reader) where T : new()
        {
            List<T> list = new List<T>();
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            //���T���������������������ַ�����ValueType������Nullable<ValueType>  
            if (typeof(T) == typeof(string) || typeof(T).IsValueType)
            {
                return GetSimpleEntities<T>(reader);
            }
            else
            {
                return GetComplexEntities<T>(reader);
            }

        }
        /// <summary>  
        /// ��DataTable�н�ÿһ�еĵ�һ��ת����T���͵�����  
        /// </summary>  
        /// <typeparam name="T">Ҫת����Ŀ����������</typeparam>  
        /// <param name="dataTable">�����п���ת������������T�����ݼ���</param>  
        /// <returns></returns>  
        private static List<T> GetSimpleEntities<T>(DataTable dataTable) where T : new()
        {
            List<T> list = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add((T)GetValueFromObject(row[0], typeof(T)));
            }
            return list;
        }
        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <param name="targetType">Ҫת����Ŀ����������</param>  
        /// <returns></returns>  
        private static object GetValueFromObject(object value, Type targetType)
        {
            if (targetType == typeof(string))//���Ҫ��valueת����string����  
            {
                return GetString(value);
            }
            else if (targetType.IsGenericType)//���Ŀ�������Ƿ���  
            {
                return GetGenericValueFromObject(value, targetType);
            }
            else//����ǻ����������ͣ�������ֵ���͡�ö�ٺ�Guid��  
            {
                return GetNonGenericValueFromObject(value, targetType);
            }
        }

        /// <summary>  
        /// ��DataTable�ж�ȡ�����������ͼ���  
        /// </summary>  
        /// <typeparam name="T">Ҫת����Ŀ����������</typeparam>  
        /// <param name="dataTable">�����п���ת������������T�����ݼ���</param>  
        /// <returns></returns>  
        private static List<T> GetComplexEntities<T>(DataTable dataTable) where T : new()
        {
            if (!propertyMappings.ContainsKey(typeof(T)))
            {
                GenerateTypePropertyMapping(typeof(T));
            }
            List<T> list = new List<T>();
            Dictionary<string, PropertyInfo> properties = propertyMappings[typeof(T)];
            //Dictionary<string, int> propertyColumnOrdinalMapping = GetPropertyColumnIndexMapping(dataTable.Columns, properties);  
            T t;
            foreach (DataRow row in dataTable.Rows)
            {
                t = new T();
                foreach (KeyValuePair<string, PropertyInfo> item in properties)
                {
                    //int ordinal = -1;  
                    //if (propertyColumnOrdinalMapping.TryGetValue(item.Key, out ordinal))  
                    //{  
                    //    item.Value.SetValue(t, GetValueFromObject(row[ordinal], item.Value.PropertyType), null);  
                    //}  
                    item.Value.SetValue(t, GetValueFromObject(row[item.Key], item.Value.PropertyType), null);
                }
                list.Add(t);
            }
            return list;
        }

        /// <summary>  
        /// ��DbDataReader��ʵ���ж�ȡ���ӵ���������  
        /// </summary>  
        /// <typeparam name="T">Ҫת����Ŀ����</typeparam>  
        /// <param name="reader">DbDataReader��ʵ��</param>  
        /// <returns></returns>  
        private static List<T> GetComplexEntities<T>(DbDataReader reader) where T : new()
        {
            if (!propertyMappings.ContainsKey(typeof(T)))//��鵱ǰ�Ƿ��Ѿ��и�������Ŀ�д����֮���ӳ��  
            {
                GenerateTypePropertyMapping(typeof(T));
            }
            List<T> list = new List<T>();
            Dictionary<string, PropertyInfo> properties = propertyMappings[typeof(T)];
            //Dictionary<string, int> propertyColumnOrdinalMapping = GetPropertyColumnIndexMapping(reader, properties);  
            T t;
            while (reader.Read())
            {
                t = new T();
                foreach (KeyValuePair<string, PropertyInfo> item in properties)
                {
                    //int ordinal = -1;  
                    //if (propertyColumnOrdinalMapping.TryGetValue(item.Key, out ordinal))  
                    //{  
                    //    item.Value.SetValue(t, GetValueFromObject(reader[ordinal], item.Value.PropertyType), null);  
                    //}  
                    item.Value.SetValue(t, GetValueFromObject(reader[item.Key], item.Value.PropertyType), null);
                }
                list.Add(t);
            }
            return list;
        }
        /// <summary>  
        /// ��DbDataReader��ʵ���ж�ȡ���������ͣ�String,ValueType)  
        /// </summary>  
        /// <typeparam name="T">Ŀ����������</typeparam>  
        /// <param name="reader">DbDataReader��ʵ��</param>  
        /// <returns></returns>  
        private static List<T> GetSimpleEntities<T>(DbDataReader reader)
        {
            List<T> list = new List<T>();
            while (reader.Read())
            {
                list.Add((T)GetValueFromObject(reader[0], typeof(T)));
            }
            return list;
        }
        /// <summary>  
        /// ��Objectת�����ַ�������  
        /// </summary>  
        /// <param name="value">object���͵�ʵ��</param>  
        /// <returns></returns>  
        private static object GetString(object value)
        {
            return Convert.ToString(value);
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <param name="targetType"></param>  
        /// <returns></returns>  
        private static object GetEnum(object value, Type targetType)
        {
            return Enum.Parse(targetType, value.ToString());
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetBoolean(object value)
        {
            if (value is Boolean)
            {
                return value;
            }
            else
            {
                byte byteValue = (byte)GetByte(value);
                if (byteValue == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetByte(object value)
        {
            if (value is Byte)
            {
                return value;
            }
            else
            {
                return byte.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetSByte(object value)
        {
            if (value is SByte)
            {
                return value;
            }
            else
            {
                return SByte.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetChar(object value)
        {
            if (value is Char)
            {
                return value;
            }
            else
            {
                return Char.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetGuid(object value)
        {
            if (value is Guid)
            {
                return value;
            }
            else
            {
                return new Guid(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetInt16(object value)
        {
            if (value is Int16)
            {
                return value;
            }
            else
            {
                return Int16.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetUInt16(object value)
        {
            if (value is UInt16)
            {
                return value;
            }
            else
            {
                return UInt16.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetInt32(object value)
        {
            if (value is Int32)
            {
                return value;
            }
            else
            {
                return Int32.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetUInt32(object value)
        {
            if (value is UInt32)
            {
                return value;
            }
            else
            {
                return UInt32.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetInt64(object value)
        {
            if (value is Int64)
            {
                return value;
            }
            else
            {
                return Int64.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetUInt64(object value)
        {
            if (value is UInt64)
            {
                return value;
            }
            else
            {
                return UInt64.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetSingle(object value)
        {
            if (value is Single)
            {
                return value;
            }
            else
            {
                return Single.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetDouble(object value)
        {
            if (value is Double)
            {
                return value;
            }
            else
            {
                return Double.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetDecimal(object value)
        {
            if (value is Decimal)
            {
                return value;
            }
            else
            {
                return Decimal.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetDateTime(object value)
        {
            if (value is DateTime)
            {
                return value;
            }
            else
            {
                return DateTime.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ��ö�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <returns></returns>  
        private static object GetTimeSpan(object value)
        {
            if (value is TimeSpan)
            {
                return value;
            }
            else
            {
                return TimeSpan.Parse(value.ToString());
            }
        }

        /// <summary>  
        /// ��Object��������ת���ɶ�Ӧ�Ŀɿ���ֵ���ͱ�ʾ  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <param name="targetType">�ɿ���ֵ����</param>  
        /// <returns></returns>  
        private static object GetGenericValueFromObject(object value, Type targetType)
        {
            if (value == DBNull.Value)
            {
                return null;
            }
            else
            {
                //��ȡ�ɿ���ֵ���Ͷ�Ӧ�Ļ�����ֵ���ͣ���int?->int,long?->long  
                Type nonGenericType = genericTypeMappings[targetType];
                return GetNonGenericValueFromObject(value, nonGenericType);
            }
        }

        /// <summary>  
        /// ��ָ���� Object ��ֵת��Ϊָ�����͵�ֵ��  
        /// </summary>  
        /// <param name="value">ʵ�� IConvertible �ӿڵ� Object������Ϊ null</param>  
        /// <param name="targetType">Ŀ����������</param>  
        /// <returns></returns>  
        private static object GetNonGenericValueFromObject(object value, Type targetType)
        {
            if (targetType.IsEnum)//��Ϊ  
            {
                return GetEnum(value, targetType);
            }
            else
            {
                switch (targetType.Name)
                {
                    case "Byte": return GetByte(value);
                    case "SByte": return GetSByte(value);
                    case "Char": return GetChar(value);
                    case "Boolean": return GetBoolean(value);
                    case "Guid": return GetGuid(value);
                    case "Int16": return GetInt16(value);
                    case "UInt16": return GetUInt16(value);
                    case "Int32": return GetInt32(value);
                    case "UInt32": return GetUInt32(value);
                    case "Int64": return GetInt64(value);
                    case "UInt64": return GetUInt64(value);
                    case "Single": return GetSingle(value);
                    case "Double": return GetDouble(value);
                    case "Decimal": return GetDecimal(value);
                    case "DateTime": return GetDateTime(value);
                    case "TimeSpan": return GetTimeSpan(value);
                    default: return null;
                }
            }
        }

        /// <summary>  
        /// ��ȡ�����������������ݿ��ֶεĶ�Ӧ��ϵӳ��  
        /// </summary>  
        /// <param name="type"></param>  
        private static void GenerateTypePropertyMapping(Type type)
        {
            if (type != null)
            {
                PropertyInfo[] properties = type.GetProperties(BindingFlag);
                Dictionary<string, PropertyInfo> propertyColumnMapping = new Dictionary<string, PropertyInfo>(properties.Length);
                string description = string.Empty;
                Attribute[] attibutes = null;
                string columnName = string.Empty;
                bool ignorable = false;
                foreach (PropertyInfo p in properties)
                {
                    ignorable = false;
                    columnName = string.Empty;
                    attibutes = Attribute.GetCustomAttributes(p);
                    foreach (Attribute attribute in attibutes)
                    {
                        //����Ƿ�������ColumnName����  
                        if (attribute.GetType() == typeof(ColumnNameAttribute))
                        {
                            columnName = ((ColumnNameAttribute)attribute).ColumnName;
                            ignorable = ((ColumnNameAttribute)attribute).Ignorable;
                            break;
                        }
                    }
                    //����������ǿɶ�����δ�����Եģ����п�����ʵ���������Զ�Ӧ����ʱ�õ���  
                    if (p.CanWrite && !ignorable)
                    {
                        //���û������ColumnName���ԣ���ֱ�ӽ�����������Ϊ���ݿ��ֶε�ӳ��  
                        if (string.IsNullOrEmpty(columnName))
                        {
                            columnName = p.Name;
                        }
                        propertyColumnMapping.Add(columnName, p);
                    }
                }
                propertyMappings.Add(type, propertyColumnMapping);
            }
        }

        //private static Dictionary<string, int> GetPropertyColumnIndexMapping(DataColumnCollection dataSource, Dictionary<string, PropertyInfo> properties)  
        //{  
        //    Stopwatch watch = new Stopwatch();  
        //    watch.Start();  
        //    Dictionary<string,int> propertyColumnIndexMapping=new Dictionary<string,int>(dataSource.Count);  
        //    foreach(KeyValuePair<string,PropertyInfo> item in properties)  
        //    {  
        //        for (int i = 0; i < dataSource.Count; i++)  
        //        {  
        //            if (item.Key.Equals(dataSource[i].ColumnName, StringComparison.InvariantCultureIgnoreCase))  
        //            {  
        //                propertyColumnIndexMapping.Add(item.Key, i);  
        //                break;  
        //            }  
        //        }  
        //    }  
        //    watch.Stop();  
        //    Debug.WriteLine("Elapsed:" + watch.ElapsedMilliseconds);  
        //    return propertyColumnIndexMapping;  
        //}  

        //private static Dictionary<string, int> GetPropertyColumnIndexMapping(DbDataReader dataSource, Dictionary<string, PropertyInfo> properties)  
        //{  
        //    Dictionary<string, int> propertyColumnIndexMapping = new Dictionary<string, int>(dataSource.FieldCount);  
        //    foreach (KeyValuePair<string, PropertyInfo> item in properties)  
        //    {  
        //        for (int i = 0; i < dataSource.FieldCount; i++)  
        //        {  
        //            if (item.Key.Equals(dataSource.GetName(i), StringComparison.InvariantCultureIgnoreCase))  
        //            {  
        //                propertyColumnIndexMapping.Add(item.Key, i);  
        //                continue;  
        //            }  
        //        }  
        //    }  
        //    return propertyColumnIndexMapping;  
        //}  
    }
    /// <summary>  
    /// �Զ������ԣ�����ָʾ��δ�DataTable����DbDataReader�ж�ȡ�������ֵ  
    /// </summary>  
    public class ColumnNameAttribute : Attribute
    {
        /// <summary>  
        /// �����Զ�Ӧ������  
        /// </summary>  
        public string ColumnName = "";
        /// <summary>  
        /// ָʾ�ڴ�DataTable����DbDataReader�ж�ȡ�������ʱ�Ƿ���Ժ����������  
        /// </summary>  
        public bool Ignorable = false;
        /// <summary>  
        /// ���캯��  
        /// </summary>  
        /// <param name="columnName">�����Զ�Ӧ������</param>  
        public ColumnNameAttribute(string columnName)
        {
            ColumnName = columnName;
            Ignorable = false;
        }
        /// <summary>  
        /// ���캯��  
        /// </summary>  
        /// <param name="ignorable">ָʾ�ڴ�DataTable����DbDataReader�ж�ȡ�������ʱ�Ƿ���Ժ����������</param>  
        public ColumnNameAttribute(bool ignorable)
        {
            Ignorable = ignorable;
        }
        /// <summary>  
        /// ���캯��  
        /// </summary>  
        /// <param name="columnName">�����Զ�Ӧ������</param>  
        /// <param name="ignorable">ָʾ�ڴ�DataTable����DbDataReader�ж�ȡ�������ʱ�Ƿ���Ժ����������</param>  
        public ColumnNameAttribute(string columnName, bool ignorable)
        {
            ColumnName = columnName;
            Ignorable = ignorable;
        }
    }

}
