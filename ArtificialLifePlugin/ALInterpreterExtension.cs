﻿using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialLifePlugin
{
    internal static partial class ALInterpreterExtension
    {
        internal static void Execute(this ISymbolicExpressionTree tree, World world, Creature creature)
        {
            int maxLoops = 1000;

            world.AddToHistory(creature);

            int cnt = 0;
            do
            {
                world = Execute(tree.Root, world, creature);
                cnt++;
            } while (world.Food > 0 && creature.Energy > 0 && cnt < maxLoops);
        }

        private static World Execute(ISymbolicExpressionTreeNode node, World world, Creature creature)
        {
            if (creature.Energy <= 0)
            {
                return world;
            }

            if (node.Symbol is ProgramRootSymbol || node.Symbol is StartSymbol)
            {
                return Execute(node.GetSubtree(0), world, creature);
            }
            else if (node.Symbol.Name == Grammar.Prog)
            {
                world = Execute(node.GetSubtree(0), world, creature);
                return Execute(node.GetSubtree(1), world, creature);
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
                }
                else
                {
                    creature.Energy--;
                }
                creature.Sense = null;
                world.AddToHistory(creature);
            }
            else if (node.Symbol.Name == Grammar.IfSenseEquals || node.Symbol.Name == Grammar.IfSenseGreater)
            {
                var symbolNode = node.GetSubtree(0);
                Sensing sense = (Sensing)Enum.Parse(typeof(Sensing), symbolNode.Symbol.Name);

                Sensing sensed = Sense(world, creature);
                if ((node.Symbol.Name == Grammar.IfSenseEquals && sensed == sense) ||
                    (node.Symbol.Name == Grammar.IfSenseNotEquals && sensed != sense) ||
                    (node.Symbol.Name == Grammar.IfSenseGreater && sensed > sense) ||
                    (node.Symbol.Name == Grammar.IfSenseLess && sensed < sense))
                {
                    return Execute(node.GetSubtree(1), world, creature);
                }
                else if (node.SubtreeCount == 3)
                {
                    return Execute(node.GetSubtree(2), world, creature);
                }
            }

            return world;
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
            else
            {
                int look = GetLook(creature.Look + 2);
                int leftLook = GetLook(creature.Look - 1);
                while (look != leftLook)
                {
                    var pos = GetLookPosition(world, creature.PosX, creature.PosY, look);
                    if (world[pos.Item1, pos.Item2] == WorldStatus.Food)
                    {
                        sense = Sensing.FoodSomewhere;
                        break;
                    }
                    look = GetLook(look + 1);
                }
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
