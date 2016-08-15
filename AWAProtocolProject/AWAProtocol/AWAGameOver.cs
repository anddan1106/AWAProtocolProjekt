using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAGameOver : AWABase
    {
        public new AWAGameOverData Data { get; set; }
        public AWAGameOver(string version, int id)
        {
            Version = version;
            Command = new AWACommand(CommandType.GameOver);
            Data = new AWAGameOverData(id);

        }
    }
}
