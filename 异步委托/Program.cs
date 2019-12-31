using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 异步委托
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("主线程id："+Thread.CurrentThread.ManagedThreadId);
            //定义一个委托
            Func<int, int, string> delFunc = (a, b) =>
            {
                //由于下面执行这个委托时使用了BeginInvoke方法，所以就会开启一个新线程去执行，所以称为异步线程
                Console.WriteLine("异步线程id：" + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(2000);
                return (a + b).ToString();
            };
            //1.先拿到BeginInvoke方法的返回值result
            IAsyncResult result = delFunc.BeginInvoke(1, 2, null, null);
            //没有执行完，主线程就一直执行下面的循环体
            while (!result.AsyncWaitHandle.WaitOne(1000))
            {
                Thread.Sleep(100);
                Console.WriteLine("主线程正在运行...");
                Console.WriteLine("主线程ID是:" + Thread.CurrentThread.ManagedThreadId.ToString());
                Console.WriteLine();
            }
            //2.调用委托的EndInvoke方法，把BeginInvoke方法的返回值result传入，即可拿到委托方法的执行结果
            string str = delFunc.EndInvoke(result);
            Console.WriteLine(str);
            Console.ReadKey();
        }
    }
}
