namespace AWAProtocol
{
    public class AWAGameAttackData
    {
        public int Id { get; set; }
        public MoveDirection Direction { get; set; }
        public int Damage { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        public AWAGameAttackData(int id, MoveDirection direction, int damage, int xPos, int yPos)
        {
            Id = id;
            Direction = direction;
            Damage = damage;
            XPos = xPos;
            YPos = yPos;
        }
    }
}