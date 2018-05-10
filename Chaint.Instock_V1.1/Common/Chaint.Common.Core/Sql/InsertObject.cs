using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DevExpress.DataAccess.Sql;
using Chaint.Common.Core.Enums;
namespace Chaint.Common.Core.Sql
{
    public class InsertObject
    {
        public List<InsertItem> InsertItems { get; private set; }
        private Dictionary<string, InsertItem> dicItems;
        public string TableName { get; set; }
        public List<QueryParameter> QueryParameters { get; private set; }
        public List<SqlParameter> SqlParameters { get; private set; }
        public InsertObject()
        {
            dicItems = new Dictionary<string, InsertItem>();
            InsertItems = new List<InsertItem>();
            QueryParameters = new List<QueryParameter>();
            SqlParameters = new List<SqlParameter>();
        }
        public void AddInsertItem(InsertItem item)
        {
            InsertItems.Add(item);
            dicItems.Add(item.DBFieldName, item);
        }
        public T GetValue<T>(string dbName)
        {
            InsertItem item;
            if (dicItems.TryGetValue(dbName, out item))
            {
                if (item.ParameterValue is T)
                {
                    return (T)item.ParameterValue;
                }
                else
                {
                    return default(T);
                }
            }
            return default(T);
        }
        public string GetString(string dbName)
        {
            object value = this.GetValue<object>(dbName);
            return Convert.ToString(value);
        }
        public string ToSqlString()
        {
            string insertFormat = "INSERT INTO {0} ({1}) VALUES ({2}) ;";
            char separator = ',';
            StringBuilder fieldSb = new StringBuilder();
            StringBuilder valueSb= new StringBuilder();
            foreach (var item in InsertItems)
            {
                fieldSb.Append(item.DBFieldName);
                fieldSb.Append(separator);
                valueSb.Append(item.ParameterName);
                valueSb.Append(separator);
                QueryParameter param = new QueryParameter();
                param.Name = item.ParameterName;
                param.Value = item.ParameterValue;
                param.Type = Utils.GetType(item.FieldType);
                QueryParameters.Add(param);
                DbType bdType = Utils.GetSqlDBType(item.FieldType);
                SqlParameter sqlParam = new SqlParameter(item.ParameterName, bdType);
                sqlParam.Value= item.ParameterValue;
                SqlParameters.Add(sqlParam);
            }
            string fieldString = fieldSb.ToString().TrimEnd(separator);
            string valueString = valueSb.ToString().TrimEnd(separator);
            return string.Format(insertFormat, TableName, fieldString,valueString);
        }
    }
}
