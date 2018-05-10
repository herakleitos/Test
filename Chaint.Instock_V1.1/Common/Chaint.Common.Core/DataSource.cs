using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Chaint.Common.Core
{
    public class DataSource
    {
        public string DisplayMember { get; set; }

        public string DisplayTitle{ get; set; }
        public string ValueMember { get; set; }
        public DataTable Data { get; set; }
    }
}
