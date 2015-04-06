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
using Plevian.Villages;
using Plevian.TechnologY;
using SFML.System;

namespace Plevian.Maps
{
    // TODO: <MapRenderer>
    // - Wyswietlac informacje o tile po najechaniu (nie po kliknieciu) jako tooltip
    // - Po kliknieciu otwierac okienko powiazane z danym tilem
    public class MapView : System.Windows.Forms.UserControl
    {
        private const int tileSizeInPixels = 32;
        private const float MouseDragStart = 2f;

        private RenderWindow renderer;
        private readonly Map map;

        public event EventHandler<TileClickedEventArgs> PlevianTileClickedEvent;
        public event EventHandler<MouseMovedEventArgs> PlevianMouseMovedEvent;
        public event EventHandler<MapDraggedEventArgs> PlevianMapDraggedEvent;

        private bool mouseDrag = false;
        private bool mouseLeftClicked = false;
        private Location previousMouseLocation = new Location(0, 0);
        private Location mouseDownStartLocation = new Location(0, 0);
        private int tileWidth, tileHeight;

        public Camera camera { get; private set; }

        private Texture
            technologyTex = Utils.ToSFMLTexture(Properties.Resources.technology),
            mountainsTex = Utils.ToSFMLTexture(Properties.Resources.mountains),
            villageTex = Utils.ToSFMLTexture(Properties.Resources.village),
            plainsTex = Utils.ToSFMLTexture(Properties.Resources.plains),
            lakeTex = Utils.ToSFMLTexture(Properties.Resources.lake);
        private Sprite
            //village = new Sprite(villageTex);
            village, lake, plains, mountains, technology;
        public MapView(Map map, System.Windows.Forms.Integration.WindowsFormsHost host)
        {
            //villageTex = new Texture(@"village");
            //villageTex = new Texture(Properties.Resources.village);
            village = setupSprite(villageTex);
            village = setupSprite(villageTex);
            lake = setupSprite(lakeTex);
            plains = setupSprite(plainsTex);
            mountains = setupSprite(mountainsTex);
            technology = setupSprite(technologyTex);
            this.map = map;
            
            renderer = new RenderWindow(Handle); // Only to avoid nulls. Will be recreated in next control resize (which is sent automatically when the window initializes)

            camera = new Camera(Size.Width, Size.Height, tileSizeInPixels * map.sizeX, tileSizeInPixels * map.sizeY);

            MouseMove += MapRenderer_MouseMove;
            MouseDown += MapRenderer_MouseDown;
            MouseUp += MapRenderer_MouseUp;
            Resize += MapView_Resize;
            setTileBounds();
        }

        private Sprite setupSprite(Texture texture)
        {
            Sprite sprite = new Sprite(texture);
            sprite.Origin = new Vector2f(texture.Size.X / 2, texture.Size.Y / 2); // Just center the origin
            sprite.Scale = new Vector2f(1.0f/3.0f, 1.0f/3.0f); // We are using 96x96px textures but 32x32 sprites, so we need to rescale it. No idea why it's hardcoded here, but maybe someone change it in the future
            sprite.Rotation = 90; // For some reason, SFML loads sprites rotated -90* lol
            return sprite;
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

            if (e.Button == MouseButtons.Left)
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
                    Tile clickedTile = map.tileAt(clickedTileLocation);
                    if (clickedTile == null)
                        return;

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
                movedHandler(this, new MouseMovedEventArgs(mouseLocationTile, map.tileAt(mouseLocationTile)));

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

            for (int i = 0; i < tileHeight; ++i)
                for (int j = 0; j < tileWidth; ++j)
                {
                    int x = j + startX;
                    int y = i + startY;
                    if (x < 0 || x >= map.sizeX) continue;
                    if (y < 0 || y >= map.sizeY) continue;
                    Tile tile = map.tileAt(new Location(x, y));
                    Sprite tileSprite = getShapeFor(tile);
                    tileSprite.Position = camera.translate(new Vector2f(x * tileSizeInPixels + (tileWidth / 2), y * tileSizeInPixels + (tileHeight / 2)));
                    renderer.Draw(tileSprite);
                }
            renderer.Display();
        }

        private Sprite getShapeFor(Tile tile)
        {
            TerrainType type = tile.type;
            switch (type)
            {
                case TerrainType.LAKES: return lake;
                case TerrainType.MOUNTAINS: return mountains;
                case TerrainType.PLAINS: return plains;
                case TerrainType.VILLAGE:
                    {
                        if (!(tile is Village))
                            throw new ArgumentException("Tile has wrong type");

                        Village village = tile as Village;
                        this.village.Color = village.Owner.color;
                        return this.village;
                    }
                case TerrainType.TEHNOLOGY:
                    {
                        if (!(tile is TechnologyTile))
                            throw new ArgumentException("Tile has wrong type");

                        TechnologyTile techTile = tile as TechnologyTile;
                        Village village = MainWindow.getInstance().villageTab.Village;
                        if (village.Owner.technologies.isDiscovered(techTile.technology))
                            technology.Color = SFML.Graphics.Color.Green;
                        else if (village.canResearch(techTile.technology))
                        {
                            if (techTile.hovered)
                                technology.Color = new SFML.Graphics.Color(192, 192, 192);
                            else
                                technology.Color = SFML.Graphics.Color.White;
                        }
                        else
                            technology.Color = SFML.Graphics.Color.Red;
                        return technology;
                    }
            }
            return null;
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
