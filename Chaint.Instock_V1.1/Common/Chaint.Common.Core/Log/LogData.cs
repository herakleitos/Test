using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaint.Common.Core.Sql;
using Chaint.Common.Core.Utils;
using System.Data;

namespace Chaint.Common.Core.Log
{
    public class LogData
    {
        public string LogTableName { get; private set; }
        private List<LogRecord> _record;
        public LogData()
        {
            LogTableName = "T_AUTOSCAN_LOG";
            _record = new List<LogRecord>();
        }
        public List<InsertObject> ToInsertObject()
        {
            List<InsertObject> excObjs = new List<InsertObject>();
            foreach (var item in _record)
            {
                InsertObject obj = new InsertObject();
                obj.TableName = LogTableName;
                InsertItem item1 = new InsertItem("FID", "@FID",
                    item.Id, Enums.Enums_FieldType.String);
                InsertItem item2 = new InsertItem("FKEY", "@FKEY",
                    item.Key, Enums.Enums_FieldType.String);
                InsertItem item3 = new InsertItem("FCONTENT", "@FCONTENT",
                    item.Content, Enums.Enums_FieldType.String);
                InsertItem item4 = new InsertItem("FDATE", "@FDATE",
                    item.OperDate, Enums.Enums_FieldType.DateTime);
                InsertItem item5 = new InsertItem("FSOURCEID", "@FSOURCEID",
                    item.SourceId, Enums.Enums_FieldType.String);
                InsertItem item6 = new InsertItem("FSOURCETABLENAME", "@FSOURCETABLENAME",
                    item.SourceTableName, Enums.Enums_FieldType.String);
                InsertItem item7 = new InsertItem("FUSER", "@FUSER",
                    item.Operator, Enums.Enums_FieldType.String);
                obj.AddInsertItem(item1);
                obj.AddInsertItem(item2);
                obj.AddInsertItem(item3);
                obj.AddInsertItem(item4);
                obj.AddInsertItem(item5);
                obj.AddInsertItem(item6);
                obj.AddInsertItem(item7);
                excObjs.Add(obj);
            }
            return excObjs;
        }
        public BatchInsertParam ToBulkInsertObject()
        {
            BatchInsertParam param = new BatchInsertParam();
            param.TableName = LogTableName;
            DataTable data = new DataTable();
            data.Columns.Add("FID", typeof(string));
            data.Columns.Add("FKEY", typeof(string));
            data.Columns.Add("FCONTENT", typeof(string));
            data.Columns.Add("FDATE", typeof(DateTime));
            data.Columns.Add("FSOURCEID", typeof(string));
            data.Columns.Add("FSOURCETABLENAME", typeof(string));
            data.Columns.Add("FUSER", typeof(string));
            foreach (var item in _record)
            {
                DataRow newRow = data.NewRow();
                newRow["FID"] = item.Id;
                newRow["FKEY"] = item.Key;
                newRow["FCONTENT"] = item.Content;
                newRow["FDATE"] = item.OperDate;
                newRow["FSOURCEID"] = item.SourceId;
                newRow["FSOURCETABLENAME"] = item.SourceTableName;
                newRow["FUSER"] = item.Operator;
                data.Rows.Add(newRow);
            }
            data.AcceptChanges();
            param.Data = data;
            param.FieldMapping.Add("FID", "FID");
            param.FieldMapping.Add("FKEY", "FKEY");
            param.FieldMapping.Add("FCONTENT", "FCONTENT");
            param.FieldMapping.Add("FDATE", "FDATE");
            param.FieldMapping.Add("FSOURCEID", "FSOURCEID");
            param.FieldMapping.Add("FSOURCETABLENAME", "FSOURCETABLENAME");
            param.FieldMapping.Add("FUSER", "FUSER");
            return param;
        }
        public void AddRecord(LogRecord rec)
        {
            _record.Add(rec);
        }
    }
    public class LogRecord
    {
        public LogRecord()
        {
            OperDate = DateTime.Now;
            Id = SequenceGuid.NewGuid().ToString();
        }
        public DateTime OperDate { get; private set; }
        public string Id { get; private set; }
        public string Key = string.Empty;
        public string Content = string.Empty;
        public string SourceId = string.Empty;
        public string SourceTableName = string.Empty;
        public string Operator = string.Empty;
    }
}
