using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Plevian.Maps;
using Plevian.Debugging;
using Plevian.Util;
using SFML.Window;

namespace Plevian.Maps
{
    // TODO: <MapRenderer>
    // - Wyswietlac informacje o tile po najechaniu (nie po kliknieciu) jako tooltip
    // - Po kliknieciu otwierac okienko powiazane z danym tilem
    public class MapView : System.Windows.Forms.UserControl
    {
        private const int tileSizeInPixels = 28;
        private const float MouseDragStart = 2f;

        private RenderWindow renderer;
        private readonly Map map;

        public event EventHandler<TileClickedEventArgs> PlevianTileClickedEvent;
        public event EventHandler<MouseMovedEventArgs>  PlevianMouseMovedEvent;
        public event EventHandler<MapDraggedEventArgs> PlevianMapDraggedEvent;



        private bool mouseDrag = false;
        private bool mouseLeftClicked = false;
        private Location previousMouseLocation = new Location(0, 0);
        private Location mouseDownStartLocation = new Location(0, 0);
        private int tileWidth, tileHeight;


        public Camera camera { get; private set; }


        public MapView(Map map, System.Windows.Forms.Integration.WindowsFormsHost host)
        {
            this.map = map;
            
            renderer = new RenderWindow(Handle); // Only to avoid nulls. Will be recreated in next control resize (which is sent automatically when the window initializes)

            camera = new Camera(Size.Width, Size.Height, tileSizeInPixels * map.sizeX, tileSizeInPixels * map.sizeY);

            MouseMove  += MapRenderer_MouseMove;
            MouseDown += MapRenderer_MouseDown;
            MouseUp += MapRenderer_MouseUp;
            Resize += MapView_Resize;
            setTileBounds();
        }


        void MapView_Resize(object sender, EventArgs e)
        {
            Logger.graphics("Resized MapView, new size: " + Size.Width + " " + Size.Height);
            renderer.Dispose();
            renderer = new RenderWindow(Handle);
            camera.changeSize(Size.Width, Size.Height);
            setTileBounds();
        }

        public void setTileBounds()
        {
            tileWidth = Size.Width / tileSizeInPixels + 2;
            tileHeight = Size.Height / tileSizeInPixels + 2;
          
        }

        void MapRenderer_MouseDown(object sender, MouseEventArgs e)
        {
            int mousePositionX = e.X;
            int mousePositionY = e.Y;
            Location mouseLocationPixels = new Location(mousePositionX, mousePositionY);

            if(e.Button == MouseButtons.Left )    
            {
                mouseDownStartLocation = mouseLocationPixels;
                mouseLeftClicked = true;
            }       
        }

        void MapRenderer_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int mousePositionX = e.X;
                int mousePositionY = e.Y;
                Location mouseLocationPixels = new Location(mousePositionX, mousePositionY);

                if (mouseLeftClicked && !mouseDrag)
                {
                    Location clickedTileLocation = PixelToTile(mouseLocationPixels);
                    Tile clickedTile = map.typeAt(clickedTileLocation);

                    EventHandler<TileClickedEventArgs> handler = PlevianTileClickedEvent;
                    if (handler != null)
                        handler(this, new TileClickedEventArgs(clickedTileLocation, clickedTile));
                }

                mouseDrag = false;
                mouseLeftClicked = false;
            }
        }

        /*void MapRenderer_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int clickedX = e.X;
            int clickedY = e.Y;
            Location clickedPixelLocation = new Location(clickedX, clickedY);
            Location clickedTileLocation = PixelToTile(clickedPixelLocation);
            Tile clickedTile = map.typeAt(clickedTileLocation);

            EventHandler<TileClickedEventArgs> handler = PlevianTileClickedEvent;
            if (handler != null)
                handler(this, new TileClickedEventArgs(clickedTileLocation, clickedTile));
        }*/

        void MapRenderer_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int mousePositionX = e.X;
            int mousePositionY = e.Y;
            Location mouseLocationPixels = new Location(mousePositionX, mousePositionY);
            Location mouseLocationTile = PixelToTile(mouseLocationPixels);

           
            EventHandler<MouseMovedEventArgs> movedHandler = PlevianMouseMovedEvent;
            if (movedHandler != null)
                movedHandler(this, new MouseMovedEventArgs(mouseLocationTile));

            if (MouseButtons != MouseButtons.Left)
            {
                mouseDrag = false;
                mouseLeftClicked = false;
            }

            if (mouseLeftClicked)
            {
                if (mouseDrag == false)
                {
                    if (mouseLocationPixels.distance(mouseDownStartLocation) > MouseDragStart)
                    {
                        mouseDrag = true;
                    }
                }
                else
                {
                    EventHandler<MapDraggedEventArgs> dragHandler;
                    dragHandler = PlevianMapDraggedEvent;
                    if (dragHandler != null)
                        dragHandler(this, new MapDraggedEventArgs(previousMouseLocation, mouseLocationPixels));

                    Location diffrence = mouseLocationPixels - previousMouseLocation;
                    diffrence = diffrence.invert();
                    camera.moveCameraBy(diffrence);

                }
            }

            previousMouseLocation = mouseLocationPixels;
        }

        private Location PixelToTile(Location pixels)
        {

            return new Location((pixels.x + (int)camera.x) / tileSizeInPixels, (pixels.y + (int)camera.y) / tileSizeInPixels);
        }

        public void handleEvents()
        {
            renderer.DispatchEvents(); // handle SFML events - NOTE this is still required when SFML is hosted in another window
        }

        public void render()
        {
            renderer.Clear(Utils.smoothSFMLColor());

            int startX = (int)camera.x / tileSizeInPixels;
            int startY = (int)camera.y / tileSizeInPixels;

            for (int i = 0; i < tileHeight ; ++i)
                for (int j = 0; j < tileWidth; ++j)
                {
                    int x = j + startX;
                    int y = i + startY;
                    if (x < 0 || x >= map.sizeX) continue;
                    if (y < 0 || y >= map.sizeY) continue;
                    TerrainType type = map.typeAt(new Location(x, y)).type;
                    Shape tile = getShapeFor(type);
                    tile.Position = camera.translate(new Vector2f(x * tileSizeInPixels, y * tileSizeInPixels));
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
