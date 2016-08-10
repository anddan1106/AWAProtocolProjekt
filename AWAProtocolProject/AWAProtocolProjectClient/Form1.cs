using AWAProtocol;
using AWAProtocolUtils;
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

namespace AWAProtocolProjectClient
{
    public partial class Form1 : Form
    {
        private TcpClient server;

        private string serverIP = "?";

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            serverIP = GetLocalIP(); //TODO read from IP_inputbox
            //server = new TcpClient("192.168.220.116", 8080);
            messageBox.Items.Add("Connecting to server...");
            server = new TcpClient(serverIP, 5000);

            Thread listenerThread = new Thread(Listen);
            listenerThread.Start();

            Thread senderThread = new Thread(Send);
            senderThread.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            messageBox.Items.Add(messageText.Text);
            messageText.Text = "";
        }


        public void Listen()
        {

            try
            {
                NetworkStream n = server.GetStream();
                while (true)
                {
                    AWABase obj = ProtocolUtils.Deserialize(new BinaryReader(n).ReadString());
                    if (obj == null)
                    {
                        BinaryWriter w = new BinaryWriter(n);
                        var errorObj = ProtocolUtils.CreateError(1);
                        w.Write(ProtocolUtils.Serialize(errorObj));
                        w.Flush();
                    }
                    else
                    {
                        switch (obj.Command.Type)
                        {
                            case "message":
                                messageBox.Items.Add(((AWAMessage)obj).Data.Message);
                                break;
                            case "error":
                                messageBox.Items.Add("ERROR! -- " + ((AWAError)obj).Data.Message);
                                break;
                            default:
                                break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Send()
        {
            string message = "";

            try
            {
                NetworkStream n = server.GetStream();

                while (!message.Equals("quit"))
                {
                    message = Console.ReadLine();
                    BinaryWriter w = new BinaryWriter(n);
                    w.Write(message);
                    w.Flush();
                }

                server.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private string GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "";
        }


    }
}
