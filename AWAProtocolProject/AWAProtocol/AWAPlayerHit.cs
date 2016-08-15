using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAPlayerHit : AWABase
    {
        public new AWAPlayerHitData Data { get; set; }
        public AWAPlayerHit(string version, int victimId, int attackerId, int newHealth)
        {
            Version = version;
            Command = new AWACommand(CommandType.PlayerHit);
            Data = new AWAPlayerHitData(victimId, attackerId, newHealth);
        }
    }
}
