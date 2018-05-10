using System.Collections.Generic;
using System.Linq;
using System.Data;
namespace Chaint.Common.Core.Entity
{
    public class BillHead
    {
        public BillHead()
        {
            Fields = new Dictionary<string, Field>();
            NeedSave = true;
        }
        public void AddField(Field field)
        {
            if (Fields.Keys.Contains(field.FieldName)) return;
            Fields.Add(field.FieldName, field);
        }
        public bool NeedSave { get; set; }
        public string Name { get; set; }
        public string PrimaryKey { get; set; }
        public string TableName { get; set; }
        public Dictionary<string,Field> Fields { get; set; }
    }
    public class BillEntry
    {
        public BillEntry()
        {
            Fields = new Dictionary<string, Field>();
        }
        public void AddField(Field field)
        {
            if (Fields.Keys.Contains(field.FieldName)) return;
            Fields.Add(field.FieldName, field);
        }
        public string CheckField { get; set; }
        public string ParentTable { get; set; }
        public string ParentPrimaryKey { get; set; }
        public string Name { get; set; }
        public string PrimaryKey { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, Field> Fields { get; set; }
        public DataTable Data { get; set; }
    }
}
