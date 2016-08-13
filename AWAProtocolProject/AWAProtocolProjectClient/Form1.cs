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
        private Button focusObject = new Button();

        public Form1()
        {
            InitializeComponent();
            ConnectTextBox.Text = GetLocalIP();

            this.KeyPreview = true;
            this.Controls.Add(focusObject);
            this.focusObject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Button_KeyDown);
            this.focusObject.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Button_PreviewKeyDown);
            this.focusObject.Location = new System.Drawing.Point(10, 10);
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

                } while (obj == null || obj.Command.Type != AWAProtocol.CommandType.Request
                || ((AWARequest)obj).Data.RequestFor != RequestType.Username);

                showMessage("Server asking for username");
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
            sendMessage();
        }
        private void sendMessage()
        {
            sendObject(ProtocolUtils.CreateMessage($"[{player.Name}]: {messageText.Text}", player.Id));
            showMessage(messageText.Text);
            messageText.Text = "";
            ActiveControl = focusObject;
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
                                CreatePlayer(p.Data.MoveType, p.Data.PlayerId, p.Data.Name, p.Data.XPos, p.Data.YPos);
                                break;

                            case AWAProtocol.CommandType.GameMove:
                                AWAGameMove move = ((AWAGameMove)obj);
                                if (move.Data.MoveType == GameMoveType.InitiatePlayer)
                                {
                                    Player opponent = new Player(move.Data.PlayerId, move.Data.Name, move.Data.XPos, move.Data.YPos, 3);
                                    opponents.Add(opponent);
                                    MovePlayer(opponent, move.Data.XPos, move.Data.YPos, move.Data.Direction, true);
                                }
                                else
                                {
                                    if (move.Data.PlayerId == player.Id)
                                        MovePlayer(player, move.Data.XPos, move.Data.YPos, move.Data.Direction);
                                    else
                                        MovePlayer(opponents.Find(o => o.Id == move.Data.PlayerId), move.Data.XPos, move.Data.YPos, move.Data.Direction);
                                    //showMessage("opponent : " + opponents.Find(o => o.Id == move.Data.PlayerId).Id.ToString());
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

        private void InitiateGame()
        {
            GameFieldPanel.Visible = true;
            ActiveControl = focusObject;

            Thread listenerThread = new Thread(Listen);
            listenerThread.Start();

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
        private void CreatePlayer(GameMoveType moveType, int playerId, string name, int xPos, int yPos)
        {
            if (moveType == GameMoveType.InitiatePlayer)
            {
                showMessage("Creating Player");
                player = new Player(playerId, name, xPos, yPos, 3);
                MovePlayer(player, xPos, yPos, MoveDirection.Down, true);
            }

        }
        private void MovePlayer(Player p, int xPos, int yPos, MoveDirection direction, bool create = false)
        {
            if (create)
            {

                if (this.InvokeRequired)
                    Invoke((Action)(() =>
                    {
                        PictureBox i = new PictureBox() { Name = p.Id.ToString() };
                        i.Name = p.Images[(int)direction].Name;
                        i.Image = p.Images[(int)direction].Image;
                        i.Size = new Size(32, 32);
                        i.Location = new Point(xPos, yPos);
                        GameFieldPanel.Controls.Add(i);
                        GameFieldPanel.Controls.Find(p.Id.ToString(), false).First().BringToFront();
                        //GameFieldPanel.Controls.Find(player.Id.ToString(), false).First().BackColor = Color.Transparent;
                        //GameFieldPanel.Controls.Find(player.Id.ToString(), false).First().ForeColor = Color.Transparent;

                    }));
                else
                {
                    p.Images[(int)direction].Location = new Point(xPos, yPos);
                    GameFieldPanel.Controls.Add(p.Images[(int)direction]);
                }
            }
            else
            {
                if (this.InvokeRequired)
                    Invoke((Action)(() =>
                    {
                        var playerImage = GameFieldPanel.Controls.Find(p.Id.ToString(), false).First();
                        playerImage.Location = new Point(xPos, yPos);
                        ((PictureBox)playerImage).Image = p.Images[(int)direction].Image;
                    }));
                else
                {
                    var playerImage = GameFieldPanel.Controls.Find(p.Id.ToString(), false).First();
                    playerImage.Location = new Point(xPos, yPos);
                    ((PictureBox)playerImage).Image = p.Images[(int)direction].Image;
                }
            }
        }


        private void showMessage(string str)
        {
            if (this.InvokeRequired)
                Invoke((Action)(() => { messageBox.Items.Add(str); }));
            else
                messageBox.Items.Add(str);
        }
        private void sendObject(AWABase obj)
        {
            NetworkStream n = server.GetStream();
            BinaryWriter w = new BinaryWriter(n);
            w.Write(ProtocolUtils.Serialize(obj));
            w.Flush();
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

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            MoveDirection direction;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    direction = MoveDirection.Up;
                    player.YPos -= tileSize;
                    if (player.YPos < 0)
                        player.YPos = 0;
                    break;
                case Keys.Right:
                    direction = MoveDirection.Right;
                    player.XPos += tileSize;
                    if (player.XPos > GameFieldPanel.Width - 32)
                        player.XPos = GameFieldPanel.Width - 32;
                    break;
                case Keys.Down:
                    direction = MoveDirection.Down;
                    player.YPos += tileSize;
                    if (player.YPos > GameFieldPanel.Height - 32)
                        player.YPos = GameFieldPanel.Height - 32;
                    break;
                case Keys.Left:
                    direction = MoveDirection.Left;
                    player.XPos -= tileSize;
                    if (player.XPos < 0)
                        player.XPos = 0;
                    break;
                default:
                    return;
            }
            sendObject(ProtocolUtils.CreateGameMove(GameMoveType.MovePlayer, player.Id, player.Name, player.XPos, player.YPos, direction));

        }

        private void Button_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                case Keys.Left:
                    e.IsInputKey = true;
                    break;
                default:
                    return;
            }

        }

        private void messageButton_KeyDown(object sender, KeyEventArgs e)
        {
            showMessage("presses key");
        }

        private void messageButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Right:
                case Keys.Up:
                case Keys.Left:
                    e.IsInputKey = true;
                    break;
            }
        }

        private void GameFieldPanel_Click(object sender, EventArgs e)
        {
            this.ActiveControl = focusObject;
        }

        private void messageText_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                sendMessage();
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
