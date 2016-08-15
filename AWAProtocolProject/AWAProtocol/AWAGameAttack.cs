using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAGameAttack : AWABase
    {
        public new AWAGameAttackData Data { get; set; }
        public AWAGameAttack(string version, int id, MoveDirection direction, int xPos, int yPos)
        {
            Version = version;
            Data = new AWAGameAttackData(id, direction, xPos, yPos);
            Command = new AWACommand(CommandType.GameAttack);
        }
    }
}
