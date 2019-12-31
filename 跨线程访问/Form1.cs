using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 跨线程访问
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //定义回调
        private delegate void setTextValueCallBack(int value);

        private void btnStart_Click(object sender, EventArgs e)
        {
            //创建线程
            Thread thread = new Thread(Test);
            //把线程设置为后台线程
            thread.IsBackground = true;
            thread.Start();
        }
        private void Test()
        {
            for (int i = 0; i < 10000; i++)
            {
                if (txtNum.InvokeRequired)//判断是否为跨线程访问
                {
                    //new Action<int>(n=> {this.txtNum.Text = n.ToString();})----创建匿名函数
                    txtNum.Invoke(new Action<int>(n=> {
                        txtNum.Text = n.ToString();
                    }),i);
                    //txtNum.Invoke拥有此控件的基础窗口句柄的线程上执行指定的委托。
                    //也就是说，Invoke()是一个方法，这方法执行的是委托，并且是在一个固定的线程上执行的。
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
