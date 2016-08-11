
using static AWAProtocol.AWABase;

namespace AWAProtocol
{
    public class AWARequestData
    {

        public RequestType RequestFor { get; set; }
        public string Id { get; set; }
        public string Message { get; set; }

        public AWARequestData(string id, RequestType requestFor, string message)
        {
            Id = id;
            Message = message;
            RequestFor = requestFor;
        }

    }
}
