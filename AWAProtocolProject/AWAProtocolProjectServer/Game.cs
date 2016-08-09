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
    class Game
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
            Console.WriteLine("Server up and running, waiting for Players...");

            try
            {
                listener.Start();

                while (players.Count() < 2)
                {
                    TcpClient c = listener.AcceptTcpClient();

                    BinaryWriter w = new BinaryWriter(c.GetStream());
                    AWAMessage message = ProtocolUtils.CreateMessage("Ange ditt namn.");
                    string clientName = "";

                    do
                    {
                        w.Write(ProtocolUtils.Serialize(message));
                        w.Flush();
                        clientName = new BinaryReader(c.GetStream()).ReadString();
                        message.Data.Message = "Namnet är upptaget." + Environment.NewLine + "Ange ett nytt namn.";
                    } while (players.Exists(p => p.Name == clientName));

                    AddNewPlayer(c, clientName);

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
            Console.WriteLine("Game initiated!");
        }
    }
}
