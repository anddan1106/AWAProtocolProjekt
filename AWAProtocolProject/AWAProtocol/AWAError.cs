using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAError : AWABase
    {
        public new AWAMessageData Data { get; set; }
        public AWAError(string version, string message)
        {
            Version = version;
            Command = new AWACommand("error");
            Data = new AWAMessageData(message);
        }
    }
}
