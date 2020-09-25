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
            //Business->DemoBLL->DemoDAL->数据库；通过DemoBLL隔离业务代码和数据库操作代码。
            //可以通过改变DemoDAL的实现无缝更换数据库或者更换orm框架
            DemoBLL bll = new DemoBLL();
            List<T_DEMO> datas = new List<ef_demo.T_DEMO>();
            datas.Add(new T_DEMO() { NAME = "aaa", DESC = "111" });
            datas.Add(new T_DEMO() { NAME = "bbb", DESC = "222" });
            datas.Add(new T_DEMO() { NAME = "ccc", DESC = "333" });
            datas.Add(new T_DEMO() { NAME = "ddd", DESC = "444" });
            bll.BulkInsert(datas);


            DemoDBContext context = new DemoDBContext();
            var test = new T_DEMO() { NAME = "ccc", DESC = "333" };
            context.demo.Add(test);
            context.SaveChanges();

            Console.WriteLine("操作完成");
            Console.Read();
        }
    }
}
