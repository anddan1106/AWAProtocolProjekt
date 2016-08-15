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
        public AWAPlayerHit(string version, int id, string attacker, int damage)
        {
            Version = version;
            Command = new AWACommand(CommandType.PlayerHit);
            Data = new AWAPlayerHitData(id, attacker, damage);
        }
    }
}
