using AWAProtocol;
using AWAProtocolUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AWAProtocolProjectServer
{
    public class Game
    {
        List<Player> players = new List<Player>();
        public Tile[][] GameField { get; set; }
        public int Height { get; set; } = 10;
        public int Width { get; set; } = 10;
        public bool IsAlive { get; private set; }

        public void Run()
        {
            IsAlive = true;
            Init();
            WaitForPlayers();

        }

        private void WaitForPlayers()
        {
            //Listen for connecting players

            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            Log.WriteLine("Server up and running, waiting for Players...");

            try
            {
                listener.Start();

                while (players.Count() < 2)
                {
                    TcpClient c = listener.AcceptTcpClient();

                    Log.WriteLine("user connecting");

                    BinaryWriter w = new BinaryWriter(c.GetStream());
                    AWARequest request = ProtocolUtils.CreateRequest("1", RequestType.Username, "Choose your name."); //TODO fixa en Id-generator
                    string clientName = "";

                    do
                    {
                        w.Write(ProtocolUtils.Serialize(request));
                        w.Flush();
                        Log.WriteLine("server sending request");
                        var obj = ProtocolUtils.Deserialize(new BinaryReader(c.GetStream()).ReadString());

                        Log.WriteLine("connected user response");


                        if (obj != null)
                        {
                            if (obj.Command.Type == CommandType.Response
                                && ((AWAResponse)obj).Data.ResponseType == ResponseType.Username)
                                clientName = ((AWAResponse)obj).Data.Message;

                            request.Data.Message = "Namnet är upptaget. Ange ett nytt namn.";
                        }
                        else
                        {
                            Log.WriteLine("incoming message was not valid");
                        }

                    } while (players.Exists(p => p.Name == clientName));

                    //TODO skicka ett namn-ok..
                    w.Write(ProtocolUtils.Serialize(ProtocolUtils.CreateOk("Username ok")));
                    w.Flush();

                    AddNewPlayer(c, clientName);
                    Log.WriteLine($"{clientName}: ansluten");

                }
                Log.WriteLine("två spelare anslutna");

                //TODO skicka signal om att stara spelet med spelarnas positioner
                string Json = $"{{{nameof(Height)} : {Height}, {nameof(Width)} : {Width}}}";
                foreach (Player player in players)
                {
                    BinaryWriter w = new BinaryWriter(player.c.GetStream());
                    w.Write(ProtocolUtils.Serialize(ProtocolUtils.CreateMove(Json,CommandType.GameInit, GameMoveType.InitiateField)));
                    w.Flush();
                }
                // lista med spelare (id, position)
                // storleken på planen

            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }

        }

        private void AddNewPlayer(TcpClient c, string clientName)
        {
            Player newPlayer = new Player(c, this, clientName);

            Thread clientThread = new Thread(newPlayer.Run);
            clientThread.Start();

            players.Add(newPlayer);
        }
        public void RemovePlayer(Player player)
        {
            players.Remove(player);

            if (players.Count() > 0)
            { }
            //TODO send message to other players
            else
                IsAlive = false;
        }

        public void Init()
        {
            // Initate Game field and create Tile-objects

            GameField = new Tile[Height][];
            for (int i = 0; i < Height; i++)
            {
                GameField[i] = new Tile[Width];
                for (int j = 0; j < Width; j++)
                {
                    GameField[i][j] = new Tile(i, j);
                }
            }
            Log.WriteLine("Game initiated!");
        }
    }
}
