using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace asyncTest
{
    class Program
    {
        //测试修改。。。
        static void Main(string[] args)
        {
            Console.WriteLine("begining...");

            //Regex reg = new Regex("<[\\s]*?(?i)img[\\s\\S]*?[\\s]((?i)onerror|onload|onabort)[\\s]*?=[\\s\\S]*?>");
            //string result = reg.Replace("<Img =1111111111onload === 2222onerror=33333333sfsf >", "9");
            //Console.WriteLine(result);
            Regex reg = new Regex("[abc]");
            string result = reg.Replace("akb", "?");
            Console.WriteLine(result);
            Thread.Sleep(1000);
            //TestMethod();
            // var result = TestMethod(0);
            // Console.WriteLine(string.Format("output result :{0}",result.Result));
            //当主线程使用了异步方法的返回值，会阻塞主线程。
            Console.WriteLine("ending...");
            Console.Read();
        }

        async static void TestMethod()
        {
            for (int i = 0; i < 5; i++)
            {
                //await Task.Delay(2000);
                Console.WriteLine(string.Format("async process{0}", i));
                await Task.Delay(2000);
                //await关键词将之后的执行放到了新线程中。
                //因此i =0时还是在主线程中，i>0之后的执行则是在新线程中异步的。
            }
        }
        async static Task<int> TestMethod(int type)
        {
            for (int i = 0; i < 5; i++)
            {
                //await Task.Delay(2000);
                Console.WriteLine(string.Format("async process{0}", i));
                await Task.Delay(2000);
                //await关键词将之后的执行放到了新线程中。
                //因此i =0时还是在主线程中，i>0之后的执行则是在新线程中异步的。
            }
            return 0;
        }
    }
}
