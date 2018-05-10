using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaint.Common.Core.Enums;

namespace Chaint.Common.Core.Sql
{
    public abstract class BaseItem
    {
        public BaseItem() { }
        public BaseItem(string dbFieldName, string parameterName, object parameterValue, Enums_FieldType fieldype)
        {
            DBFieldName = dbFieldName;
            ParameterName = parameterName;
            ParameterValue = parameterValue;
            FieldType = fieldype;
        }
        public string DBFieldName { get; protected set; }
        public string ParameterName { get; protected set; }
        public Enums_FieldType FieldType { get; protected set; }
        public object ParameterValue { get; protected set; }
    }
}
