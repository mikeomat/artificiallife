using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace ArtificialLifePlugin
{
    [StorableClass]
    public class Creature
    {
        private int look;

        [Storable]
        public int Energy { get; set; }
        [Storable]
        public int PosX { get; set; }
        [Storable]
        public int PosY { get; set; }

        [Storable]
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

        [StorableConstructor]
        public Creature(bool deserializing)
        {

        }

        public Creature(int energy, int posX, int posY)
        {
            Energy = energy;
            PosX = posX;
            PosY = posY;
            Look = 3;
        }

        public Creature Copy()
        {
            Creature copy = new Creature(Energy, PosX, PosY);
            copy.Look = Look;
            return copy;
        }
    }
}
