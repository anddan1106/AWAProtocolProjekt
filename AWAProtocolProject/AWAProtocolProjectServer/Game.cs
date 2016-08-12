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
        public int Height { get; set; } = 12;
        public int Width { get; set; } = 12;
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

                    Thread clientThread = new Thread(AddNewPlayer);
                    clientThread.Start(c);

                }
                Log.WriteLine("två spelare anslutna");

                //TODO skicka signal om att stara spelet med spelarnas positioner
                // lista med spelare (id, position)
                // storleken på planen
                foreach (Player player in players)
                {

                }

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


        public void AddNewPlayer(object o)
        {
            TcpClient c = (TcpClient)o;
            BinaryWriter w;
            string playerName = "";
            Log.WriteLine("user connecting");

            try
            {
                w = new BinaryWriter(c.GetStream());
                AWARequest request = ProtocolUtils.CreateRequest("1", RequestType.Username, "Choose your name."); //TODO fixa en Id-generator

                bool AskForUsername = true;
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
                        {
                            playerName = ((AWAResponse)obj).Data.Message;
                            lock (players)
                            {
                                if (!players.Exists(p => p.Name == playerName))
                                {
                                    players.Add(new Player(c, this, playerName, players.Count() == 0 ? 1 : players.Max(p => p.Id) + 1));
                                    AskForUsername = false;
                                }
                                else
                                    request.Data.Message = "Namnet är upptaget. Ange ett nytt namn.";
                            }
                        }
                    }
                    else
                    {
                        Log.WriteLine("incoming message was not valid");
                    }

                } while (AskForUsername);

                w.Write(ProtocolUtils.Serialize(ProtocolUtils.CreateOk("Username ok")));
                w.Flush();

            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);

            }

            Log.WriteLine($"{playerName}: ansluten");
            Player newPlayer = players.SingleOrDefault(p => p.Name == playerName);

            string Json = $"{{\"Height\" : {Height}, \"Width\" : {Width}}}";
            w = new BinaryWriter(c.GetStream());
            w.Write(ProtocolUtils.Serialize(ProtocolUtils.CreateGameInit(Height, Width)));
            w.Flush();
            w.Write(ProtocolUtils.Serialize(ProtocolUtils.CreatePlayerInit(newPlayer.Id, 0, 0, MoveDirection.Down)));
            w.Flush();

            Listen(newPlayer);

        }

        private void Listen(Player player)
        {
            while (player.IsAlive)
            {
                //TODO Listen for player input
                var obj = ProtocolUtils.Deserialize(new BinaryReader(player.c.GetStream()).ReadString());
                if (obj != null)
                {
                    if(obj.Command.Type == CommandType.GameMove)
                    {
                        Log.WriteLine("gameMove");
                        foreach (var p in players)
                        {
                            var w = new BinaryWriter(p.c.GetStream());
                            w.Write(ProtocolUtils.Serialize((AWAGameMove)obj));
                            w.Flush();
                        }
                    }
                    else if (obj.Command.Type == CommandType.Message)
                    {
                        Log.WriteLine("Message");
                        foreach (var p in players)
                        {
                            var w = new BinaryWriter(p.c.GetStream());
                            w.Write(ProtocolUtils.Serialize((AWAMessage)obj));
                            w.Flush();
                        }

                    }
                }                
            }

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
