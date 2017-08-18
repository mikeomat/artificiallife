using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtificialLifePlugin
{
    [StorableClass]
    [Creatable(CreatableAttribute.Categories.GeneticProgrammingProblems, Priority = 160)]
    [Item("Clever Creatures Problem", "The Clever Creatures problem.")]
    public class Problem : SymbolicExpressionTreeProblem
    {
        private const string WorldWidthParameterName = "WorldWidth";
        private const string WorldHeightParameterName = "WorldHeight";
        private const string RandomWorldParameterName = "RandomWorld";
        private const string RandomWorldSeedParameterName = "RandomWorldSeed";
        private const string InitialEnergyParameterName = "InitialEnergy";
        private const string InitialPosXParameterName = "InitialPosX";
        private const string InitialPosYParameterName = "InitialPosY";
        private const string InitialLookParameterName = "InitialLook";
        private const string WorldCountParameterName = "WorldCount";
        private const string WorldParameterName = "World";
        private const string World2ParameterName = "World2";
        private const string World3ParameterName = "World3";
        private const string World4ParameterName = "World4";
        private const string InitialPosX2ParameterName = "InitialPosX2";
        private const string InitialPosY2ParameterName = "InitialPosY2";
        private const string InitialLook2ParameterName = "InitialLook2";
        private const string InitialPosX3ParameterName = "InitialPosX3";
        private const string InitialPosY3ParameterName = "InitialPosY3";
        private const string InitialLook3ParameterName = "InitialLook3";
        private const string InitialPosX4ParameterName = "InitialPosX4";
        private const string InitialPosY4ParameterName = "InitialPosY4";
        private const string InitialLook4ParameterName = "InitialLook4";

        public IFixedValueParameter<IntValue> WorldWidthParameter => (IFixedValueParameter<IntValue>)Parameters[WorldWidthParameterName];
        public IFixedValueParameter<IntValue> WorldHeightParameter => (IFixedValueParameter<IntValue>)Parameters[WorldHeightParameterName];

        public ValueParameter<IntValue> WorldCountParameter => (ValueParameter<IntValue>)Parameters[WorldCountParameterName];
        public ValueParameter<IntMatrix> WorldParameter => (ValueParameter<IntMatrix>)Parameters[WorldParameterName];
        public ValueParameter<IntMatrix> World2Parameter => (ValueParameter<IntMatrix>)Parameters[World2ParameterName];
        public ValueParameter<IntMatrix> World3Parameter => (ValueParameter<IntMatrix>)Parameters[World3ParameterName];
        public ValueParameter<IntMatrix> World4Parameter => (ValueParameter<IntMatrix>)Parameters[World4ParameterName];
        public IFixedValueParameter<BoolValue> RandomWorldParameter => (IFixedValueParameter<BoolValue>)Parameters[RandomWorldParameterName];
        public IFixedValueParameter<IntValue> RandomWorldSeedParameter => (IFixedValueParameter<IntValue>)Parameters[RandomWorldSeedParameterName];
        public IFixedValueParameter<IntValue> InitialEnergyParameter => (IFixedValueParameter<IntValue>)Parameters[InitialEnergyParameterName];
        public IFixedValueParameter<IntValue> InitialPosXParameter => (IFixedValueParameter<IntValue>)Parameters[InitialPosXParameterName];
        public IFixedValueParameter<IntValue> InitialPosYParameter => (IFixedValueParameter<IntValue>)Parameters[InitialPosYParameterName];
        public IFixedValueParameter<IntValue> InitialLookParameter => (IFixedValueParameter<IntValue>)Parameters[InitialLookParameterName];
        public IFixedValueParameter<IntValue> InitialPosX2Parameter => (IFixedValueParameter<IntValue>)Parameters[InitialPosX2ParameterName];
        public IFixedValueParameter<IntValue> InitialPosY2Parameter => (IFixedValueParameter<IntValue>)Parameters[InitialPosY2ParameterName];
        public IFixedValueParameter<IntValue> InitialLook2Parameter => (IFixedValueParameter<IntValue>)Parameters[InitialLook2ParameterName];
        public IFixedValueParameter<IntValue> InitialPosX3Parameter => (IFixedValueParameter<IntValue>)Parameters[InitialPosX3ParameterName];
        public IFixedValueParameter<IntValue> InitialPosY3Parameter => (IFixedValueParameter<IntValue>)Parameters[InitialPosY3ParameterName];
        public IFixedValueParameter<IntValue> InitialLook3Parameter => (IFixedValueParameter<IntValue>)Parameters[InitialLook3ParameterName];
        public IFixedValueParameter<IntValue> InitialPosX4Parameter => (IFixedValueParameter<IntValue>)Parameters[InitialPosX4ParameterName];
        public IFixedValueParameter<IntValue> InitialPosY4Parameter => (IFixedValueParameter<IntValue>)Parameters[InitialPosY4ParameterName];
        public IFixedValueParameter<IntValue> InitialLook4Parameter => (IFixedValueParameter<IntValue>)Parameters[InitialLook4ParameterName];
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
            BestKnownQuality = 100.0;
            Parameters.Add(new FixedValueParameter<IntValue>(WorldWidthParameterName, "Width of the world.", new IntValue(25)));
            Parameters.Add(new FixedValueParameter<IntValue>(WorldHeightParameterName, "Height of the world.", new IntValue(25)));
            Parameters.Add(new FixedValueParameter<BoolValue>(RandomWorldParameterName, "Random world.", new BoolValue(true)));
            Parameters.Add(new FixedValueParameter<IntValue>(RandomWorldSeedParameterName, "Random world seed", new IntValue(1234)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialEnergyParameterName, "Initial Energy of creature", new IntValue(5)));

            Parameters.Add(new FixedValueParameter<IntValue>(InitialPosXParameterName, "Initial PosX of creature", new IntValue(0)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialPosYParameterName, "Initial PosY of creature", new IntValue(0)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialLookParameterName, "Initial Look of creature values from 0 to 7 starting with left top clockwise", new IntValue(3)));

            Parameters.Add(new FixedValueParameter<IntValue>(InitialPosX2ParameterName, "Initial PosX of creature", new IntValue(0)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialPosY2ParameterName, "Initial PosY of creature", new IntValue(0)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialLook2ParameterName, "Initial Look of creature values from 0 to 7 starting with left top clockwise", new IntValue(3)));


            Parameters.Add(new FixedValueParameter<IntValue>(InitialPosX3ParameterName, "Initial PosX of creature", new IntValue(0)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialPosY3ParameterName, "Initial PosY of creature", new IntValue(0)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialLook3ParameterName, "Initial Look of creature values from 0 to 7 starting with left top clockwise", new IntValue(3)));


            Parameters.Add(new FixedValueParameter<IntValue>(InitialPosX4ParameterName, "Initial PosX of creature", new IntValue(0)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialPosY4ParameterName, "Initial PosY of creature", new IntValue(0)));
            Parameters.Add(new FixedValueParameter<IntValue>(InitialLook4ParameterName, "Initial Look of creature values from 0 to 7 starting with left top clockwise", new IntValue(3)));

            Parameters.Add(new FixedValueParameter<IntValue>(WorldCountParameterName, "Count of different Worlds", new IntValue(1)));
            Parameters.Add(new ValueParameter<IntMatrix>(WorldParameterName, "Food within world 1", new IntMatrix(25, 25)));
            Parameters.Add(new ValueParameter<IntMatrix>(World2ParameterName, "Food within world 2", new IntMatrix(25, 25)));
            Parameters.Add(new ValueParameter<IntMatrix>(World3ParameterName, "Food within world 3", new IntMatrix(25, 25)));
            Parameters.Add(new ValueParameter<IntMatrix>(World4ParameterName, "Food within world 4", new IntMatrix(25, 25)));

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
            const int max = 1000;
            const string bestSolutionsName = "Best Solutions";
            const string bestSolutionsTreeName = "Best Solutions Trees";
            const string bestSolutionsCountName = "Best Solutions Count";
            var bestQuality = qualities.Max();

            if (results.ContainsKey(bestSolutionsName) && bestQuality < ((ItemArray<Solution>)results[bestSolutionsName].Value).First().Quality || bestQuality < 95)
            {
                return;
            }

            var bestSolutions = trees.Select((t, idx) => new { tree = t, index = idx, quality = qualities[idx] }).Where((t) => qualities[t.index] == bestQuality)
                .OrderBy(t => t.tree.Length).Select(t => CreateSolution(t.tree, t.quality, ExecuteWorld(t.tree))).OrderByDescending(t => t.Quality).ToList();
            if (!results.ContainsKey(bestSolutionsName))
            {
                bestSolutions = bestSolutions.Take(Math.Min(bestSolutions.Count(), max)).ToList();
                results.Add(new Result(bestSolutionsCountName, new IntValue(bestSolutions.Count())));
                results.Add(new Result(bestSolutionsName, new ItemArray<Solution>(bestSolutions)));
                results.Add(new Result(bestSolutionsTreeName, new ItemArray<ISymbolicExpressionTree>(bestSolutions.Select(t => t.Tree))));
            }
            else
            {
                var solutions = ((ItemArray<Solution>)results[bestSolutionsName].Value).ToList();
                if (solutions.First().Quality < bestQuality)
                {
                    solutions.Clear();
                }
                bestSolutions.AddRange((ItemArray<Solution>)results[bestSolutionsName].Value);
                bestSolutions = bestSolutions
                    .Distinct(new LambdaComparer<Solution>((s1, s2) => s1.Tree.Length == s2.Tree.Length && s1.World.WorldNr == s2.World.WorldNr && s1.Tree.Depth == s2.Tree.Depth))
                    .OrderByDescending(t => t.Quality).ThenBy(t => t.Tree.Length)
                    .Take(Math.Min(bestSolutions.Count(), max)).ToList();
                results[bestSolutionsName].Value = new ItemArray<Solution>(bestSolutions);
                results[bestSolutionsTreeName].Value = new ItemArray<ISymbolicExpressionTree>(bestSolutions.Select(t => t.Tree));
                results[bestSolutionsCountName].Value = new IntValue(bestSolutions.Count());
            }
        }

        private Solution CreateSolution(ISymbolicExpressionTree tree, double quality, World world)
        {
            return new Solution(tree, WorldHeightParameter.Value.Value, WorldWidthParameter.Value.Value, quality, world);
        }
        private Solution CreateSolution(Solution solution)
        {
            return new Solution(solution.Tree, solution.Height, solution.Width, solution.Quality, solution.World);
        }

        public override double Evaluate(ISymbolicExpressionTree tree, IRandom random)
        {
            World world = ExecuteWorld(tree);
            int initialEnergy = InitialEnergyParameter.Value.Value;
            Creature creature = world.History.Last();
            double foodRatio = 1.0 - (double)world.Food / world.InitialFood;
            double energyRatio = (double)creature.Energy / (world.InitialFood + initialEnergy);
            return (energyRatio + foodRatio) * 100 / 2.0;
        }

        private void InitializeGrammar(int maxLength, int maxDepth)
        {
            var grammar = Grammar.Create();
            Encoding = new SymbolicExpressionTreeEncoding(grammar, maxLength, maxDepth);
        }

        private World ExecuteWorld(ISymbolicExpressionTree tree)
        {
            int seed = tree.GetHashCode();
            World world = CreateWorld(seed);
            Creature creature = CreateCreature(seed);
            tree.Execute(world, creature);
            return world;
        }

        private Creature CreateCreature(int seed)
        {
            Random rand = new Random(seed);
            int posIdx = rand.Next(0, WorldCountParameter.Value.Value);

            int posX = 0, posY = 0, look = 0;
            if (WorldCountParameter.Value.Value <= 1 || posIdx == 0)
            {
                posX = InitialPosXParameter.Value.Value;
                posY = InitialPosYParameter.Value.Value;
                look = InitialLookParameter.Value.Value;
            }
            else if (posIdx == 1)
            {
                posX = InitialPosX2Parameter.Value.Value;
                posY = InitialPosY2Parameter.Value.Value;
                look = InitialLook2Parameter.Value.Value;
            }
            else if (posIdx == 2)
            {
                posX = InitialPosX3Parameter.Value.Value;
                posY = InitialPosY3Parameter.Value.Value;
                look = InitialLook3Parameter.Value.Value;
            }
            else if (posIdx == 3)
            {
                posX = InitialPosX4Parameter.Value.Value;
                posY = InitialPosY4Parameter.Value.Value;
                look = InitialLook4Parameter.Value.Value;
            }

            return new Creature(InitialEnergyParameter.Value.Value, posX, posY, look);
        }

        private World CreateWorld(int seed)
        {
            if (RandomWorldParameter.Value.Value)
            {
                return new World(WorldWidthParameter.Value.Value, WorldHeightParameter.Value.Value, RandomWorldSeedParameter.Value.Value);
            }

            IntMatrix world = null;
            if (WorldCountParameter.Value.Value <= 1)
            {
                world = WorldParameter.Value;
            }
            else
            {
                world = GetWorldMatrix(seed);
            }
            ISymbolicExpressionGrammar grammar = Encoding.Grammar;
            return new World(WorldWidthParameter.Value.Value, WorldHeightParameter.Value.Value, world)
            {
                RepeatSense = grammar.Symbols.First(s => s.Name == Sensing.Repeat.ToString()).Enabled,
                WorldNr = GetWorldIndex(seed)
            };
        }

        private int GetWorldIndex(int seed)
        {
            Random rand = new Random(seed);
            return rand.Next(0, WorldCountParameter.Value.Value);
        }

        private IntMatrix GetWorldMatrix(int seed)
        {
            int worldIdx = GetWorldIndex(seed);
            return worldIdx == 0 ? WorldParameter.Value :
                    worldIdx == 1 ? World2Parameter.Value :
                    worldIdx == 2 ? World3Parameter.Value :
                    World4Parameter.Value;
        }
    }
    public class LambdaComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _expression;

        public LambdaComparer(Func<T, T, bool> lambda)
        {
            _expression = lambda;
        }

        public bool Equals(T x, T y)
        {
            return _expression(x, y);
        }

        public int GetHashCode(T obj)
        {
            /*
             If you just return 0 for the hash the Equals comparer will kick in. 
             The underlying evaluation checks the hash and then short circuits the evaluation if it is false.
             Otherwise, it checks the Equals. If you force the hash to be true (by assuming 0 for both objects), 
             you will always fall through to the Equals check which is what we are always going for.
            */
            return 0;
        }
    }
}

