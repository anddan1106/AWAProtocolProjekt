using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAGameMoveData
    {
        public string Json { get; set; }
        public GameMoveType MoveType { get; set; }
        public AWAGameMoveData(string json, GameMoveType moveType)
        {
            MoveType = moveType;
            Json = json;
        }
    }
}
