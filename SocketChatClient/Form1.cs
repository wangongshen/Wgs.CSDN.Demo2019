using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketChatClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //取消跨线程之间的检查
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        Socket socketCommunication;
        private void btnStart_Click(object sender, EventArgs e)
        {
            //创建一个通讯的Socket
            socketCommunication = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //获得要连接的远程服务器的IP和端口号
            IPAddress ip = IPAddress.Parse(txtServer.Text);
            IPEndPoint point = new IPEndPoint(ip,Convert.ToInt32(txtPort.Text));
            //调用Connection方法去连接远程服务器
            socketCommunication.Connect(point);
            //提示连接成功
            ShowMsg("连接成功");
            //开启一个新的线程不断的去接收服务端发来的消息
            Thread thread = new Thread(Receive);
            //设置为后台线程
            thread.IsBackground = true;
            thread.Start();
        }

        void ShowMsg(string str)
        {
            //如果不是创建这个控件的线程来调用它
            if (txtLog.InvokeRequired)
            {
                if (txtLog.InvokeRequired)
                {
                    //跨线程访问
                    txtLog.Invoke(new Action<string>(s =>
                    {
                        txtLog.AppendText(s + "\r\n");
                    }), str);
                }
            }
            else
            {
                txtLog.AppendText(str + "\r\n");
            }
        }

        /// <summary>
        /// 客户端向服务端发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            //获取输入的信息
            string str = txtMsg.Text.Trim();
            //把输入的消息转为字节数组，所用的编码要和服务端保持一致
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            //向服务端发送消息
            socketCommunication.Send(buffer);
        }

        void Receive() {
            try
            {
                //因为要不停的去接收，所以写一个while循环里
                byte[] buffer = new byte[1024 * 1024 * 3];
                while (true)
                {
                    //不管是发送还是接收消息，都要拿到负责通讯的socket
                    //size表示实际接收到的有效字节数
                    int size = socketCommunication.Receive(buffer);
                    //为防止服务器端关掉，客户端还在不断的接收消息
                    int type = buffer[0];
                    if (size == 0)
                    {
                        break;
                    }
                    if (type == 0)
                    {
                        //接收文本

                        string str = Encoding.UTF8.GetString(buffer, 1, size - 1);
                        //在文本框中显示接收到的消息
                        ShowMsg(socketCommunication.RemoteEndPoint + ":" + str);
                    }
                    else if (type == 1)
                    {
                        //接收文件

                        //#region 第一种实现思路，使用文件流读写
                        //SaveFileDialog sfd = new SaveFileDialog();
                        //sfd.InitialDirectory = @"F:\123";
                        //sfd.Title = "请选择要保存的文件位置";
                        //sfd.Filter = "所有文件|*.*|";
                        //sfd.ShowDialog(this);
                        //string path = sfd.FileName;
                        //using (FileStream fsWrite=new FileStream(path,FileMode.OpenOrCreate,FileAccess.Write)) {
                        //    //把接收到的数据从第一个字节位置，size-1个字节的数据写入到流指向的文件中
                        //    fsWrite.Write(buffer,1,size-1);
                        //}
                        //MessageBox.Show("保存成功");
                        //#endregion

                        #region 第二种实现思路，不用使用文件流写入使用File.WriteAllBytes
                        using (SaveFileDialog sfd = new SaveFileDialog())
                        {
                            sfd.DefaultExt = "doc";
                            sfd.Filter = "所有文件|*.*|文本文件(*.txt)|*.txt|word文档(*.doc)|*.doc";
                            if (sfd.ShowDialog(this) != DialogResult.OK)
                            {
                                return;
                            }
                            else
                            {
                                byte[] newBuffer = new byte[size - 1];
                                Buffer.BlockCopy(buffer, 1, newBuffer, 0, size - 1);
                                File.WriteAllBytes(sfd.FileName, newBuffer);
                                MessageBox.Show("保存成功");
                            }
                        }
                        #endregion
                    }
                    else if (type == 2) {
                        //接收震动
                        ZD();
                    }
                 
                  
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        /// <summary>
        /// 震动，改变窗体作为位置
        /// </summary>
        void ZD()
        {
            for (int i = 0; i < 600; i++)
            {
                this.Location = new Point(200, 200);
                this.Location = new Point(260, 260);
            }
        }
    }
}
