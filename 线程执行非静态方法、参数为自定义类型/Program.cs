using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 线程执行非静态方法_参数为自定义类型
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadDemoClass demoClass = new ThreadDemoClass();//实例化类的对象

            //使用委托绑定线程池要执行的方法（无参数）
            WaitCallback waitCallback1 = demoClass.Run1;
            //将方法排入队列，在线程池变为可用时执行
            ThreadPool.QueueUserWorkItem(waitCallback1);

            //使用委托绑定线程池要执行的方法（有参数）
            WaitCallback waitCallback2 = new WaitCallback(demoClass.Run1);
            //将方法排入队列，在线程池变为可用时执行
            ThreadPool.QueueUserWorkItem(waitCallback2, "张三");

            UserInfo userInfo = new UserInfo();
            userInfo.Name = "李四";
            userInfo.Age = 33;

            //使用委托绑定线程池要执行的方法（有参数,自定义类型的参数）
            WaitCallback waitCallback3 = new WaitCallback(demoClass.Run2);
            //将方法排入队列，在线程池变为可用时执行
            ThreadPool.QueueUserWorkItem(waitCallback3, userInfo);

            Console.WriteLine();
            Console.WriteLine("主线程运行...");
            Console.WriteLine("主线程ID是:" + Thread.CurrentThread.ManagedThreadId.ToString());
            Console.ReadKey();
        }
    }
    public class ThreadDemoClass
    {
        public void Run1(object obj)
        {
            string name = obj as string;

            Console.WriteLine();
            Console.WriteLine("子线程运行...");
            Console.WriteLine("子线程名字是：" + name);
            Console.WriteLine("子线程ID是:" + Thread.CurrentThread.ManagedThreadId.ToString());
        }

        public void Run2(object obj)
        {
            UserInfo userInfo = (UserInfo)obj;

            Console.WriteLine();
            Console.WriteLine("子线程运行...");
            Console.WriteLine("子线程名字是：" + userInfo.Name);
            Console.WriteLine("我今年：" + userInfo.Age + "岁");
            Console.WriteLine("子线程ID是:" + Thread.CurrentThread.ManagedThreadId.ToString());
        }
    }

    public class UserInfo
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
