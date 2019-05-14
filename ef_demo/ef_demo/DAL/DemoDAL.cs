using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ef_demo
{
    public class DemoDAL:BaseDAL<TbGTRemovePeoInfo>,IDemoDAL
    {
        protected DemoDBContext context;
        public DemoDAL(DemoDBContext context) : base(context)
        {
            this.context = context;
        }
    }
}
