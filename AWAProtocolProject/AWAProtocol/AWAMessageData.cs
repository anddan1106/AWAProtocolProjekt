using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAMessageData : AWAData
    {
        public string Message { get; set; }
        public int SenderId { get; set; }
        public AWAMessageData(string message, int senderId)
        {
            SenderId = senderId;
            Message = message;
        }
    }
}
