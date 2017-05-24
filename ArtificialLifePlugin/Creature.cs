using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace ArtificialLifePlugin
{
    [StorableClass]
    public class Creature
    {
        [Storable]
        public int Energy { get; set; }
        [Storable]
        public int PosX { get; set; }
        [Storable]
        public int PosY { get; set; }

        [Storable]
        public int Look { get; set; }

        [Storable]
        public int? Sense { get; set; }

        [StorableConstructor]
        public Creature(bool deserializing)
        {

        }

        public Creature(int energy, int posX, int posY, int look)
        {
            Energy = energy;
            PosX = posX;
            PosY = posY;
        }

        public Creature Copy()
        {
            return new Creature(Energy, PosX, PosY, Look);
        }
    }
}
