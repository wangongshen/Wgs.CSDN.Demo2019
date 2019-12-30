using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 线程同步实现锁机制以抢票为例
{
    class Program
    {
        static void Main(string[] args)
        {
            Ticket ticket = new Ticket();
            //创建两个线程同时访问Sale方法
            Thread t1 = new Thread(new ThreadStart(ticket.Sale));
            Thread t2 = new Thread(new ThreadStart(ticket.Sale));
            Thread t3 = new Thread(new ThreadStart(ticket.Sale));
            Thread t4 = new Thread(new ThreadStart(ticket.Sale));
            //启动线程
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            Console.ReadKey();
        }

        class Ticket
        {
            private object locker = new object();
            //剩余图书数量
            public int num = 3;
            public void Sale()
            {
                lock (locker)
                {
                    int tmp = num;
                    if (tmp > 0)//判断是否有书，如果有就可以卖
                    {
                        Thread.Sleep(1000);
                        num -= 1;
                        Console.WriteLine("售出一张车票，还剩余{0}张", num);
                    }
                    else
                    {
                        Console.WriteLine("抢完了");
                    }
                }
            }
        }
    }
}
