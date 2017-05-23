using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using System;
using System.Linq;

namespace ArtificialLifePlugin
{
    [StorableClass]
    [Creatable(CreatableAttribute.Categories.GeneticProgrammingProblems, Priority = 160)]
    [Item("Artificial Life Problem", "The artificial life problem.")]
    public class Problem : SymbolicExpressionTreeProblem
    {
        private const string WorldWidthParameterName = "WorldWidth";
        private const string WorldHeightParameterName = "WorldHeight";
        private const string RandomWorldParameterName = "RandomWorld";
        private const string RandomWorldSeedParameterName = "RandomWorldSeed";
        private const string InitialEnergyParameterName = "InitialEnergy";
        private const string InitialPosXParameterName = "InitialPosX";
        private const string InitialPosYParameterName = "InitialPosY";
        private const string WorldParameterName = "World";

        public IFixedValueParameter<IntValue> WorldWidthParameter => (IFixedValueParameter<IntValue>)Parameters[WorldWidthParameterName];
        public IFixedValueParameter<IntValue> WorldHeightParameter => (IFixedValueParameter<IntValue>)Parameters[WorldHeightParameterName];
        public ValueParameter<IntMatrix> WorldParameter => (ValueParameter<IntMatrix>)Parameters[WorldParameterName];
        public IFixedValueParameter<BoolValue> RandomWorldParameter => (IFixedValueParameter<BoolValue>)Parameters[RandomWorldParameterName];
        public IFixedValueParameter<IntValue> RandomWorldSeedParameter => (IFixedValueParameter<IntValue>)Parameters[RandomWorldSeedParameterName];
        public IFixedValueParameter<IntValue> InitialEnergyParameter => (IFixedValueParameter<IntValue>)Parameters[InitialEnergyParameterName];
        public IFixedValueParameter<IntValue> InitialPosXParameter => (IFixedValueParameter<IntValue>)Parameters[InitialPosXParameterName];
        public IFixedValueParameter<IntValue> InitialPosYParameter => (IFixedValueParameter<IntValue>)Parameters[InitialPosYParameterName];
        public override bool Maximization => true;

        #region item cloning and persistence
        [StorableConstructor]
        protected Problem(bool deserializing) : base(deserializing) { }
        [StorableHook(HookType.AfterDeserialization)]
        private void AfterDeserialization() { }

        protected Problem(Problem original, Cloner cloner) : base(original, cloner) { }
        public override IDeepCloneable Clone(Cloner cloner)
        {
            return new Problem(this, cloner);
        }
        #endregion

        public Problem() : base()
        {
            Parameters.Add(new FixedValueParameter<IntValue>(WorldWidthParameterName, "Width of the world.", new IntValue(25)));
            Parameters.Add(new FixedValueParameter<IntValue>(WorldHeightParameterName, "Height of the world.", new IntValue(25)));
            Parameters.Add(new FixedValueParameter<BoolValue>(RandomWorldParameterName, "Random world.", new BoolValue(true)));
            Parameters.Add(new FixedValueParameter<IntValue>(RandomWorldSeedParameterName, "Random world seed", new IntValue(1234)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialEnergyParameterName, "Initial Energy of creature", new IntValue(5)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialPosXParameterName, "Initial PosX of creature", new IntValue(0)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialPosYParameterName, "Initial PosY of creature", new IntValue(0)));
            Parameters.Add(new ValueParameter<IntMatrix>(WorldParameterName, "Food within world", new IntMatrix(25, 25)));

            EventHandler widthHeight = (s, e) =>
            {
                WorldParameter.Value = new IntMatrix(WorldHeightParameter.Value.Value, WorldWidthParameter.Value.Value);
            };
            WorldWidthParameter.ValueChanged += widthHeight;
            WorldHeightParameter.ValueChanged += widthHeight;

            InitializeGrammar(50, 10);
        }

        public override void Analyze(ISymbolicExpressionTree[] trees, double[] qualities, ResultCollection results, IRandom random)
        {
            const string bestSolutionResultName = "Best Solution";
            const string bestSolutionTreeResultName = "Best Solution Tree";
            var bestQuality = Maximization ? qualities.Max() : qualities.Min();
            var bestIdx = Array.IndexOf(qualities, bestQuality);

            World world = ExecuteWorld(trees[bestIdx]);
            if (!results.ContainsKey(bestSolutionResultName))
            {
                results.Add(new Result(bestSolutionResultName, new Solution(trees[bestIdx], WorldHeightParameter.Value.Value, WorldWidthParameter.Value.Value, bestQuality, world)));
                results.Add(new Result(bestSolutionTreeResultName, trees[bestIdx]));
            }
            else if (((Solution)(results[bestSolutionResultName].Value)).Quality < qualities[bestIdx])
            {
                results[bestSolutionResultName].Value = new Solution(trees[bestIdx], WorldHeightParameter.Value.Value, WorldWidthParameter.Value.Value, bestQuality, world);
                results[bestSolutionTreeResultName].Value = trees[bestIdx];
            }
        }

        public override double Evaluate(ISymbolicExpressionTree tree, IRandom random)
        {
            World world = ExecuteWorld(tree);
            return world.History.Count;
        }

        private void InitializeGrammar(int maxLength, int maxDepth)
        {
            var grammar = new SimpleSymbolicExpressionGrammar();
            grammar.AddTerminalSymbols(new[] { Grammar.Move, Grammar.TurnLeft, Grammar.TurnRight });
            grammar.AddSymbol(Grammar.Prog, 2, 2);

            Encoding = new SymbolicExpressionTreeEncoding(grammar, maxLength, maxDepth);
        }

        private World ExecuteWorld(ISymbolicExpressionTree tree)
        {
            World world = CreateWorld();
            Creature creature = CreateCreature();
            tree.Execute(world, creature);
            return world;
        }

        private Creature CreateCreature()
        {
            return new Creature(InitialEnergyParameter.Value.Value, InitialPosXParameter.Value.Value, InitialPosYParameter.Value.Value);
        }

        private World CreateWorld()
        {
            if (RandomWorldParameter.Value.Value)
            {
                return new World(WorldWidthParameter.Value.Value, WorldHeightParameter.Value.Value, RandomWorldSeedParameter.Value.Value);
            }
            return new World(WorldWidthParameter.Value.Value, WorldHeightParameter.Value.Value, WorldParameter.Value);
        }
    }
}
