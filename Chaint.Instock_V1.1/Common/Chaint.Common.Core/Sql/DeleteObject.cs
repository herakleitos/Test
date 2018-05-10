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
    public class DeleteObject
    {
        public List<WhereItem> WhereItems { get; private set; }
        public string FilterString { get; set; }
        public string TableName { get; set; }
        public List<QueryParameter> QueryParameters { get; private set; }
        public List<SqlParameter> SqlParameters { get; private set; }
        public DeleteObject()
        {
            WhereItems = new List<WhereItem>();
            QueryParameters = new List<QueryParameter>();
            SqlParameters = new List<SqlParameter>();
        }
        public string ToSqlString()
        {
            //{0}-table, {1} where
            string deleteFormat = "DELETE FROM {0}{1} ;";
            string equal = "=";
            char seperator = ',';
            string and = " AND ";
            StringBuilder whereSb = new StringBuilder();
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
                string ParamsWhereString = ParamsWhereSb.ToString().TrimEnd(and.ToCharArray());
                if (!ParamsWhereString.IsNullOrEmptyOrWhiteSpace())
                {
                    whereSb.Append(and);
                    whereSb.Append(ParamsWhereString);
                }
            }
            return string.Format(deleteFormat, TableName, whereSb.ToString());
        }
    }
}
