using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaint.Common.Core.Enums;
namespace Chaint.Common.Core.Sql
{
    public class WhereItem:BaseItem
    {
        public List<SqlParam> InOption { get; set; }
        // 0 代表单个的 name=@value ,1 代表 name in(@value1,@value2)
        public int Type { get; private set; }
        public WhereItem(string dbFieldName, string parameterName, object parameterValue, Enums_FieldType fieldype):
            base(dbFieldName, parameterName, parameterValue, fieldype)
        {
            Type = 0;
        }
        public WhereItem(string dbFieldName, Enums_FieldType fieldype)
        {
            this.DBFieldName = dbFieldName;
            this.FieldType = fieldype;
            InOption = new List<SqlParam>();
            Type = 1;
        }
    }
    public class SqlParam
    {
        public SqlParam(string parameterName, object parameterValue, Enums_FieldType fieldype)
        {
            ParameterName = parameterName;
            ParameterValue = parameterValue;
            FieldType = fieldype;
        }
        public string ParameterName { get; protected set; }
        public object ParameterValue { get; protected set; }

        public Enums_FieldType FieldType { get; protected set; }
    }
}
