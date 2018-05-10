using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace Chaint.Common.Core.Sql
{
    public class ExcuteObject
    {
        public ExcuteObject()
        {
            Parameters = new List<SqlParameter>();
        }
        public string Sql { get; set; }
        public List<SqlParameter> Parameters { get; set; }
        /// <summary>
        /// 用于批量插入
        /// </summary>
        public BatchInsertParam BatchInsertParam{ get; set; }
    }
}
