using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAPlayerRemove : AWABase
    {
        public new AWAPlayerRemoveData Data { get; set; }
        public AWAPlayerRemove(string version, int id)
        {
            Version = version;
            Command = new AWACommand(CommandType.PlayerRemove);
            Data = new AWAPlayerRemoveData(id);
        }
    }
}
