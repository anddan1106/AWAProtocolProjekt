using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWARequest : AWABase
    {
        public new AWARequestData Data { get; set; }

        public AWARequest(string id, RequestType requestFor, string message, string version)
        {
            Version = version;
            Command = new AWACommand(CommandType.Request);
            Data = new AWARequestData(id, requestFor, message);

        }

    }
}
