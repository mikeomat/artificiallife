using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using HeuristicLab.Core.Views;
using HeuristicLab.MainForm;

namespace ArtificialLifePlugin
{
    [View("Artificial Life View")]
    [Content(typeof(Solution), IsDefaultView = true)]
    public sealed partial class SolutionView : NamedItemView
    {
        public new Solution Content
        {
            get { return (Solution)base.Content; }
            set { base.Content = value; }
        }

        public SolutionView()
          : base()
        {
            InitializeComponent();
            pictureBox.Image = new Bitmap(1024, 1024);
        }

        protected override void OnContentChanged()
        {
            base.OnContentChanged();
            if (Content == null)
            {
                using (var g = Graphics.FromImage(pictureBox.Image))
                {
                    g.Clear(DefaultBackColor);
                }
            }
            else
            {
                PaintTiles(pictureBox.Image, Content.World);
            }
        }

        private void PaintTiles(Image image, World world)
        {
            int w = image.Width;
            int h = image.Height;

            float tileHeight = h / (float)world.Height;
            float tileWidth = w / (float)world.Width;

            tileWidth = Math.Min(tileWidth, tileHeight);
            tileHeight = tileWidth; // draw square tiles

            using (var g = Graphics.FromImage(image))
            {
                g.Clear(DefaultBackColor);

                foreach (var history in world.History)
                {
                    bool isFirst = world.History.IndexOf(history) == 0;
                    bool isLast = world.History.IndexOf(history) == world.History.Count - 1;

                    float posX = tileWidth * history.PosX;
                    float posY = tileHeight * history.PosY;

                    Brush brush = Brushes.LightBlue;
                    if (world[history.PosX, history.PosY] == WorldStatus.Eaten)
                    {
                        brush = Brushes.Gold;
                    }

                    g.FillRectangle(brush, posX, posY, tileWidth, tileHeight);
                }

                for (int y = 0; y < Content.Height; y++)
                {
                    for (int x = 0; x < Content.Width; x++)
                    {
                        float posX = tileWidth * x;
                        float posY = tileHeight * y;
                        switch (world[x, y])
                        {
                            case WorldStatus.Eaten:
                            case WorldStatus.Food: g.DrawImage(Properties.Resources.food, new Rectangle((int)posX, (int)posY, (int)tileWidth, (int)tileHeight)); break;
                        }

                        g.DrawLine(Pens.Black, posX, posY, posX, posY + tileHeight);
                        g.DrawLine(Pens.Black, posX, posY, posX + tileWidth, posY);
                    }
                }

                var first = world.History.First();
                g.DrawImage(Properties.Resources.start, new Rectangle((int)(first.PosX * tileWidth), (int)(first.PosY * tileHeight), (int)tileWidth, (int)tileHeight));
                var last = world.History.Last();
                g.DrawImage(Properties.Resources.end, new Rectangle((int)(last.PosX * tileWidth), (int)(last.PosY * tileHeight), (int)tileWidth, (int)tileHeight));

                g.DrawLine(Pens.Black, 0, h - 1, w, h - 1);
                g.DrawLine(Pens.Black, w - 1, 0, w - 1, h);
            }
        }
    }
}