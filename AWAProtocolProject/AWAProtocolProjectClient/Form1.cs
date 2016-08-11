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


        private void ConnectButton_Click(object sender, EventArgs e)
        {
            serverIP = GetLocalIP(); //TODO read from IP_inputbox
            //server = new TcpClient("192.168.220.116", 8080);
            messageBox.Items.Add("Connecting to server...");
            try
            {
                server = new TcpClient(serverIP, 5000);
                NetworkStream n = server.GetStream();
                //TODO loopa tills vi får ett usernameRequest
                AWABase obj = null;
                do
                {
                    obj = ProtocolUtils.Deserialize(new BinaryReader(n).ReadString());

                } while (obj == null || obj.Command.Type != AWAProtocol.CommandType.Request || ((AWARequest)obj).Data.RequestFor != RequestType.Username);

                UsernameLabel.Text = ((AWARequest)obj).Data.Message;
                ConnectPanel.Visible = false;
                UsernamePanel.Show();



            }
            catch (Exception)
            {
                //TODO add error message and possibliy try again
                throw;
            }

            //Thread senderThread = new Thread(Send);
            //senderThread.Start();

        }

        private void messageButton_Click(object sender, EventArgs e)
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
                        var errorObj = ProtocolUtils.CreateError(1);
                        sendObject(n, errorObj);
                    }
                    else
                    {
                        switch (obj.Command.Type)
                        {
                            case AWAProtocol.CommandType.Message:
                                messageBox.Items.Add(((AWAMessage)obj).Data.Message);
                                break;

                            case AWAProtocol.CommandType.Error:
                                messageBox.Items.Add("ERROR! -- " + ((AWAError)obj).Data.Message);
                                break;

                            case AWAProtocol.CommandType.GameInit:
                                CreateGame(((AWAGameMove)obj).Data.Json, ((AWAGameMove)obj).Data.MoveType);
                                break;
                            case AWAProtocol.CommandType.GameMove:
                                PlayerMove(((AWAGameMove)obj).Data.Json);
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

        private void CreateGame(string json, GameMoveType moveType)
        {
            //TODO om moveType ör field size eller spelares placering
            throw new NotImplementedException();
        }

        private void PlayerMove(string json)
        {
            //TODO flytta spelare
            throw new NotImplementedException();
        }

        private void InitiateGame(string json)
        {

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

        private void UsernameButton_Click(object sender, EventArgs e)
        {
            try
            {
                NetworkStream n = server.GetStream();
                string name = UsernameTextBox.Text;
                Log.WriteLine($"sending username : {name}");
                AWABase obj = null;

                sendObject(n, new AWAResponse("1", ResponseType.Username, name, "1.0"));
                obj = ProtocolUtils.Deserialize(new BinaryReader(n).ReadString());
                if (obj != null)
                {
                    if (obj.Command.Type == AWAProtocol.CommandType.Request && ((AWARequest)obj).Data.RequestFor == RequestType.Username)
                        UsernameLabel.Text = ((AWARequest)obj).Data.Message;
                    else if (obj.Command.Type == AWAProtocol.CommandType.Ok)
                    {
                        UsernamePanel.Visible = false;
                        InitiateGame();
                    }
                    else
                    {
                        UsernameLabel.Text = "försök igen.";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void InitiateGame()
        {
            GameFieldPanel.Visible = true;
            GameFieldLabel.Text = "waiting for other players...";

            //TODO skapa lyssnar-tråd och skickar-tråd
            Thread listenerThread = new Thread(Listen);
            listenerThread.Start();

        }

        private void sendObject(NetworkStream n, AWABase obj)
        {
            BinaryWriter w = new BinaryWriter(n);
            w.Write(ProtocolUtils.Serialize(obj));
            w.Flush();
        }
    }
}
