using System;
using System.Collections.Generic;

namespace ArtificialLifePlugin
{
    public class World
    {
        private WorldStatus[][] Status { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Food { get; set; }
        public int MovementCount { get; set; }

        public List<Creature> History { get; set; }

        public World(int seed, int width, int height)
        {
            Width = width;
            Height = height;
            History = new List<Creature>();
            InitWorld(seed, width, height);
        }

        public WorldStatus this[int x, int y]
        {
            get
            {
                return Status[y][x];
            }
            set
            {
                Status[y][x] = value;
            }
        }



        public int GetWorldPosX(int x)
        {
            return (x + Width) % Width;
        }

        public int GetWorldPosY(int y)
        {
            return (y + Height) % Height;
        }

        public void AddToHistory(Creature creature)
        {
            History.Add(creature.Copy());
        }

        private void InitWorld(int seed, int width, int height)
        {
            Random rand = new Random(seed);
            Status = new WorldStatus[height][];

            for (int y = 0; y < height; y++)
            {
                Status[y] = new WorldStatus[width];
                for (int x = 0; x < width; x++)
                {
                    if (rand.Next(100) > 90)
                    {
                        this[x,y] = WorldStatus.Food;
                        Food++;
                    }
                }
            }
        }
    }
}
