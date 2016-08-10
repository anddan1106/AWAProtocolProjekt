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
        public static AWABase Deserialize(string data)
        {

            try
            {
                var obj = JsonConvert.DeserializeObject<AWABase>(data);

                if (obj.Command != null 
                    && obj.Version != null 
                    && obj.Data != null)
                {

                    switch (obj.Command.Type)
                    {
                        case "message":
                            var msg = JsonConvert.DeserializeObject<AWAMessage>(data);
                            if (msg.Data.Message != null) 
                                return msg;
                            break;
                        case "error":
                            var err = JsonConvert.DeserializeObject<AWAError>(data);
                            if (err.Data.Message != null && err.Data.Code > 0)
                                return err;
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
                //return new AWAError("1.0", "Incoming data was not a valid protocol object");
            }
            return null;
        }

        public static AWAMessage CreateMessage(string message, string version = "1.0")
        {
            return new AWAMessage(version, message);
        }

        public static AWAError CreateError(int code, string version = "1.0")
        {
            return new AWAError(version, code);
        }

        public static string Serialize(AWABase obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
