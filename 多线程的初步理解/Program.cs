using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 多线程的初步理解
{
    class Program
    {
        static void Main(string[] args)
        {
            //通过new实例化方式
            //Thread thread = new Thread(new ThreadStart(ThreadA));
            //直接传入相应的无参无返回值方法——语法糖（允许这种简写方式，运行时编译器会自动加上）
            Thread thread = new Thread(ThreadA);
            //调用Start方法启动线程
            thread.Start();
            Console.ReadKey();
        }

        static void ThreadA() {
            Console.WriteLine("这是个无参的方法");
        }
    }
}
