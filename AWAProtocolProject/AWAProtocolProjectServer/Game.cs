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
        int tileSize = 32;
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

                //foreach (Player player in players)
                //{

                //}

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
                AWARequest request = ProtocolUtils.CreateRequest("1", RequestType.Username, "Choose your name.");

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
                                    players.Add(new Player(c, this, playerName, players.Count() == 0 ? 1 : players.Max(p => p.Id) + 1, 0, 0));
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

            Player newPlayer = players.SingleOrDefault(p => p.Name == playerName);
            if (newPlayer != null)
            {

                Log.WriteLine($"{playerName} id {newPlayer.Id}: ansluten");

                string Json = $"{{\"Height\" : {Height}, \"Width\" : {Width}}}";
                w = new BinaryWriter(c.GetStream());
                w.Write(ProtocolUtils.Serialize(ProtocolUtils.CreateGameInit(Height, Width)));
                w.Flush();
                w.Write(ProtocolUtils.Serialize(ProtocolUtils.CreatePlayerInit(
                    newPlayer.Name, newPlayer.Id, newPlayer.XPos, newPlayer.YPos, MoveDirection.Down)));
                w.Flush();
                foreach (var p in players)
                {
                    if (p.Id != newPlayer.Id)
                    {
                        BinaryWriter writer = new BinaryWriter(p.c.GetStream());
                        writer.Write(ProtocolUtils.Serialize(ProtocolUtils.CreateGameMove(
                            GameMoveType.InitiatePlayer, newPlayer.Id, newPlayer.Name, newPlayer.XPos, newPlayer.YPos, MoveDirection.Down)));
                        writer.Flush();
                        writer = new BinaryWriter(newPlayer.c.GetStream());
                        writer.Write(ProtocolUtils.Serialize(ProtocolUtils.CreateGameMove(
                            GameMoveType.InitiatePlayer, p.Id, p.Name, p.XPos, p.YPos, MoveDirection.Down)));
                        Log.WriteLine("player" + p.Name + " : " + p.XPos + " : " + p.YPos);
                        writer.Flush();
                    }
                }

                Listen(newPlayer);

            }
        }

        private void Listen(Player player)
        {
            while (player.IsAlive)
            {
                var obj = ProtocolUtils.Deserialize(new BinaryReader(player.c.GetStream()).ReadString());
                if (obj != null)
                {
                    switch (obj.Command.Type)
                    {
                        case CommandType.GameMove:
                            var move = (AWAGameMove)obj;
                            Log.WriteLine(move.Data.PlayerId + " moved to " + move.Data.XPos + ":" + move.Data.YPos);
                            Player movingPlayer = players.Find(p => p.Id == move.Data.PlayerId);
                            movingPlayer.XPos = move.Data.XPos;
                            movingPlayer.YPos = move.Data.YPos;
                            foreach (var p in players)
                            {
                                var w = new BinaryWriter(p.c.GetStream());
                                w.Write(ProtocolUtils.Serialize((AWAGameMove)obj));
                                w.Flush();
                            }
                            break;
                        case CommandType.Message:
                            foreach (var p in players)
                            {
                                if (p.Id != ((AWAMessage)obj).Data.SenderId)
                                {
                                    var w = new BinaryWriter(p.c.GetStream());
                                    w.Write(ProtocolUtils.Serialize((AWAMessage)obj));
                                    w.Flush();
                                }
                            }
                            break;
                        case CommandType.GameAttack:
                            int hitX = ((AWAGameAttack)obj).Data.XPos;
                            int hitY = ((AWAGameAttack)obj).Data.YPos;
                            switch (((AWAGameAttack)obj).Data.Direction)
                            {
                                case MoveDirection.Up:
                                    hitY -= tileSize;
                                    break;
                                case MoveDirection.Right:
                                    hitX += tileSize;
                                    break;
                                case MoveDirection.Down:
                                    hitY += tileSize;
                                    break;
                                case MoveDirection.Left:
                                    hitX -= tileSize;
                                    break;
                                default:
                                    break;
                            }
                            Player killedPlayer = null;
                            foreach (Player p in players)
                            {
                                var w = new BinaryWriter(p.c.GetStream());
                                w.Write(ProtocolUtils.Serialize((AWAGameAttack)obj));
                                w.Flush();
                                if (p.XPos == hitX && p.YPos == hitY)
                                {
                                    p.Health -= ((AWAGameAttack)obj).Data.Damage;
                                    if (p.Health < 1)
                                        killedPlayer = p;
                                    string attacker = players.SingleOrDefault(o => o.Id == ((AWAGameAttack)obj).Data.Id).Name;
                                    foreach (Player p2 in players)
                                    {
                                        w = new BinaryWriter(p2.c.GetStream());
                                        w.Write(ProtocolUtils.Serialize(ProtocolUtils.CreatePlayerHit(p.Id, ((AWAGameAttack)obj).Data.Id, p.Health)));
                                        w.Flush();
                                    }
                                }
                            }
                            if (killedPlayer != null)
                                RemovePlayer(killedPlayer.Id);

                            break;
                    }


                }
            }

        }

        public void RemovePlayer(int id)
        {
            foreach (Player p in players)
            {
                var w = new BinaryWriter(p.c.GetStream());
                w.Write(ProtocolUtils.Serialize(ProtocolUtils.CreateGameOver(id)));
                w.Flush();
            }
            players.RemoveAll(p => p.Id == id);
            if (players.Count <= 0)
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
