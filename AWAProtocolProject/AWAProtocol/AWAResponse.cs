using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAResponse : AWABase
    {
        public new AWAResponseData Data { get; set; }

        public AWAResponse(string id, ResponseType responseType, string message, string version)
        {
            Version = version;
            Command = new AWACommand(CommandType.Response);
            Data = new AWAResponseData(id, responseType, message);

        }

    }
}
