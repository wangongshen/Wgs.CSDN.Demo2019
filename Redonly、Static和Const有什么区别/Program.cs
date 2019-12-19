using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redonly_Static和Const之间的区别
{
    class Program
    {
        #region readonly:初始化的时候可以赋值也可以不赋值，在构造函数中也可以再赋值，其他情 况下就不能再赋值了；
        //readonly string strReadonlyA;
        //readonly string strReadonlyB="OOO";
        //Program() {
        //    strReadonlyA = "AAA";
        //    strReadonlyB = "BBB";
        //}
        //static void Main(string[] args)
        //{
        //    Program program = new Program();
        //    program.strReadonlyA = "CCC";
        //    program.strReadonlyB = "DDD";

        //}
        #endregion

        #region static:初始化的时候不一定要赋值，赋值后也可以被更改，调用的时候不需要实例化；
        //static string strStaticA;
        //static string strStaticB = "OOO";
        //Program()
        //{
        //    strStaticA = "AAA";
        //    strStaticB = "BBB";
        //}
        //static void Main(string[] args)
        //{
        //    Program program = new Program();
        //    strStaticA = "CCC";
        //    program.strStaticB = "DDD";

        //}

        #endregion

        #region static:初始化的时候必须赋值，赋值后就不能再更改；
        //const string strConstA = "OOO";
        //const string strConstB ;
        //Program()
        //{
        //    strConstA = "AAA";
        //    strConstB = "BBB";
        //}
        //static void Main(string[] args)
        //{
        //    strConstA = "CCC";
        //    strConstB = "DDD";

        //}

        #endregion
    }
}
