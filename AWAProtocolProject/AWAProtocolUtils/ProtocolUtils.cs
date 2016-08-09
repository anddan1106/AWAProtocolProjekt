using AWAProtocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocolUtils
{
    public class ProtocolUtils
    {
        public AWABase Parse(string data) {
            try
            {
                var json = JsonConvert.DeserializeObject<AWABase>(data);

            }
            catch (Exception ex)
            {
                return new AWAError("1.0", "Incoming data was not a valid protocol object");
            }
            return null;
        }

        public static AWAMessage CreateMessage(string message)
        {
            return new AWAMessage("1.0", message);
        }

        public static string Serialize(AWABase obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
