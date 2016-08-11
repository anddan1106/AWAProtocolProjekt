using static AWAProtocol.AWABase;

namespace AWAProtocol
{
    public class AWACommand
    {
        public CommandType Type { get; set; }
        public AWACommand(CommandType type)
        {
            Type = type;
        }
    }
}