using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAError : AWABase
    {
        public new AWAErrorData Data { get; set; }
        public AWAError(string version, int code)
        {
            Version = version;
            Command = new AWACommand("error");
            Data = new AWAErrorData(code);
        }
    }
}
