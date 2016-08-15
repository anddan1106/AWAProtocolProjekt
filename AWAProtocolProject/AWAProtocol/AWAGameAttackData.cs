namespace AWAProtocol
{
    public class AWAGameAttackData
    {
        public int Id { get; set; }
        public MoveDirection Direction { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        public AWAGameAttackData(int id, MoveDirection direction, int xPos, int yPos)
        {
            Id = id;
            Direction = direction;
            XPos = xPos;
            YPos = yPos;
        }
    }
}