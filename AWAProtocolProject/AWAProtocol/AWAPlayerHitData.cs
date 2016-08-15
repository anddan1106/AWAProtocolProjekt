namespace AWAProtocol
{
    public class AWAPlayerHitData : AWAData
    {
        public int Id { get; set; }
        public string Attacker { get; set; }
        public int Damage { get; set; }
        public AWAPlayerHitData(int id, string attacker, int damage)
        {
            Id = id;
            Attacker = attacker;
            Damage = damage;
        }
    }
}