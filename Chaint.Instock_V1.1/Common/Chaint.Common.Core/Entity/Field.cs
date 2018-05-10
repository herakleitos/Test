using Chaint.Common.Core.Enums;

namespace Chaint.Common.Core.Entity
{
    public class Field
    {
        public Field(string fieldname, string dbName, Enums_FieldType type)
        {
            DBName = dbName;
            FieldName = fieldname;
            Type = type;
        }
        /// <summary>
        /// 数据库字段名
        /// </summary>
        public string DBName{get;set;}
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        public object Value { get; set; }
        public Enums_FieldType Type { get; set; }
    }
}
