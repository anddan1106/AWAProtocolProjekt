using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocolProjectServer
{
    class Tile
    {
        private char icon;
        public int XPos { get; set; }
        public int YPos { get; set; }
        public List<Player> Players { get; set; }
        public char Icon
        {
            get { return Players.Count() == 0 ? icon : Players.FirstOrDefault().Icon; }

            set { icon = value; }
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
