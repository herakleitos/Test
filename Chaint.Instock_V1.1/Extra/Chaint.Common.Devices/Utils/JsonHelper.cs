using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.Common;

using System.Runtime.Serialization;



/*-----------------------------------------------------------------------------------
 * 作者: Automation&IT Dept. 
 * 
 * 创建时间: 2015-05-21
 * 
 * 功能描述: 
 *      Json数据格式的互转
 *      
 *      Json字符串对应的类对象定义格式如：
 *      
        string jsonstr=  [{
        "width":180,
        "length":230,
        "cursta":50,
        "outqty":220,
        "seqno":123456,
        "receiveno":"H001234"
        },{
        "width":220,
        "length":230,
        "cursta":50,
        "outqty":220,
        "seqno":123456,
        "receiveno":"H00aaaa"
        }]

 * 对象定义方式：
 * 
 * 如：  [DataContract]
    public class OrderInform
    {
        [DataMember(Order = 0)]
        public int width { get; set; }

        [DataMember(Order = 1)]
        public int length { get; set; }
        [DataMember(Order = 2)]
        public int cursta { get; set; }
        [DataMember(Order = 3)]
        public int outqty { get; set; }
        [DataMember(Order = 4)]
        public int seqno { get; set; }

        [DataMember(Order = 5)]
        public string receiveno { get; set; }
    }
 
 *      
 *      
 * 
 ------------------------------------------------------------------------------------*/

namespace Chaint.Common.Devices.Utils
{
    /// <summary>
    /// JSON帮助类
    /// </summary>
    public class JsonHelper
    {
        #region 通用方法
        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        public static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = StringFilter(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type == typeof(Guid))
            {
                str = "\"" + str + "\"";
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        }

        /// <summary>
        /// 过滤字符串
        /// </summary>
        public static string StringFilter(string str)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char c = str.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 列转json
        /// <summary>
        /// 列转json
        /// </summary>
        /// <param name="dt">表</param>
        /// <param name="r">列</param>
        public static string ColumnToJson(DataTable dt, int r)
        {
            StringBuilder strSql = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strSql.Append(dt.Rows[i][r]);
                strSql.Append(",");
            }
            return strSql.ToString().Trim(',');
        }
        #endregion

