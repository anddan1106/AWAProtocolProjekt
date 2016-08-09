namespace AWAProtocol
{
    public class AWACommand
    {
        public string Type { get; set; }
        public AWACommand(string type)
        {
            Type = type;
        }
    }
}