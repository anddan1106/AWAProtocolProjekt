using AWAProtocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AWAProtocol.AWABase;

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
                        case CommandType.Message:
                            var msg = JsonConvert.DeserializeObject<AWAMessage>(data);
                            if (msg.Data.Message != null)
                                return msg;
                            break;

                        case CommandType.Error:
                            var err = JsonConvert.DeserializeObject<AWAError>(data);
                            if (err.Data.Message != null && err.Data.Code > 0)
                                return err;
                            break;

                        case CommandType.Request:
                            var req = JsonConvert.DeserializeObject<AWARequest>(data);
                            if (req.Data.Id != null && req.Data.Id.Length > 0)
                                return req;
                            break;

                        case CommandType.Response:
                            var res = JsonConvert.DeserializeObject<AWAResponse>(data);
                            if (res.Data.Id != null && res.Data.Id.Length > 0)
                                return res;
                            break;

                        case CommandType.Ok:
                            var ok = JsonConvert.DeserializeObject<AWAOk>(data);
                            if (ok.Data.Message != null && ok.Data.Message.Length > 0)
                                return ok;
                            break;

                        case CommandType.GameInit:
                            var gin = JsonConvert.DeserializeObject<AWAGameInit>(data);
                            if (gin.Data.Height > 0 && gin.Data.Width > 0)
                                return gin;
                            break;
                        case CommandType.PlayerInit:
                            var pin = JsonConvert.DeserializeObject<AWAPlayerInit>(data);
                            if (pin.Data.MoveType == GameMoveType.InitiatePlayer
                                && pin.Data.PlayerId > 0 && pin.Data.XPos >= 0
                                && pin.Data.YPos >= 0)
                                return pin;
                            break;
                        case CommandType.GameMove:
                            var move = JsonConvert.DeserializeObject<AWAGameMove>(data);
                            if (move.Data.PlayerId > 0 && move.Data.XPos >= 0
                                && move.Data.YPos >= 0)
                                return move;
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
        public static AWAError CreateError(int code, string version = "1.0")
        {
            return new AWAError(version, code);
        }
        public static AWAMessage CreateMessage(string message, int senderId, string version = "1.0")
        {
            return new AWAMessage(version, message, senderId);
        }
        public static AWARequest CreateRequest(string id, RequestType requestFor, string message, string version = "1.0")
        {
            return new AWARequest(id, requestFor, message, version);
        }
        public static AWAOk CreateOk(string message, string version = "1.0")
        {
            return new AWAOk(version, message);
        }
        public static AWAGameMove CreateGameMove(GameMoveType moveType, int playerId, string name, int xPos, int yPos, MoveDirection direction, string version = "1.0")
        {
            return new AWAGameMove(version, moveType, playerId, name, xPos, yPos, direction);
        }
        public static AWAPlayerInit CreatePlayerInit(string name, int playerId, int xPos, int yPos, MoveDirection direction, string version = "1.0")
        {
            return new AWAPlayerInit(version, GameMoveType.InitiatePlayer, playerId, name, xPos, yPos, direction);
        }
        public static AWAPlayerRemove CreatePlayerRemove(int id, string version = "1.0")
        {
            return new AWAPlayerRemove(version, id);
        }
        public static AWAGameInit CreateGameInit(int height, int width, string version = "1.0")
        {
            return new AWAGameInit(version, height, width);
        }

        public static string Serialize(AWABase obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
