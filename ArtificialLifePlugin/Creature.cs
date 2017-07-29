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

        public int?[] Register { get; set; }

        [StorableConstructor]
        public Creature(bool deserializing)
        {

        }

        public Creature(int energy, int posX, int posY, int look)
        {
            Energy = energy;
            PosX = posX;
            PosY = posY;
            Register = new int?[3];
        }

        public Creature Copy()
        {
            return new Creature(Energy, PosX, PosY, Look);
        }

        public int? ReadRegister(Register register)
        {
            return this.Register[(int)register];
        }

        public void WriteRegister(Register register, int? value)
        {
            this.Register[(int)register] = value;
        }
    }
}
