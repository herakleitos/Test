using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Chaint.Common.Core.Sql
{
    public class BatchInsertParam
    {
        public BatchInsertParam()
        {
            FieldMapping = new Dictionary<string, string>();
        }
        public string TableName { get; set; }
        public Dictionary<string, string> FieldMapping { get; set; }

        public DataTable Data { get; set; }
    }
}
