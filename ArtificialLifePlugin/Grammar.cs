using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialLifePlugin
{
    public static class Grammar
    {
        public static string Move = "Move";
        public static string TurnLeft = "TurnLeft";
        public static string TurnRight = "TurnRight";

        public static string IfSenseEquals = "IfSenseEquals";
        public static string IfSenseNotEquals = "IfSenseNotEquals";
        public static string IfSenseGreater = "IfSenseGreater";
        public static string IfSenseLess = "IfSenseLess";

        public static string Prog = "Prog";

        public static string[] SensingValues = Enum.GetNames(typeof(Sensing));


        public static ISymbolicExpressionGrammar Create()
        {
            var grammar = new SimpleSymbolicExpressionGrammar();

            grammar.AddTerminalSymbols(new[] { Move, TurnLeft, TurnRight });
            grammar.AddSymbol(Prog, 2, 2);
            AddSensingValues(grammar);
            AddIf(grammar);

            return grammar;
        }
        private static void AddSensingValues(ISymbolicExpressionGrammar grammar)
        {
            foreach (var sensing in SensingValues)
            {
                ISymbol symbol = new SimpleSymbol(sensing, 0);
                grammar.AddSymbol(symbol);
            }
        }


        private static void AddIf(ISymbolicExpressionGrammar grammar)
        {
            string[] ifSymbolNames = new[] { IfSenseEquals, IfSenseGreater, IfSenseNotEquals, IfSenseLess };
            List<ISymbol> ifSymbols = new List<ISymbol>();
            foreach (var ifSymbolName in ifSymbolNames)
            {
                var ifSymbol = new SimpleSymbol(ifSymbolName, "", 2, 3);
                grammar.AddSymbol(ifSymbol);
                ifSymbols.Add(ifSymbol);
            }

            foreach (var ifSymbol in ifSymbols)
            {
                foreach (var currentSymbol in grammar.Symbols)
                {
                    if (currentSymbol is ProgramRootSymbol || currentSymbol.Name == "Defun") continue;

                    if (currentSymbol is StartSymbol || currentSymbol.Name == Prog)
                    {
                        grammar.AddAllowedChildSymbol(currentSymbol, ifSymbol);
                        if (currentSymbol is StartSymbol) continue;
                    }

                    if (SensingValues.Contains(currentSymbol.Name))
                    {
                        grammar.AddAllowedChildSymbol(ifSymbol, currentSymbol, 0);
                        grammar.RemoveAllowedChildSymbol(ifSymbol, currentSymbol, 1);
                        grammar.RemoveAllowedChildSymbol(ifSymbol, currentSymbol, 2);
                    }
                    else
                    {
                        grammar.AddAllowedChildSymbol(ifSymbol, currentSymbol, 1);
                        grammar.AddAllowedChildSymbol(ifSymbol, currentSymbol, 2);
                        grammar.RemoveAllowedChildSymbol(ifSymbol, currentSymbol, 0);
                    }
                }
            }
        }
    }
}
