using HeuristicLab.Data;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using System;
using System.Collections.Generic;

namespace ArtificialLifePlugin
{
    [StorableClass]
    public class World
    {
        [Storable]
        private WorldStatus[][] Status { get; set; }
        [Storable]
        public int Width { get; private set; }
        [Storable]
        public int Height { get; private set; }
        [Storable]
        public int Food { get; set; }
        [Storable]
        public int InitialFood { get; private set; }
        [Storable]
        public List<Creature> History { get; set; }
        [Storable]
        public bool RepeatSense { get; set; }

        [StorableConstructor]
        public World(bool deserializing)
        {
        }

        public World(int width, int height, IntMatrix matrix)
        {
            Width = width;
            Height = height;
            History = new List<Creature>();
            InitWorld(width, height);
            InitFood(matrix);
            RepeatSense = true;
        }

        public World(int width, int height, int seed)
        {
            Width = width;
            Height = height;
            History = new List<Creature>();
            InitWorld(width, height);
            InitFood(seed);
            RepeatSense = true;
        }

        private void InitFood(IntMatrix matrix)
        {
            for (int y = 0; y < matrix.Rows; y++)
            {
                for (int x = 0; x < matrix.Columns; x++)
                {
                    if (matrix[y, x] > 0)
                    {
                        this[x, y] = WorldStatus.Food;
                        Food++;
                    }
                }
            }
            InitialFood = Food;
        }

        private void InitFood(int seed)
        {
            Random rand = new Random(seed);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (rand.Next(100) > 80)
                    {
                        this[x, y] = WorldStatus.Food;
                        Food++;
                    }
                }
            }
            InitialFood = Food;
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

        private void InitWorld(int width, int height)
        {
            Status = new WorldStatus[height][];

            for (int y = 0; y < height; y++)
            {
                Status[y] = new WorldStatus[width];
            }
        }
    }
}
