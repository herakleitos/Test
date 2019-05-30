using ef_demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ef_demo
{
    class Program
    {
        static void Main(string[] args)
        {

            DemoBLL bll = new DemoBLL();
            List<T_DEMO> datas = new List<ef_demo.T_DEMO>();
            datas.Add(new T_DEMO() { NAME = "aaa", DESC = "111" });
            datas.Add(new T_DEMO() { NAME = "bbb", DESC = "222" });
            datas.Add(new T_DEMO() { NAME = "ccc", DESC = "333" });
            datas.Add(new T_DEMO() { NAME = "ddd", DESC = "444" });
            bll.BulkInsert(datas);

            Console.WriteLine("操作完成");
            Console.Read();
        }
    }
}
