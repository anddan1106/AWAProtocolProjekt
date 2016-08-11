using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAOk : AWABase
    {
        public new AWAOkData Data { get; set; }
        public AWAOk(string version, string message)
        {
            Version = version;
            Command = new AWACommand(CommandType.Ok);
            Data = new AWAOkData(message);
        }

    }
}
