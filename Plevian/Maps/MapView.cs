using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using Plevian.Maps;
using Plevian.Debugging;
using Plevian.Util;

namespace Plevian.Maps
{
    // TODO: <MapRenderer>
    // - Wyswietlac informacje o tile po najechaniu (nie po kliknieciu) jako tooltip
    // - Po kliknieciu otwierac okienko powiazane z danym tilem
    public class MapView : System.Windows.Forms.UserControl
    {
        private const int tileSizeInPixels = 28;

        private RenderWindow renderer;
        private readonly Map map;

        public event EventHandler<TileClickedEventArgs> PlevianTileClickedEvent;
        public event EventHandler<MouseMovedEventArgs>  PlevianMouseMovedEvent;

        public MapView(Map map, System.Windows.Forms.Integration.WindowsFormsHost host)
        {
            this.map = map;
            
            renderer = new RenderWindow(Handle); // Only to avoid nulls. Will be recreated in next control resize (which is sent automatically when the window initializes)

            MouseMove  += MapRenderer_MouseMove;
            MouseClick += MapRenderer_MouseClick;
            Resize += MapView_Resize;
        }

        void MapView_Resize(object sender, EventArgs e)
        {
            Logger.graphics("Resized MapView, new size: " + Size.Width + " " + Size.Height);
            renderer.Dispose();
            renderer = new RenderWindow(Handle);
        }

        void MapRenderer_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int clickedX = e.X;
            int clickedY = e.Y;
            Location clickedPixelLocation = new Location(clickedX, clickedY);
            Location clickedTileLocation = PixelToTile(clickedPixelLocation);
            TerrainType clickedTile = map.typeAt(clickedTileLocation);

            EventHandler<TileClickedEventArgs> handler = PlevianTileClickedEvent;
            if (handler != null)
                handler(this, new TileClickedEventArgs(clickedTileLocation, clickedTile));
        }

        void MapRenderer_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int mousePositionX = e.X;
            int mousePositionY = e.Y;
            Location mouseLocationPixels = new Location(mousePositionX, mousePositionY);
            Location mouseLocationTile = PixelToTile(mouseLocationPixels);

            EventHandler<MouseMovedEventArgs> handler = PlevianMouseMovedEvent;
            if (handler != null)
                handler(this, new MouseMovedEventArgs(mouseLocationTile));
        }

        private Location PixelToTile(Location pixels)
        {
            return new Location(pixels.x / tileSizeInPixels, pixels.y / tileSizeInPixels);
        }

        public void handleEvents()
        {
            renderer.DispatchEvents(); // handle SFML events - NOTE this is still required when SFML is hosted in another window
        }

        public void render()
        {
            renderer.Clear(Utils.smoothSFMLColor());

            for (int i = 0; i < map.sizeX; ++i)
                for (int j = 0; j < map.sizeY; ++j)
                {
                    TerrainType type = map.typeAt(new Location(i, j));
                    Shape tile = getShapeFor(type);
                    tile.Position = new SFML.Window.Vector2f(i * tileSizeInPixels, j * tileSizeInPixels);
                    renderer.Draw(tile);
                }

           renderer.Display();
        }

        private Shape shape = new RectangleShape(new SFML.Window.Vector2f(tileSizeInPixels, tileSizeInPixels));
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
