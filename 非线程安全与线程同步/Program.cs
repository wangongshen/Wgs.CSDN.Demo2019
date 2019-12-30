using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 非线程安全与线程同步
{
    class Program
    {
        private static object locker = new object();
        static int Num = 1;
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            //开始计时
            stopWatch.Start();
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(Run);
                thread.Start();
                //thread.Join();
            }

            Num++;
            Console.WriteLine("Num:" + Num);
            Console.WriteLine("主线程ID是:" + Thread.CurrentThread.ManagedThreadId.ToString());
            //停止计时
            stopWatch.Stop();

            //输出执行的时间，毫秒数
            Console.WriteLine("执行时间为：" + stopWatch.ElapsedMilliseconds + "毫秒");
            Console.ReadKey();
        }

        public static void Run()
        {
            lock (locker) {
                Num++;
                Console.WriteLine("Num:" + Num);
                Console.WriteLine("子线程ID是:" + Thread.CurrentThread.ManagedThreadId.ToString());
            }
          
        }
    }
}
