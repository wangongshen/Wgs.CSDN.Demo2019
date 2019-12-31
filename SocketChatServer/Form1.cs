using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketChatServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnStartListing_Click(object sender, EventArgs e)
        {
            //创建负责监听的Socket
            Socket socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //获取要监听的IP地址，把IP字符串转为IP
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            //获取要监听的IP和端口，合并为一个变量（创建要监听的IP和端口）
            IPEndPoint point = new IPEndPoint(ip,Convert.ToInt32(txtPort.Text));
            //绑定要监听的ip和端口号
            socketWatch.Bind(point);
            //给出监听提示
            ShowMsg("监听成功");
            //开始监听。设置服务器在一个时间点内最多能够监听的队列数量。
            socketWatch.Listen(10);

            Thread thread = new Thread(Listen);
            thread.IsBackground = true;
            thread.Start(socketWatch);

        }

        void ShowMsg(string str) {
            //如果不是创建这个控件的线程来调用它
            if (txtLog.InvokeRequired)
            {
                if (txtLog.InvokeRequired) {
                    //跨线程访问
                    txtLog.Invoke(new Action<string>(s =>
                    {
                        txtLog.AppendText(s + "\r\n");
                    }), str);
                }
            }
            else {
                txtLog.AppendText(str+"\r\n");
            }
        }

        /// <summary>
        /// 等待客户端的连接，同时创建一个与之通信的socket
        /// </summary>
        void Listen(object obj) {
            //下面开始等待客户端来连接，同时会创建一个负责通讯的socket
            Socket socketWatch = obj as Socket;
            while (true)
            {
                Socket socketCommunication = socketWatch.Accept();
                //给出连接成功提示信息。
                ShowMsg(socketCommunication.RemoteEndPoint.ToString() + ":连接成功");
                //开启一个新的线程不停的接收客户端发来的消息
                Thread thread = new Thread(Receive);
                thread.IsBackground = true;//最好设置为后台线程，为什么？
                thread.Start(socketCommunication);

            }
        }

        void Receive(object obj) {
            Socket socketCommunication = obj as Socket;
            //定义一个2M大小的字节数组
            byte[] buffer = new byte[1024*1024*2];
            while (true)
            {
                try
                {
                    //Receive方法的参数表示接收到的数据存储的位置，需要的是一个字节数组，返回值为int
                    int size = socketCommunication.Receive(buffer);
                    if (size == 0)
                    {
                        //关闭并退出Socket
                        socketCommunication.Shutdown(SocketShutdown.Both);
                        socketCommunication.Close();
                        return;
                    }
                    //string str1 = Encoding.Default.GetString(buffer, 0, r);
                    //通过Encodeing从buffer字节数组中的0个位置开始，把r个字节转成字符串；使用Default编写也可以
                    string str = Encoding.UTF8.GetString(buffer, 0, size);
                    //把接收到的数据放到文本框中
                    ShowMsg(socketCommunication.RemoteEndPoint.ToString() + ":" + str);
                }
                catch (Exception)
                {

                    throw;
                }
               
            }
        }
    }
}
