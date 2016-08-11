using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AWADoD
{
    public class Player
    {
        private TcpClient c;
        private Game game;

        public string Name { get; set; }
        public char Icon { get; set; }
        public bool IsAlive { get; set; }


        public Player(TcpClient c, Game game, string name)
        {
            this.c = c;
            this.game = game;
            Name = name;
            Icon = 'P';
            IsAlive = true;
        }

        public void Run()
        {
            try
            {
                string message = "";
                while (IsAlive)
                {
                    NetworkStream n = c.GetStream();
                    message = new BinaryReader(n).ReadString();
                    //TODO recieve message from player.
                }

                game.RemovePlayer(this);
                c.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
