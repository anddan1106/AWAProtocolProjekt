using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAErrorData : AWAData
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public AWAErrorData(int code)
        {
            Code = code;
            switch (code)
            {
                case 1:
                    Message = "Incoming data was not a valid protocol object";
                    break;
                default:
                    Code = -1;
                    Message = "Message Code was not valid";
                    break;
            }
        }
    }
}
