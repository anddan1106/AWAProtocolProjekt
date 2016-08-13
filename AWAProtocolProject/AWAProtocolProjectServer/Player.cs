using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocolProjectServer
{
    public class Player
    {
        public TcpClient c;
        public Game game;

        public string Name { get; set; }
        public bool IsAlive { get; set; }
        public int Id { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }


        public Player(TcpClient c, Game game, string name, int id, int xPos, int yPos)
        {
            this.c = c;
            this.game = game;
            Name = name;
            Id = id;
            XPos = xPos;
            YPos = yPos;

            IsAlive = true;
        }
    }
}
