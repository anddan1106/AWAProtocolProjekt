using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocolProjectServer
{
    class Tile
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public List<Player> Players { get; set; }
        public char Icon
        {
            get { return Players.Count() == 0 ? Icon : Players.FirstOrDefault().Icon; }

            set { Icon = value; }
        }

        public Tile(int x, int y)
        {
            XPos = x;
            YPos = y;
            Players = new List<Player>();
            Icon = 'o';
        }
    }
}
