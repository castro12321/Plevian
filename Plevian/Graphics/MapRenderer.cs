using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using Plevian.Maps;

namespace Plevian.Graphics
{
    class MapRenderer : System.Windows.Forms.Control
    {
        private readonly RenderWindow renderer;
        private readonly Map map;

        public MapRenderer(Map map)
        {
            this.map = map;
            Size = new System.Drawing.Size(400, 400);
            //Location = new System.Drawing.Point(100, 100);
            renderer = new RenderWindow(Handle);
        }

        public void handleEvents()
        {
            renderer.DispatchEvents(); // handle SFML events - NOTE this is still required when SFML is hosted in another window
        }

        public void render()
        {
            renderer.Clear(randomColor());

            for (int i = 0; i < map.sizeX; ++i)
                for (int j = 0; j < map.sizeY; ++j)
                {
                    TerrainType type = map.typeAt(new Location(i, j));
                    Shape tile = getShapeFor(type);
                    tile.Position = new SFML.Window.Vector2f(i*28, j*28);
                    renderer.Draw(tile);
                }

           renderer.Display();
        }

        private Shape shape = new CircleShape(14, 6);
        private Shape getShapeFor(TerrainType type)
        {
            switch(type)
            {
                case TerrainType.LAKES: shape.FillColor = Color.Blue; break;
                case TerrainType.MOUNTAINS: shape.FillColor = Color.White; break;
                case TerrainType.PLAINS: shape.FillColor = Color.Green; break;
                case TerrainType.VILLAGE: shape.FillColor = Color.Red; break;
            }
            return shape;
        }

        private static Random random = new Random();
        byte oldR = 64;
        byte oldG = 64;
        byte oldB = 64;
        private Color randomColor()
        {
            oldR = (byte)random.Next(oldR - 2, oldR + 3);
            oldG = (byte)random.Next(oldG - 2, oldG + 3);
            oldB = (byte)random.Next(oldB - 2, oldB + 3);
            return new Color(oldR, oldG, oldB);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            // don't call base.OnPaint(e) to prevent forground painting
            // base.OnPaint(e);
        }
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
        {
            // don't call base.OnPaintBackground(e) to prevent background painting
            //base.OnPaintBackground(pevent);
        }
    }
}
