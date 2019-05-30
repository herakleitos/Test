using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ef_demo
{
    public class DemoDBContext: DbContext
    {
        public DemoDBContext() : base("name=XDbContextConnString")
        {

        }
        public virtual DbSet<T_DEMO> demo { get; set; }
    }
}
