using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 构造有参数的线程ParameterizedThreadStart
{
    class Program
    {
        static void Main(string[] args)
        {
            //Thread thread = new Thread(new ParameterizedThreadStart(WorkerA));
            Thread thread = new Thread(WorkerA);
            List<int> list = new List<int>() { 1, 2, 3 };
            thread.Start(list);
            Console.WriteLine("主程序退出");
            Console.ReadKey();
        }

        static void WorkerA(object data)
        {
            List<int> listmy = (List<int>)data;
            Thread.Sleep(1000);
            for (int i = 0; i < listmy.Count; i++)
            {
                Console.WriteLine("传入的参数" + listmy[i].ToString());
            }
            Console.WriteLine("后台程序退出");
        }
    }
}
