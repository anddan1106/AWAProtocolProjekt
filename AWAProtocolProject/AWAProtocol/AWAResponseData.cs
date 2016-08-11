namespace AWAProtocol
{
    public class AWAResponseData
    {

        public string Id { get; set; }
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; }

        public AWAResponseData(string id, ResponseType responseType, string message)
        {
            Id = id;
            ResponseType = responseType;
            Message = message;
        }
    }
}