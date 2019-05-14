using ef_demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ef_demo
{
    class Program
    {
        static void Main(string[] args)
        {

            DemoBLL bll = new DemoBLL();

            var result = bll.GetAll(w => w.ID == 1);
        }
    }
}
