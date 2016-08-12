using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAPlayerInit : AWABase
    {
        public new AWAGameMoveData Data { get; set; }
        public AWAPlayerInit(string version, GameMoveType moveType, int playerId, int xPos, int yPos, MoveDirection direction)
        {
            Version = version;
            Command = new AWACommand(CommandType.PlayerInit);
            Data = new AWAGameMoveData(moveType, playerId, xPos, yPos, direction);
        }
    }
}
