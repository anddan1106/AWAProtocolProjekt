using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocolProjectServer
{
    public class Tile
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public List<Player> Players { get; set; }
        
        public Tile(int x, int y)
        {
            XPos = x;
            YPos = y;
            Players = new List<Player>();
        }
    }
}
