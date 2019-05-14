using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ef_demo
{
    public class DemoBLL : BaseBLL<TbGTRemovePeoInfo>,IDemoBLL
    {
        protected IDemoDAL dal;
        public DemoBLL()
        {
            dal = container.Resolve<IDemoDAL>();
            baseDAL = dal;
        }
        public DemoBLL(IDemoDAL dal) : base(dal)
        {
            this.dal = dal;
        }
    }
}
