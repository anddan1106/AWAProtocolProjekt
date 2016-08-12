using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAGameMove : AWABase
    {
        public new AWAGameMoveData Data { get; set; }
        public AWAGameMove(string version, GameMoveType moveType, int playerId, int xPos, int yPos, MoveDirection direction)
        {
            Command = new AWACommand(CommandType.GameMove);
            Version = version;
            Data = new AWAGameMoveData(moveType, playerId, xPos, yPos, direction);
        }
    }
}
