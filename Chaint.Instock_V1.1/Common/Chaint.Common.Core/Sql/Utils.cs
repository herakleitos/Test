using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DevExpress.DataAccess.Sql;
using System.Data;
using Chaint.Common.Core.Enums;

namespace Chaint.Common.Core.Sql
{
    public static class Utils
    {
        public static Type GetType(Enums_FieldType dbFieldType)
        {
            Type type =null;
            if (dbFieldType == Enums_FieldType.String)
            {
                type = typeof(string);
            }
            if (dbFieldType == Enums_FieldType.Int32)
            {
                type = typeof(int);
            }
            if (dbFieldType == Enums_FieldType.Int64)
            {
                type = typeof(long);
            }
            if (dbFieldType == Enums_FieldType.DateTime)
            {
                type = typeof(DateTime);
            }
            if (dbFieldType == Enums_FieldType.Decimal)
            {
                type = typeof(decimal);
            }
            if (dbFieldType == Enums_FieldType.Bool)
            {
                type = typeof(bool);
            }
            return type;
        }
        public static DbType GetSqlDBType(Enums_FieldType dbFieldType)
        {
            DbType type = DbType.String;
            if (dbFieldType == Enums_FieldType.String)
            {
                type = DbType.String;
            }
            if (dbFieldType == Enums_FieldType.Int32)
            {
                type = DbType.Int32;
            }
            if (dbFieldType == Enums_FieldType.Int64)
            {
                type = DbType.Int64;
            }
            if (dbFieldType == Enums_FieldType.DateTime)
            {
                type = DbType.DateTime;
            }
            if (dbFieldType == Enums_FieldType.Decimal)
            {
                type = DbType.Decimal;
            }
            if (dbFieldType == Enums_FieldType.Bool)
            {
                type = DbType.Boolean;
            }
            return type;
        }
        public static QueryParameter GetQueryParameter(BaseItem item)
        {
            QueryParameter param = new QueryParameter();
            param.Name = item.ParameterName;
            param.Value = item.ParameterValue;
            param.Type = Utils.GetType(item.FieldType);
            return param;
        }
        public static SqlParameter GetSqlParameter(BaseItem item)
        {
            DbType bdType = Utils.GetSqlDBType(item.FieldType);
            SqlParameter sqlParam = new SqlParameter(item.ParameterName, bdType);
            sqlParam.Value = item.ParameterValue;
            return sqlParam;
        }
        public static QueryParameter GetQueryParameter(SqlParam item)
        {
            QueryParameter param = new QueryParameter();
            param.Name = item.ParameterName;
            param.Value = item.ParameterValue;
            param.Type = Utils.GetType(item.FieldType);
            return param;
        }
        public static SqlParameter GetSqlParameter(SqlParam item)
        {
            DbType bdType = Utils.GetSqlDBType(item.FieldType);
            SqlParameter sqlParam = new SqlParameter(item.ParameterName, bdType);
            sqlParam.Value = item.ParameterValue;
            return sqlParam;
        }
    }
}
