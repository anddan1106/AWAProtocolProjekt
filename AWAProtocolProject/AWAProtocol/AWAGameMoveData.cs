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
        public AWAGameMoveData(GameMoveType moveType, int playerId, int xPos, int yPos)
        {
            MoveType = moveType;
            PlayerId = playerId;
            XPos = xPos;
            YPos = yPos;
        }
    }
}
