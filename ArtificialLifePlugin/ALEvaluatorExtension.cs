using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialLifePlugin
{
    internal static partial class ALEvaluatorExtension
    {
        internal static void Execute(this ISymbolicExpressionTree tree, World world, Creature creature)
        {
            int maxLoops = 50;

            world.AddToHistory(creature);

            int cnt = 0;
            do
            {
                world = Execute(tree.Root, world, creature, tree.Root.Subtrees.Skip(1).ToArray());
                cnt++;
            } while (world.Food > 0 && creature.Energy > 0 && cnt < maxLoops);
        }

        private static World Execute(ISymbolicExpressionTreeNode node, World world, Creature creature, IEnumerable<ISymbolicExpressionTreeNode> adfs)
        {
            if (creature.Energy <= 0)
            {
                return world;
            }

            if (node.Symbol is ProgramRootSymbol || node.Symbol is StartSymbol)
            {
                return Execute(node.GetSubtree(0), world, creature, adfs);
            }
            else if (node.Symbol.Name == Grammar.Prog)
            {
                world = Execute(node.GetSubtree(0), world, creature, adfs);
                return Execute(node.GetSubtree(1), world, creature, adfs);
            }

            if (node.Symbol.Name == Grammar.TurnLeft)
            {
                creature.Look--;
            }
            else if (node.Symbol.Name == Grammar.TurnRight)
            {
                creature.Look++;
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
                else
                {
                    creature.Energy--;
                }
                world.MovementCount++;
            }

            world.AddToHistory(creature);

            return world;
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
                x= world.GetWorldPosX(x + 1);
            }
            else if (look == 7)
            {
                x= world.GetWorldPosX(x - 1);
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
