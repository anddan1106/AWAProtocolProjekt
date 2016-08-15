using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAMessage : AWABase
    {
        public new AWAMessageData Data { get; set; }
        public AWAMessage(string version, string message, int senderId)
        {
            Version = version;
            Command = new AWACommand(CommandType.Message);
            Data = new AWAMessageData(message, senderId);
        }
    }
}
