using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace ArtificialLifePlugin
{
    [StorableClass]
    public sealed class Solution : NamedItem
    {
        [Storable]
        public int Width { get; private set; }
        [Storable]
        public int Height { get; private set; }
        [Storable]
        public ISymbolicExpressionTree Tree { get; private set; }
        [Storable]
        public World World { get; private set; }
        [Storable]
        public double Quality { get; private set; }

        #region item cloning and persistence
        [StorableConstructor]
        private Solution(bool deserializing) : base(deserializing) { }
        [StorableHook(HookType.AfterDeserialization)]
        private void AfterDeserialization() { }

        private Solution(Solution original, Cloner cloner)
          : base(original, cloner)
        {
            this.Height = original.Height;
            this.Width = original.Width;
            this.Tree = cloner.Clone(original.Tree);
            this.World = original.World;
            this.Quality = original.Quality;
        }
        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new Solution(this, cloner);
        }
        #endregion

        public Solution(ISymbolicExpressionTree tree, int length, int width, double quality, World world)
          : base("Clever Creatures Solution", "A Clever Creatures solution.")
        {
            this.Tree = tree;
            this.Height = length;
            this.Width = width;
            this.Quality = quality;
            this.World = world;
        }
    }
}
