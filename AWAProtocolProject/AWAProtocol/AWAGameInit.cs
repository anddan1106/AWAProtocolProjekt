using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAGameInit : AWABase
    {
        public new AWAGameInitData Data { get; set; }
        public AWAGameInit(string version, int height, int width)
        {
            Version = version;
            Command = new AWACommand(CommandType.GameInit);
            Data = new AWAGameInitData(height, width);
        }
    }
}
