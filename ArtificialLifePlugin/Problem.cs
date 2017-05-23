using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialLifePlugin
{
    [StorableClass]
    [Creatable(CreatableAttribute.Categories.GeneticProgrammingProblems, Priority = 160)]
    [Item("Artificial Life Problem", "The artificial life problem.")]
    public class Problem : SymbolicExpressionTreeProblem
    {
        private const string WorldWidthParameterName = "WorldWidth";
        private const string WorldHeightParameterName = "WorldHeight";

        public IFixedValueParameter<IntValue> WorldWidthParameter => (IFixedValueParameter<IntValue>)Parameters[WorldWidthParameterName];
        public IFixedValueParameter<IntValue> WorldHeightParameter => (IFixedValueParameter<IntValue>)Parameters[WorldHeightParameterName];
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

        public Problem()
          : base()
        {
            Parameters.Add(new FixedValueParameter<IntValue>(WorldWidthParameterName, "Width of the world.", new IntValue(10)));
            Parameters.Add(new FixedValueParameter<IntValue>(WorldHeightParameterName, "Height of the world.", new IntValue(10)));
            InitializeGrammar(1000, 17);
        }

        public override void Analyze(ISymbolicExpressionTree[] trees, double[] qualities, ResultCollection results, IRandom random)
        {
            const string bestSolutionResultName = "Best Solution";
            var bestQuality = Maximization ? qualities.Max() : qualities.Min();
            var bestIdx = Array.IndexOf(qualities, bestQuality);

            if (!results.ContainsKey(bestSolutionResultName))
            {
                results.Add(new Result(bestSolutionResultName, new Solution(trees[bestIdx], WorldHeightParameter.Value.Value, WorldWidthParameter.Value.Value, bestQuality)));
            }
            else if (((Solution)(results[bestSolutionResultName].Value)).Quality < qualities[bestIdx])
            {
                results[bestSolutionResultName].Value = new Solution(trees[bestIdx], WorldHeightParameter.Value.Value, WorldWidthParameter.Value.Value, bestQuality);
            }
        }

        public override double Evaluate(ISymbolicExpressionTree tree, IRandom random)
        {
            return tree.Evaluate();
            //var length = WorldHeightParameter.Value.Value;
            //var width = WorldWidthParameter.Value.Value;

            //var lawn = Interpreter.EvaluateLawnMowerProgram(length, width, tree);
            //// count number of squares that have been mowed
            //int numberOfMowedCells = 0;
            //for (int i = 0; i < length; i++)
            //    for (int j = 0; j < width; j++)
            //        if (lawn[i, j])
            //        {
            //            numberOfMowedCells++;
            //        }
            //return numberOfMowedCells;
        }

        private void InitializeGrammar(int maxLength, int maxDepth)
        {
            var grammar = new SimpleSymbolicExpressionGrammar();
            grammar.AddTerminalSymbols(new[] { Grammar.Foward, Grammar.Backward, Grammar.Left, Grammar.Right });

            Encoding = new SymbolicExpressionTreeEncoding(grammar, maxLength, maxDepth);
        }
    }
}
