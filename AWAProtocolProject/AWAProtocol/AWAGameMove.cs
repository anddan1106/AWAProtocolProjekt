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
        public AWAGameMove(string version, CommandType commandType, GameMoveType moveType, string jsonData)
        {
            Command = new AWACommand(commandType);
            Version = version;
            Data = new AWAGameMoveData(jsonData, moveType);
        }
    }
}
