namespace ArtificialLifePlugin
{
    public class Creature
    {
        private int look;
        public int Energy { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        public int Look
        {
            get
            {
                return look;
            }
            set
            {
                look = (value + 8) % 8;
            }
        }

        public Creature(int energy, int posX, int posY)
        {
            Energy = energy;
            PosX = posX;
            PosY = posY;
        }

        public Creature Copy()
        {
            Creature copy = new Creature(Energy, PosX, PosY);
            copy.Look = Look;
            return copy;
        }
    }
}
