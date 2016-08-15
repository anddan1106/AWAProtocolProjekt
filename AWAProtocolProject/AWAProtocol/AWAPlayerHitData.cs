namespace AWAProtocol
{
    public class AWAPlayerHitData : AWAData
    {
        public int VictimId { get; set; }
        public int AttackerId { get; set; }
        public int NewHealth { get; set; }
        public AWAPlayerHitData(int victimId, int attackerId, int newHealth)
        {
            VictimId = victimId;
            AttackerId = attackerId;
            NewHealth = newHealth;
        }
    }
}