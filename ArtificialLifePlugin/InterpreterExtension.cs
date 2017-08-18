using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialLifePlugin
{
    internal static partial class InterpreterExtension
    {
        internal static void Execute(this ISymbolicExpressionTree tree, World world, Creature creature)
        {
            const int maxLoops = 800;

            world.AddToHistory(creature);

            int cnt = 0;
            do
            {
                world = ExecuteNode(tree.Root, world, creature);
                cnt++;
            } while (world.Food > 0 && creature.Energy > 0 && cnt < maxLoops);
        }

        private static World ExecuteNode(ISymbolicExpressionTreeNode node, World world, Creature creature)
        {
            if (creature.Energy <= 0)
            {
                return world;
            }

            if (node.Symbol is ProgramRootSymbol || node.Symbol is StartSymbol)
            {
                return ExecuteNode(node.GetSubtree(0), world, creature);
            }
            else if (node.Symbol.Name == Grammar.Prog)
            {
                world = ExecuteNode(node.GetSubtree(0), world, creature);
                return ExecuteNode(node.GetSubtree(1), world, creature);
            }

            if (node.Symbol.Name == Grammar.TurnLeft)
            {
                creature.Look = GetLook(creature.Look - 1);
            }
            else if (node.Symbol.Name == Grammar.TurnRight)
            {
                creature.Look = GetLook(creature.Look + 1);
            }
            else if (node.Symbol.Name == Grammar.Move)
            {
                var pos = GetLookPosition(world, creature.PosX, creature.PosY, creature.Look);
                creature.PosX = pos.Item1;
                creature.PosY = pos.Item2;

                if (world[creature.PosX, creature.PosY] == WorldStatus.Food)
                {
                    world[creature.PosX, creature.PosY] = WorldStatus.Eaten;
                    world.Food--;
                    creature.Energy++;
                }
                else if(world[creature.PosX, creature.PosY] != WorldStatus.Eaten)
                {
                    creature.Energy--;
                }
                world.AddToHistory(creature);
            }
            else if (node.Symbol.Name == Grammar.Sense)
            {
                var symbolNode = node.GetSubtree(0);
                Register register = (Register)Enum.Parse(typeof(Register), symbolNode.Symbol.Name);

                Sensing sense = Sense(world, creature);

                if (world.RepeatSense && creature.ReadRegister(register) == (int)sense)
                {
                    creature.WriteRegister(register, (int)Sensing.Repeat);
                }
                else
                {
                    creature.WriteRegister(register, (int)sense);
                }

            }
            else if (Grammar.IfSymbolNames.Contains(node.Symbol.Name))
            {
                int firstArgument = GetArgumentValue(node.GetSubtree(0), creature);
                int secondArgument = GetArgumentValue(node.GetSubtree(1), creature);

                if ((node.Symbol.Name == Grammar.IfEquals && firstArgument == secondArgument) ||
                    (node.Symbol.Name == Grammar.IfNotEquals && firstArgument != secondArgument) ||
                    (node.Symbol.Name == Grammar.IfGreater && firstArgument > secondArgument) ||
                    (node.Symbol.Name == Grammar.IfLess && firstArgument < secondArgument))
                {
                    return ExecuteNode(node.GetSubtree(2), world, creature);
                }
                else if (node.SubtreeCount == 4)
                {
                    return ExecuteNode(node.GetSubtree(3), world, creature);
                }
            }
            else if (node.Symbol.Name == Grammar.Increase || node.Symbol.Name == Grammar.Decrease)
            {
                Register register = GetRegister(node.GetSubtree(0));
                var value = creature.ReadRegister(register);
                if (node.Symbol.Name == Grammar.Increase)
                {
                    creature.WriteRegister(register, value++);
                }
                else
                {
                    creature.WriteRegister(register, value--);
                }
            }
            else if (node.Symbol.Name == Grammar.ShiftLeft || node.Symbol.Name == Grammar.ShiftRight)
            {
                Register register = GetRegister(node.GetSubtree(0));
                var value = creature.ReadRegister(register);
                if (node.Symbol.Name == Grammar.ShiftLeft)
                {
                    creature.WriteRegister(register, value<<1);
                }
                else
                {
                    creature.WriteRegister(register, value>>1);
                }
            }

            return world;
        }

        private static int GetArgumentValue(ISymbolicExpressionTreeNode node, Creature creature)
        {
            if (Grammar.SensingValues.Contains(node.Symbol.Name))
            {
                return (int)GetSensing(node);
            }
            else if (Grammar.RegisterValues.Contains(node.Symbol.Name))
            {
                Register register = GetRegister(node);
                return creature.ReadRegister(register);
            }

            throw new ArgumentException();
        }

        public static Sensing GetSensing(ISymbolicExpressionTreeNode node)
        {
            return (Sensing)Enum.Parse(typeof(Sensing), node.Symbol.Name);
        }
        public static Register GetRegister(ISymbolicExpressionTreeNode node)
        {
            return (Register)Enum.Parse(typeof(Register), node.Symbol.Name);
        }

        private static Sensing Sense(World world, Creature creature)
        {
            Sensing sense = Sensing.Empty;
            var aheadPos = GetLookPosition(world, creature.PosX, creature.PosY, creature.Look);
            var leftPos = GetLookPosition(world, creature.PosX, creature.PosY, GetLook(creature.Look - 1));
            var rightPos = GetLookPosition(world, creature.PosX, creature.PosY, GetLook(creature.Look + 1));

            if (world[aheadPos.Item1, aheadPos.Item2] == WorldStatus.Food)
            {
                sense = Sensing.Food;
            }
            else if (world[leftPos.Item1, leftPos.Item2] == WorldStatus.Food)
            {
                sense = Sensing.FoodLeft;
            }
            else if (world[rightPos.Item1, rightPos.Item2] == WorldStatus.Food)
            {
                sense = Sensing.FoodRight;
            }

            return sense;
        }

        private static int GetLook(int look)
        {
            return (look + 8) % 8;
        }

        private static Tuple<int, int> GetLookPosition(World world, int x, int y, int look)
        {
            if (look < 3)
            {
                y = world.GetWorldPosY(y - 1);
                int moveX = look - 1;
                x = world.GetWorldPosX(x + moveX);
            }
            else if (look == 3)
            {
                x = world.GetWorldPosX(x + 1);
            }
            else if (look == 7)
            {
                x = world.GetWorldPosX(x - 1);
            }
            else
            {
                y = world.GetWorldPosY(y + 1);
                int moveX = -(look - 5);
                x = world.GetWorldPosX(x + moveX);
            }

            return new Tuple<int, int>(x, y);
        }
    }
}
