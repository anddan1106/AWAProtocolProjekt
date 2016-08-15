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
using WMPLib;

namespace AWAProtocolProjectClient
{
    public partial class Form1 : Form
    {
        WMPLib.WindowsMediaPlayer soundPlayer = new WMPLib.WindowsMediaPlayer();
        // System.Media.SoundPlayer sound = new System.Media.SoundPlayer ();
        private TcpClient server;

        private string serverIP = "?";
        private Player player;
        private List<Player> opponents = new List<Player>();
        private int tileSize = 32;
        private Button focusObject = new Button();

        public Form1()
        {
            InitializeComponent();
            soundPlayer.URL = @"C:\Projects\_Repos\AWAProtocolProjekt\AWAProtocolProject\AWAProtocolProjectClient\Resources\MySound.mp3.mp3";
            // sound.SoundLocation = "MySound.wav";
            soundPlayer.controls.play();
            ConnectTextBox.Text = GetLocalIP();

            this.KeyPreview = true;
            this.Controls.Add(focusObject);
            this.focusObject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Button_KeyDown);
            this.focusObject.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Button_PreviewKeyDown);
            this.focusObject.Location = new System.Drawing.Point(10, 10);
            this.ActiveControl = ConnectTextBox;
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
                UsernameTextBox.Focus();
            }
            catch (Exception)
            {
                showMessage("Connection was not estabished.. is the server IP correct?");
                //TODO add error message and possibliy try again
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
                    if (obj.Command.Type == AWAProtocol.CommandType.Request
                        && ((AWARequest)obj).Data.RequestFor == RequestType.Username)
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
                showMessage("something went wrong, looks like the server is down :(");
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
                                Label playerLable = new Label();
                                playerLable.Name = p.Data.PlayerId + "healthLabel";
                                playerLable.Text = p.Data.Name + ":" + Environment.NewLine + $"{10}";
                                playerLable.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Underline, GraphicsUnit.Point, ((byte)(0)));
                                playerLable.Location = new Point(0, 0);
                                playerLable.Size = new Size(130, 50);
                                playerLable.TabIndex = 0;
                                if (this.InvokeRequired)
                                    Invoke((Action)(() =>
                                    {
                                        this.healthPanel.Controls.Add(playerLable);
                                    }));
                                //healthPanel.BringToFront();
                                break;

                            case AWAProtocol.CommandType.GameMove:
                                AWAGameMove move = ((AWAGameMove)obj);
                                if (move.Data.MoveType == GameMoveType.InitiatePlayer)
                                {
                                    Player newOpponent = new Player(move.Data.PlayerId, move.Data.Name, move.Data.XPos, move.Data.YPos, 3);
                                    opponents.Add(newOpponent);
                                    MovePlayer(newOpponent, move.Data.XPos, move.Data.YPos, move.Data.Direction, true);
                                    Label opponentLable = new Label();
                                    opponentLable.Name = newOpponent.Id + "healthLabel";
                                    opponentLable.Text = newOpponent.Name + ":" + Environment.NewLine + $"{newOpponent.Health}";
                                    opponentLable.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Underline, GraphicsUnit.Point, ((byte)(0)));
                                    opponentLable.Location = new Point(0, 50 * healthPanel.Controls.Count);
                                    opponentLable.Size = new Size(130, 50);
                                    opponentLable.TabIndex = 0;
                                    if (this.InvokeRequired)
                                        Invoke((Action)(() =>
                                        {
                                            this.healthPanel.Controls.Add(opponentLable);
                                        }));
                                }
                                else
                                {
                                    if (move.Data.PlayerId == player.Id)
                                        MovePlayer(player, move.Data.XPos, move.Data.YPos, move.Data.Direction);
                                    else
                                        MovePlayer(opponents.Find(o => o.Id == move.Data.PlayerId), move.Data.XPos, move.Data.YPos, move.Data.Direction);
                                }
                                break;

                            case AWAProtocol.CommandType.GameAttack:
                                //TODO handle game attack mysfaktor says Dennis
                                break;
                            case AWAProtocol.CommandType.PlayerHit:
                                AWAPlayerHit hit = (AWAPlayerHit)obj;
                                Player opponent = opponents.SingleOrDefault(o => o.Id == hit.Data.VictimId);
                                if (opponent != null)
                                {
                                    opponent.Health = hit.Data.NewHealth;
                                    if (this.InvokeRequired)
                                        Invoke((Action)(() =>
                                        {
                                            healthPanel.Controls
                         .Find($"{opponent.Id}healthLabel", false).SingleOrDefault()
                         .Text = opponent.Name + ":" + Environment.NewLine + $"{opponent.Health}";
                                        }));
                                }
                                else if (player.Id == hit.Data.VictimId)
                                {
                                    player.Health = hit.Data.NewHealth;
                                    if (this.InvokeRequired)
                                        Invoke((Action)(() =>
                                        {
                                            healthPanel.Controls
                         .Find($"{player.Id}healthLabel", false).SingleOrDefault()
                         .Text = player.Name + ":" + Environment.NewLine + $"{player.Health}";
                                        }));
                                }
                                break;
                            case AWAProtocol.CommandType.GameOver:
                                AWAGameOver gameOver = (AWAGameOver)obj;

                                if (gameOver.Data.Id == player.Id)
                                {
                                    showMessage("YOU DIED! TOO BAD!");
                                    focusObject.Enabled = false;
                                    GameFieldPanel.Controls.RemoveByKey(gameOver.Data.Id.ToString());
                                }
                                else
                                {
                                    opponents.RemoveAll(o => o.Id == gameOver.Data.Id);
                                    GameFieldPanel.Controls.RemoveByKey(gameOver.Data.Id.ToString());
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
                showMessage("something went wrong, looks like the server is down :(");
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
            p.CurrentDirection = direction;
            if (create)
            {

                if (this.InvokeRequired)
                    Invoke((Action)(() =>
                    {
                        PictureBox i = new PictureBox() { Name = p.Id.ToString() };
                        i.BackColor = Color.Transparent;
                        //                        i.Name = p.Images[(int)p.CurrentDirection].Name;
                        i.Image = p.Images[(int)p.CurrentDirection].Image;
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
                    GameFieldPanel.Controls.Add(p.Images[(int)p.CurrentDirection]);
                }
            }
            else
            {
                if (this.InvokeRequired)
                    Invoke((Action)(() =>
                    {
                        var playerImage = GameFieldPanel.Controls.Find(p.Id.ToString(), false).First();
                        playerImage.Location = new Point(xPos, yPos);
                        ((PictureBox)playerImage).Image = p.Images[(int)p.CurrentDirection].Image;
                    }));
                else
                {
                    var playerImage = GameFieldPanel.Controls.Find(p.Id.ToString(), false).First();
                    playerImage.Location = new Point(xPos, yPos);
                    ((PictureBox)playerImage).Image = p.Images[(int)p.CurrentDirection].Image;
                }
            }
        }

        // skriver ut ett meddelande i messageBox, om funktionen anropas av en obehörig tråd skickas en delegat till ägandetråden
        private void showMessage(string str)
        {
            if (this.InvokeRequired)
                Invoke((Action)(() =>
                {
                    messageBox.Items.Add(str);
                    messageBox.TopIndex = messageBox.Items.Count - 1;
                }));
            else
            {
                messageBox.Items.Add(str);
                messageBox.TopIndex = messageBox.Items.Count - 1;
            }


        }

        // används för att skicka meddelande enligt protokollet till servern
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

        // hantering av piltangenterna för förflyttning
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
                case Keys.Space:
                    //TODO skulle kunna intergrera med gamemove.
                    sendObject(ProtocolUtils.CreateGameAttack(player.Id, player.CurrentDirection, player.attackDamage, player.XPos, player.YPos));
                    return;
                default:
                    return;
            }
            sendObject(ProtocolUtils.CreateGameMove(GameMoveType.MovePlayer, player.Id, player.Name, player.XPos, player.YPos, direction));

        }

        // Fånga upp piltangenter för att undvika att focus flyttas 
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
            }

        }

        //private void messageButton_KeyDown(object sender, KeyEventArgs e)
        //{
        //    showMessage("presses key");
        //}

        // Fånga upp piltangenter för att undvika att focus ändras 
        //private void messageButton_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        //{
        //    switch (e.KeyCode)
        //    {
        //        case Keys.Down:
        //        case Keys.Right:
        //        case Keys.Up:
        //        case Keys.Left:
        //            e.IsInputKey = true;
        //            break;
        //    }
        //}

        // Ändra fokus till focusObject när man klickar på GameFieldPanel
        private void GameFieldPanel_Click(object sender, EventArgs e)
        {
            this.ActiveControl = focusObject;
        }

        // Hantera enter tangenten som skicka-knapp
        private void messageText_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                sendMessage();
        }

        private void ConnectTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ConnectButton_Click(null, null);
        }

        private void UsernameTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                UsernameButton_Click(null, null);
        }
    }
}
