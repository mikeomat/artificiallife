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

        public static string IfSenseGreater = "IfSenseGreater";

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
            var ifEqual = new SimpleSymbol(IfSenseEquals, "", 2, 3);
            grammar.AddSymbol(ifEqual);
            var ifGreater = new SimpleSymbol(IfSenseGreater, "", 2, 3);
            grammar.AddSymbol(ifGreater);

            foreach (var s in grammar.Symbols)
            {
                if (s is ProgramRootSymbol || s.Name == "Defun") continue;

                if (s is StartSymbol || s.Name == Prog)
                {
                    grammar.AddAllowedChildSymbol(s, ifEqual);
                    grammar.AddAllowedChildSymbol(s, ifGreater);
                    if (s is StartSymbol) continue;
                }

                if (SensingValues.Contains(s.Name))
                {
                    grammar.AddAllowedChildSymbol(ifEqual, s, 0);
                    grammar.RemoveAllowedChildSymbol(ifEqual, s, 1);
                    grammar.RemoveAllowedChildSymbol(ifEqual, s, 2);
                    grammar.AddAllowedChildSymbol(ifGreater, s, 0);
                    grammar.RemoveAllowedChildSymbol(ifGreater, s, 1);
                    grammar.RemoveAllowedChildSymbol(ifGreater, s, 2);
                }
                else
                {
                    grammar.AddAllowedChildSymbol(ifEqual, s, 1);
                    grammar.AddAllowedChildSymbol(ifEqual, s, 2);
                    grammar.RemoveAllowedChildSymbol(ifEqual, s, 0);
                    grammar.AddAllowedChildSymbol(ifGreater, s, 1);
                    grammar.AddAllowedChildSymbol(ifGreater, s, 2);
                    grammar.RemoveAllowedChildSymbol(ifGreater, s, 0);
                }
            }
        }
    }
}
