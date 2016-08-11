using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAOkData : AWAData
    {
        public string Message { get; set; }
        public AWAOkData(string message)
        {
            Message = message;
        }
    }
}