        #region 对象转json
        /// <summary>
        /// 对象转json
        /// </summary>
        public static string ToJson(object jsonObject)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            PropertyInfo[] propertyInfo = jsonObject.GetType().GetProperties();
            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object objectValue = propertyInfo[i].GetGetMethod().Invoke(jsonObject, null);
                Type type = propertyInfo[i].PropertyType;
                string strValue = objectValue.ToString();
                strValue = StringFormat(strValue, type);
                sb.Append("\"" + propertyInfo[i].Name + "\":");
                sb.Append(strValue + ",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
        #endregion

        #region list转json
        /// <summary>
        /// list转json
        /// </summary>
        public static string ListToJson<T>(IList<T> list)
        {
            object obj = list[0];
            return ListToJson<T>(list, obj.GetType().Name);
        }

        private static string ListToJson<T>(IList<T> list, string JsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(JsonName))
                JsonName = list[0].GetType().Name;
            Json.Append("{\"" + JsonName + "\":[");
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pi.Length; j++)
                    {
                        Type type = pi[j].GetValue(list[i], null).GetType();
                        Json.Append("\"" + pi[j].Name.ToString() + "\":" + StringFormat(pi[j].GetValue(list[i], null).ToString(), type));
                        if (j < pi.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        #endregion

        #region 对象集合转换为json
        /// <summary>
        /// 对象集合转换为json
        /// </summary>
        /// <param name="array">对象集合</param>
        /// <returns>json字符串</returns>
        public static string ToJson(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString += ToJson(item) + ",";
            }
            jsonString = jsonString.Substring(0, jsonString.Length - 1);
            return jsonString + "]";
        }
        #endregion

        #region 普通集合转换Json
        /// <summary>    
        /// 普通集合转换Json   
        /// </summary>   
        /// <param name="array">集合对象</param> 
        /// <returns>Json字符串</returns>  
        public static string ToArrayString(IEnumerable array)
        {
            string jsonString = "[";
            foreach (object item in array)
            {
                jsonString = ToJson(item.ToString()) + ",";
            }
            jsonString.Remove(jsonString.Length - 1, jsonString.Length);
            return jsonString + "]";
        }
        #endregion

        #region  DataSet转换为Json
        /// <summary>    
        /// DataSet转换为Json   
        /// </summary>    
        /// <param name="dataSet">DataSet对象</param>   
        /// <returns>Json字符串</returns>    
        public static string ToJson(DataSet dataSet)
        {
            string jsonString = "{";
            foreach (DataTable table in dataSet.Tables)
            {
                jsonString += "\"" + table.TableName + "\":" + ToJson(table) + ",";
            }
            jsonString = jsonString.TrimEnd(',');
            return jsonString + "}";
        }
        #endregion

        #region Datatable转换为Json
        /// <summary>     
        /// Datatable转换为Json     
        /// </summary>    
        public static string ToJson(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                StringBuilder jsonString = new StringBuilder();
                jsonString.Append("[");
                DataRowCollection drc = dt.Rows;
                for (int i = 0; i < drc.Count; i++)
                {
                    jsonString.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string strKey = dt.Columns[j].ColumnName;
                        string strValue = drc[i][j].ToString();

                        Type type = dt.Columns[j].DataType;
                        jsonString.Append("\"" + strKey + "\":");
                        strValue = StringFormat(strValue, type);
                        if (j < dt.Columns.Count - 1)
                            jsonString.Append(strValue + ",");
                        else
                            jsonString.Append(strValue);
                    }
                    jsonString.Append("},");
                }
                jsonString.Remove(jsonString.Length - 1, 1);
                jsonString.Append("]");
                return jsonString.ToString();
            }
            else
                return "[]";
        }

        /// <summary>    
        /// DataTable转换为Json
        /// </summary>    
        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + StringFormat(dt.Rows[i][j].ToString(), type));
                        if (j < dt.Columns.Count - 1)
                            Json.Append(",");
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                        Json.Append(",");
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        #endregion

        #region DataReader转换为Json
        /// <summary>     
        /// DataReader转换为Json     
        /// </summary>     
        /// <param name="dataReader">DataReader对象</param>     
        /// <returns>Json字符串</returns>  
        public static string ToJson(DbDataReader dataReader)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            while (dataReader.Read())
            {
                jsonString.Append("{");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    Type type = dataReader.GetFieldType(i);
                    string strKey = dataReader.GetName(i);
                    string strValue = dataReader[i].ToString();
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (i < dataReader.FieldCount - 1)
                        jsonString.Append(strValue + ",");
                    else
                        jsonString.Append(strValue);
                }
                jsonString.Append("},");
            }
            dataReader.Close();
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        #endregion


        /// <summary>
        /// 将JSon格式的字符串转化为相应的 集合类对象
        /// 
        /// 其中转换的对象 
        /// 类定义中须包含[DataContract]属性，类属性包含[DataMember]
        /// 类属性成员定义中还须区分数据类型
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string jsonString)
        {
            using (var ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                return (T)new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }


        /// <summary>
        /// 将对象转化为JSon格式的字符串
        /// </summary>
        /// <typeparam name="T">一个集合类型，如IList</typeparam>
        /// <param name="obj">一般为一个实体类型</param>
        /// <returns></returns>
        public static string ToJSon<T>(T obj)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T)).WriteObject(ms, obj);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }


        #region 返回错误
        public static string error()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("error", typeof(int));
            DataRow dr = dt.NewRow();
            dr["error"] = 1;
            dt.Rows.Add(dr);

            string strRet=ToJson(dt);

            dt.Dispose();
            dt = null;
            return strRet;
        }
        #endregion

    }
}