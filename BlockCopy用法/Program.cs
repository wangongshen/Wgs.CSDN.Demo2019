using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockCopy用法
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] byA = new byte[1, 2, 3, 4, 5];
            byte[] byB = new byte[byA.Length + 1];
            Buffer.BlockCopy(byA, 0, byB, 1, byA.Length);
            Console.ReadKey();
        }
    }
}
