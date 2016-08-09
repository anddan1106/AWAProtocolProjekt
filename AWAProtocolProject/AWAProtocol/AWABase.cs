using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public abstract class AWABase
    {
        public string Version { get; set; }
        public AWACommand Command { get; set; }
        public AWAData Data { get; set; }



    }
}
