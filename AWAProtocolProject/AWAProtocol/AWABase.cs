using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWABase
    {

        public string Version { get; set; }
        public AWACommand Command { get; set; }
        public AWAData Data { get; set; }



    }
    public enum RequestType { Username };
    public enum ResponseType { Username };
    public enum CommandType { Error, Message, Request, Response, Ok , GameMove, GameInit};
    public enum GameMoveType { InitiateField, InitiatePlayers, PlayerMove}


}
