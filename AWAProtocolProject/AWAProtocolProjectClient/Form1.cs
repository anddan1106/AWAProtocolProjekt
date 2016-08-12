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
        private Player player;
        private List<Player> opponents = new List<Player>();
        private int tileSize = 32;

        public Form1()
        {
            InitializeComponent();
            ConnectTextBox.Text = GetLocalIP();
            //this.KeyDown += Form1_KeyDown;
            //this.PreviewKeyDown += Form1_PreviewKeyDown;

        }


        private void ConnectButton_Click(object sender, EventArgs e)
        {
            //server = new TcpClient("192.168.220.116", 8080);
            showMessage("Connecting to server...");
            try
            {
                serverIP = ConnectTextBox.Text;
                server = new TcpClient(serverIP, 5000);
                NetworkStream n = server.GetStream();
                showMessage("Connected to server...");
                AWABase obj = null;
                do
                {
                    obj = ProtocolUtils.Deserialize(new BinaryReader(n).ReadString());
                    showMessage("Server asking for username");

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
        private void UsernameButton_Click(object sender, EventArgs e)
        {
            try
            {
                NetworkStream n = server.GetStream();
                string name = UsernameTextBox.Text;
                Log.WriteLine($"sending username : {name}");
                AWABase obj = null;

                sendObject(new AWAResponse("1", ResponseType.Username, name, "1.0"));
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
        private void messageButton_Click(object sender, EventArgs e)
        {
            //TODO Skicka chat meddelande
            showMessage(messageText.Text);
            messageText.Text = "";
        }
        public void Listen()
        {

            try
            {
                NetworkStream n = server.GetStream();
                while (true)
                {
                    string str = new BinaryReader(n).ReadString();
                    AWABase obj = ProtocolUtils.Deserialize(str);
                    if (obj == null)
                    {
                        showMessage("object was not valid: " + str);
                        var errorObj = ProtocolUtils.CreateError(1); // No handeling ServerSide
                        sendObject(errorObj);
                    }
                    else
                    {
                        switch (obj.Command.Type)
                        {
                            case AWAProtocol.CommandType.Message:
                                showMessage(((AWAMessage)obj).Data.Message);
                                break;

                            case AWAProtocol.CommandType.Error:
                                showMessage("ERROR! -- " + ((AWAError)obj).Data.Message);
                                break;

                            case AWAProtocol.CommandType.GameInit:
                                CreateGame(((AWAGameInit)obj).Data.Height, ((AWAGameInit)obj).Data.Width);
                                showMessage("Game initiated..");
                                break;

                            case AWAProtocol.CommandType.PlayerInit:
                                AWAPlayerInit p = ((AWAPlayerInit)obj);
                                CreatePlayer(p.Data.MoveType, p.Data.PlayerId, p.Data.XPos, p.Data.YPos);
                                showMessage("Player Init");
                                // TODO place player
                                break;

                            case AWAProtocol.CommandType.GameMove:
                                AWAGameMove m = ((AWAGameMove)obj);
                                //MovePlayer(m.Data.)
                                if (m.Data.MoveType == GameMoveType.InitiatePlayer)
                                {
                                    //TODO Create new opponent
                                    Player opponent = new Player(m.Data.PlayerId, m.Data.XPos, m.Data.YPos, 3);
                                    opponents.Add(opponent);
                                    MovePlayer(opponent, m.Data.XPos, m.Data.YPos, m.Data.Direction);
                                }
                                else
                                {
                                    if (m.Data.PlayerId == player.Id)
                                        MovePlayer(player, m.Data.XPos, m.Data.YPos, m.Data.Direction);
                                    else
                                        MovePlayer(opponents.Find(o => o.Id == m.Data.PlayerId), m.Data.XPos, m.Data.YPos, m.Data.Direction);
                                    // TODO move player
                                }
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
        private void CreatePlayer(GameMoveType moveType, int playerId, int xPos, int yPos)
        {
            if (moveType == GameMoveType.InitiatePlayer)
            {
                showMessage("Creating Player");
                player = new Player(playerId, xPos, yPos, 3);
                MovePlayer(player, xPos, yPos, MoveDirection.Left, true);
                //TODO Skapa en tråd som lyssnar på tangentbordet
            }

        }
        private void CreateGame(int h, int w)
        {
            GameFieldLabel.Visible = false;
            GameFieldPanel.Show();
            //Panel newPanel = new Panel();
            //for (int i = 0; i < h; i++)
            //{
            //    for (int j = 0; j < w; j++)
            //    {
            //        if (this.InvokeRequired)
            //        {
            //            Invoke((Action)(() =>
            //            {
            //                PictureBox pictureBox = new PictureBox();
            //                pictureBox.Location = new Point(0 + i * 32, 0 + j * 32);
            //                pictureBox.Size = new System.Drawing.Size(32, 32);
            //                pictureBox.BackColor = (i + j) % 2 == 0 ? Color.Black : Color.White;
            //                this.GameFieldPanel.Controls.Add(pictureBox);
            //            }));

            //        }
            //    }
            //}

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
        private void InitiateGame()
        {
            GameFieldPanel.Visible = true;

            //TODO skapa lyssnar-tråd och skickar-tråd
            Thread listenerThread = new Thread(Listen);
            listenerThread.Start();

        }
        private void sendObject(AWABase obj)
        {
            NetworkStream n = server.GetStream();
            BinaryWriter w = new BinaryWriter(n);
            w.Write(ProtocolUtils.Serialize(obj));
            w.Flush();
        }
        private void showMessage(string str)
        {
            if (this.InvokeRequired)
                Invoke((Action)(() => { messageBox.Items.Add(str); }));
            else
                messageBox.Items.Add(str);
        }

        private void MovePlayer(Player p, int xPos, int yPos, MoveDirection direction, bool create = false)
        {
            if (create)
            {

                if (this.InvokeRequired)
                    Invoke((Action)(() =>
                    {
                        PictureBox i = new PictureBox() { Name = p.Id.ToString() };
                        i.Name = player.Images[(int)direction].Name;
                        GameFieldPanel.Controls.Add(i);
//                        GameFieldPanel.Controls.Add(player.Images[(int)direction]);
                        player.Images[(int)direction].Location = new Point(xPos, yPos);
                        //GameFieldPanel.Controls.Find(player.Id.ToString(), false).First().BringToFront();
                        //GameFieldPanel.Controls.Find(player.Id.ToString(), false).First().BackColor = Color.Transparent;
                        //GameFieldPanel.Controls.Find(player.Id.ToString(), false).First().ForeColor = Color.Transparent;

                    }));
                else
                {
                    player.Images[(int)direction].Location = new Point(player.XPos, player.YPos);
                    GameFieldPanel.Controls.Add(player.Images[(int)direction]);
                }
            }


            if (this.InvokeRequired)
                Invoke((Action)(() =>
                {
                    showMessage(((int)direction).ToString());
                    var playerImage = GameFieldPanel.Controls.Find(player.Id.ToString(), false).First();
                    playerImage.Location = new Point(player.XPos, player.YPos);
                    ((PictureBox)playerImage).Image = player.Images[(int)direction].Image;
                }));
            else
            {
                var playerImage = GameFieldPanel.Controls.Find(player.Id.ToString(), false).First();
                playerImage.Location = new Point(player.XPos, player.YPos);
                ((PictureBox)playerImage).Image = player.Images[(int)direction].Image;
            }
        }



        private void Button_Click(object sender, EventArgs e)
        {
            MoveDirection direction = MoveDirection.Down;
            switch (((Button)sender).Name)
            {
                case nameof(UpButton):
                    direction = MoveDirection.Up;
                    player.YPos -= tileSize;
                    if (player.YPos < 0)
                        player.YPos = 0;
                    break;
                case nameof(RightButton):
                    direction = MoveDirection.Right;
                    player.XPos += tileSize;
                    if (player.XPos > GameFieldPanel.Width)
                        player.XPos = GameFieldPanel.Width;
                    break;
                case nameof(DownButton):
                    direction = MoveDirection.Down;
                    player.YPos += tileSize;
                    if (player.YPos > GameFieldPanel.Height)
                        player.YPos = GameFieldPanel.Height;
                    break;
                case nameof(LeftButton):
                    direction = MoveDirection.Left;
                    player.XPos -= tileSize;
                    if (player.XPos < 0)
                        player.XPos = 0;
                    break;
                default:
                    break;
            }
            sendObject(ProtocolUtils.CreateGameMove(GameMoveType.MovePlayer, player.Id, player.XPos, player.YPos, direction));
            showMessage("moveObject sent!");

        }

        //void Form1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    switch (e.KeyCode)
        //    {
        //        case Keys.Left:
        //            break;
        //        case Keys.Right:
        //            break;
        //        case Keys.Up:
        //            break;
        //        case Keys.Down:
        //            break;
        //    }
        //    showMessage("KeydOWN");
        //}
        //private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        //{
        //    switch (e.KeyCode)
        //    {
        //        case Keys.Left:
        //        case Keys.Right:
        //        case Keys.Up:
        //        case Keys.Down:
        //            e.IsInputKey = true;
        //            break;

        //        default:
        //            break;
        //    }
        //    showMessage("PreviewKeyDown");

        //}
        //protected override bool IsInputKey(Keys keyData)
        //{
        //    switch (keyData)
        //    {
        //        case Keys.Right:
        //        case Keys.Left:
        //        case Keys.Up:
        //        case Keys.Down:
        //            return true;
        //        case Keys.Shift | Keys.Right:
        //        case Keys.Shift | Keys.Left:
        //        case Keys.Shift | Keys.Up:
        //        case Keys.Shift | Keys.Down:
        //            return true;
        //    }
        //    return base.IsInputKey(keyData);
        //}
    }
}
