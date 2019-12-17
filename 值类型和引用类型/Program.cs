using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 值类型和引用类型
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("值类型，保存的是具体的值");
            int x = 1;
            int y = x;
            y = y + x;
            Console.WriteLine("x:" + x);
            Console.WriteLine("y:" + y);

            Console.WriteLine("引用类型，保存的是值的地址");
            User user1 = new User();
            user1.name = "张一";
            User user2 = user1;
            user2.name = "张二";
            Console.WriteLine("user1.name:" + user1.name);
            Console.WriteLine("user2.name:" + user2.name );
            Console.ReadKey();
        }
    }
}
