using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWABase
    {
        public AWAVersion Version { get; set; }
        public AWACommand Command { get; set; }
        public AWAData Data { get; set; }
        public AWABase(AWAVersion version, AWACommand command, AWAData data)
        {
            Version = version;
            Command = command;
            Data = data;
        }

        public override string ToString()
        {
            return "{" + Version.ToString() 
                + "," + Command.ToString() 
                + "," + Data.ToString() + "}";
        }
    }
}
