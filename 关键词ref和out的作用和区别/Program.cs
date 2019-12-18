using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 关键词ref和out的作用和区别
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("==============Ref的使用==============");
            int num1 = 10;
            int num2 = 20;
            Console.WriteLine("加Ref之前");
            TestA(num1, num2);
            Console.WriteLine("TestA外输出：num1=" + num1 + ";num2=" + num2);
            Console.WriteLine("加Ref之后");
            //为了避免不必要的麻烦，我们将num1和num2重新赋值
            num1 = 10;
            num2 = 20;
            TestB(ref num1, ref num2);
            Console.WriteLine("TestB外输出：num1=" + num1 + ";num2=" + num2);
            Console.WriteLine("==============Out的使用==============");
            int numA, numB,numC;
            numC = TestC(num1, num2, out numA, out numB);
            Console.WriteLine("numA:"+numA+";numB:"+numB+";numC:"+numC);
            Console.ReadKey();

        }

        public static void TestA(int num1,int num2) {
            num1++;
            num2++;
            Console.WriteLine("TestA内输出：num1="+num1+";num2="+num2);
        }

        public static void TestB(ref int num1, ref int num2)
        {
            num1++;
            num2++;
            Console.WriteLine("TestB内输出：num1=" + num1 + ";num2=" + num2);
        }

        public static int TestC(int num1, int num2,out int num3,out int num4)
        {
            //如果我想反回多个参数，只用return是不行的，用out就可以解决
            num3 = num1 + num2;
            num4 = num1 * num3;
            return num1 * num2;
        }
    }
}
