using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 线程池
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch(); //计时用
            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                //创建1000个线程
                Thread thread = new Thread(()=> {
                    int count = 0;
                    count++;
                });
            }
            sw.Stop();
            Console.WriteLine("运行创建线程所花费的时间："+sw.ElapsedMilliseconds);
            sw.Restart();
            for (int i = 0; i < 1000; i++)
            {
                //此处s为WaitCallback类型，转到定义查看为public delegate void WaitCallback(object state)，即是有一个参数的无返回值的委托
                ThreadPool.QueueUserWorkItem((s) =>
                {
                    int count = 0;
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    count++;
                });
            }
            sw.Stop();
            Console.WriteLine("运行线程池线程所花费的时间："+sw.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}
