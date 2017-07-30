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

        public static string IfEquals = "IfEquals";
        public static string IfNotEquals = "IfNotEquals";
        public static string IfGreater = "IfGreater";
        public static string IfLess = "IfLess";
        public static string[] IfSymbolNames = { IfEquals, IfNotEquals, IfGreater, IfLess };

        public static string Increase = "Increase";
        public static string Decrease = "Decrease";
        public static string ShiftLeft = "ShiftLeft";
        public static string ShiftRight = "ShiftRight";

        public static string Sense = "Sense";

        public static string Prog = "Prog";

        public static string[] SensingValues = Enum.GetNames(typeof(Sensing));
        public static string[] RegisterValues = Enum.GetNames(typeof(Register));


        public static ISymbolicExpressionGrammar Create()
        {
            var grammar = new SimpleSymbolicExpressionGrammar();

            grammar.AddTerminalSymbols(new[] { Move, TurnLeft, TurnRight });
            grammar.AddSymbol(Prog, 2, 2);
            AddSensingValues(grammar);
            AddRegisterValues(grammar);
            AddWithRegisterArgument(Increase, grammar);
            AddWithRegisterArgument(Decrease, grammar);
            AddWithRegisterArgument(ShiftLeft, grammar);
            AddWithRegisterArgument(ShiftRight, grammar);
            AddWithRegisterArgument(Sense, grammar);
            AddIf(grammar);
            return grammar;
        }

        private static void AddRegisterValues(ISymbolicExpressionGrammar grammar)
        {
            foreach (var register in RegisterValues)
            {
                ISymbol symbol = new SimpleSymbol(register, 0);
                grammar.AddSymbol(symbol);
            }
        }

        private static void AddSensingValues(ISymbolicExpressionGrammar grammar)
        {
            foreach (var sensing in SensingValues)
            {
                ISymbol symbol = new SimpleSymbol(sensing, 0);
                grammar.AddSymbol(symbol);
            }
        }

        private static void AddWithRegisterArgument(string symbolName, ISymbolicExpressionGrammar grammar)
        {
            ISymbol symbol = new SimpleSymbol(symbolName, "", 1, 1);
            grammar.AddSymbol(symbol);
            foreach (var currentSymbol in grammar.Symbols)
            {
                if (currentSymbol is ProgramRootSymbol || currentSymbol.Name == "Defun") continue;

                if (currentSymbol is StartSymbol || currentSymbol.Name == Prog)
                {
                    grammar.AddAllowedChildSymbol(currentSymbol, symbol);
                    if (currentSymbol is StartSymbol) continue;
                }

                if (RegisterValues.Contains(currentSymbol.Name))
                {
                    grammar.AddAllowedChildSymbol(symbol, currentSymbol, 0);
                }
            }
        }


        private static void AddIf(ISymbolicExpressionGrammar grammar)
        {
            List<ISymbol> ifSymbols = new List<ISymbol>();
            foreach (var ifSymbolName in IfSymbolNames)
            {
                var ifSymbol = new SimpleSymbol(ifSymbolName, "", 3, 4);
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

                    else if (SensingValues.Contains(currentSymbol.Name) || RegisterValues.Contains(currentSymbol.Name))
                    {
                        grammar.AddAllowedChildSymbol(ifSymbol, currentSymbol, 0);
                        grammar.AddAllowedChildSymbol(ifSymbol, currentSymbol, 1);
                    }
                    else
                    {
                        grammar.AddAllowedChildSymbol(ifSymbol, currentSymbol, 2);
                        grammar.AddAllowedChildSymbol(ifSymbol, currentSymbol, 3);
                    }
                }
            }
        }
    }
}
