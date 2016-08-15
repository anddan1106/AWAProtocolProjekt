using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAGameMoveData
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public int PlayerId { get; set; }
        public GameMoveType MoveType { get; set; }
        public MoveDirection Direction { get; set; }
        public string Name { get; set; }
        public AWAGameMoveData(GameMoveType moveType, int playerId, string name, int xPos, int yPos, MoveDirection direction)
        {
            Direction = direction;
            MoveType = moveType;
            PlayerId = playerId;
            Name = name;
            XPos = xPos;
            YPos = yPos;
        }
    }
}
