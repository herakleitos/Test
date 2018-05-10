using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DevExpress.DataAccess.Sql;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.Enums;
namespace Chaint.Common.Core.Sql
{
    public class UpdateObject
    {
        public List<UpdateItem> UpdateItems { get; private set; }
        public List<WhereItem> WhereItems { get; private set; }
        public string FilterString { get; set; }
        public string TableName { get; set; }
        public List<QueryParameter> QueryParameters { get; private set; }
        public List<SqlParameter> SqlParameters { get; private set; }
        public UpdateObject()
        {
            UpdateItems = new List<UpdateItem>();
            WhereItems = new List<WhereItem>();
            QueryParameters = new List<QueryParameter>();
            SqlParameters = new List<SqlParameter>();
        }
        public void AddUpdateItem(UpdateItem item)
        {
            UpdateItems.Add(item);
        }
        public void AddWhereItem(WhereItem item)
        {
            WhereItems.Add(item);
        }
        public string ToSqlString()
        {
            //{0}-table, {1} set items, {2} where
            string insertFormat = "UPDATE {0} SET {1} {2} ;";
            char seperator = ',';
            string equal = "=";
            string and = " AND ";
            StringBuilder contentSb = new StringBuilder();
            StringBuilder whereSb = new StringBuilder();
            foreach (var item in UpdateItems)
            {
                contentSb.Append(item.DBFieldName);
                contentSb.Append(equal);
                contentSb.Append(item.ParameterName);
                contentSb.Append(seperator);
                QueryParameter param = new QueryParameter();
                param.Name = item.ParameterName;
                param.Value = item.ParameterValue;
                param.Type = Utils.GetType(item.FieldType);
                QueryParameters.Add(param);
                DbType bdType = Utils.GetSqlDBType(item.FieldType);
                SqlParameter sqlParam = new SqlParameter(item.ParameterName, bdType);
                sqlParam.Value = item.ParameterValue;
                SqlParameters.Add(sqlParam);
            }
            if (!FilterString.IsNullOrEmptyOrWhiteSpace() || !WhereItems.IsEmpty())
            {
                whereSb.Append(" WHERE 1=1 ");
                if (!FilterString.IsNullOrEmptyOrWhiteSpace())
                {
                    whereSb.Append(and);
                    whereSb.Append(FilterString);
                }
                StringBuilder ParamsWhereSb = new StringBuilder();
                if (!WhereItems.IsEmpty())
                {
                    foreach (var item in WhereItems)
                    {
                        if (item.Type == 0)
                        {
                            ParamsWhereSb.Append(item.DBFieldName);
                            ParamsWhereSb.Append(equal);
                            ParamsWhereSb.Append(item.ParameterName);
                            ParamsWhereSb.Append(and);
                            QueryParameters.Add(Utils.GetQueryParameter(item));
                            SqlParameters.Add(Utils.GetSqlParameter(item));
                        }
                        else if (item.Type == 1)
                        {
                            string inFilter = "{0} IN ({1})";
                            StringBuilder inFilterSb = new StringBuilder();
                            foreach (SqlParam param in item.InOption)
                            {
                                inFilterSb.Append(param.ParameterName);
                                inFilterSb.Append(seperator);
                                QueryParameters.Add(Utils.GetQueryParameter(param));
                                SqlParameters.Add(Utils.GetSqlParameter(param));
                            }
                            inFilter =
                                string.Format(inFilter, item.DBFieldName, inFilterSb.ToString().TrimEnd(seperator));
                            ParamsWhereSb.Append(inFilter);
                            ParamsWhereSb.Append(and);
                        }
                    }
                }
                string filter = ParamsWhereSb.ToString();
                string ParamsWhereString = filter.Remove(filter.LastIndexOf(and));
                if (!ParamsWhereString.IsNullOrEmptyOrWhiteSpace())
                {
                    whereSb.Append(and);
                    whereSb.Append(ParamsWhereString);
                }
            }      
            string contentString = contentSb.ToString().TrimEnd(seperator);
            return string.Format(insertFormat,TableName, contentString, whereSb.ToString());
        }
    }
}
