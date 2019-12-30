using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 前台线程和后台线程
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("程序启动");
            Thread thread = new Thread(Worker);
            thread.Start();
            thread.IsBackground = true;
            thread.Join();//执行这句话的线程（主线程）会等待后台线程结束之后才能继续执行
            Console.WriteLine("主线程退出");
            Console.ReadKey();
        }

        static void Worker() {
            Thread.Sleep(1000);
            Console.WriteLine("后台线程退出");
            //Console.ReadKey();
        }
    }
}
