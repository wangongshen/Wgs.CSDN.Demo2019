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

namespace SocketChatServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //定义一个键值对集合，将客户端的IP地址和Socket存入集合中
        Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();

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
        Socket socketCommunication;
        void Listen(object obj) {
            //下面开始等待客户端来连接，同时会创建一个负责通讯的socket
            Socket socketWatch = obj as Socket;
            while (true)
            {
                //创建一个通讯连接
                socketCommunication = socketWatch.Accept();
                //把上面创建好的负责通讯的socket存入集合中
                dicSocket.Add(socketCommunication.RemoteEndPoint.ToString(),socketCommunication);
                //这里cboUssers属于跨线程访问
                if (cboUsers.InvokeRequired)
                {
                    //跨线程访问
                    cboUsers.Invoke(new Action(() =>
                    {
                        cboUsers.Items.Add(socketCommunication.RemoteEndPoint.ToString());
                    }), null);
                }
                else {
                    //将远程连接的客户端的IP地址和端口号存储到下拉列表框中
                    cboUsers.Items.Add(socketCommunication.RemoteEndPoint.ToString());
                }
               
                //给出连接成功提示信息。
                ShowMsg(socketCommunication.RemoteEndPoint.ToString() + ":连接成功");
                //开启一个新的线程不停的接收客户端发来的消息
                Thread thread = new Thread(Receive);
                thread.IsBackground = true;//最好设置为后台线程，为什么？
                thread.Start(socketCommunication);
            }
        }
        /// <summary>
        /// 服务端接收客户端发来的信息
        /// </summary>
        /// <param name="obj"></param>
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

      
                }
               
            }
        }

        /// <summary>
        /// 服务端向客户端发送信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            //获取要发送的文本
            string str = txtMsg.Text;
            //把输入的字符串消息的转为字节数组
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            byte[] newBuffer = new byte[buffer.Length + 1];
            newBuffer[0] = 0;
            //BlockCopy的用法是：将buffer字节从第0个下标往后复制buffer.Length个长度追加到newBuffer字节第1个下标2后面
            Buffer.BlockCopy(buffer, 0, newBuffer, 1, buffer.Length);
            //send方法需要的就是字节数组
            //socketCommunication.Send(buffer);
            string ip = cboUsers.SelectedItem.ToString();
            dicSocket[ip].Send(newBuffer);
        }

        private void btnGroupSend_Click(object sender, EventArgs e)
        {
            //获取要发送的文本
            string str = txtMsg.Text;
            //把输入的字符串消息的转为字节数组
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            byte[] newBuffer = new byte[buffer.Length + 1];
            newBuffer[0] = 0;
            //BlockCopy的用法是：将buffer字节从第0个下标往后复制buffer.Length个长度追加到newBuffer字节第1个下标2后面
            Buffer.BlockCopy(buffer, 0, newBuffer, 1, buffer.Length);
            for (int i = 0; i < cboUsers.Items.Count; i++)
            {
                dicSocket[cboUsers.Items[i].ToString()].Send(newBuffer);
            }
          
        }
        /// <summary>
        /// 选择要发送的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"D:\";
            ofd.Title = "请选择要发送的文件";
            ofd.Filter = "所有文件|*.*";
            ofd.ShowDialog();
            txtPath.Text = ofd.FileName;
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            //#region 第一种实现方式，使用文件流读写
            ////获取要发送文件的路径
            //string path = txtPath.Text;
            //using (FileStream fsRead=new FileStream(path,FileMode.Open,FileAccess.Read)) {
            //    byte[] buffer = new byte[1024 * 1024 * 5];
            //    int size = fsRead.Read(buffer,0,buffer.Length);
            //    List<byte> list = new List<byte>();
            //    list.Add(1);
            //    list.AddRange(buffer);
            //    byte[] newBuffer = list.ToArray();
            //    //第四个参数一般设置为none，除非外网与内网通讯有特殊的要求
            //    dicSocket[cboUsers.SelectedItem.ToString()].Send(newBuffer,0,size+1,SocketFlags.None);
            //}
            //#endregion

            #region 第二种实现方式，使用File.ReadAllBytes读取
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    //如果没有选择文件
                    return;
                }
                else
                {
                    byte[] buffer = File.ReadAllBytes(ofd.FileName);
                    byte[] newBuffer = new byte[buffer.Length + 1];
                    newBuffer[0] = 1;
                    Buffer.BlockCopy(buffer, 0, newBuffer, 1, buffer.Length);
                    //这里第二个参数SocketFlags.None可以不需要，因为这里不存在多发送内容，读取的是文件中的所有字节数
                    dicSocket[cboUsers.SelectedItem.ToString()].Send(newBuffer, SocketFlags.None);
                }
            }
            #endregion
        }

        private void btnZD_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[1];
            buffer[0] = 2;
            dicSocket[cboUsers.SelectedItem.ToString()].Send(buffer);
        }
    }
}
