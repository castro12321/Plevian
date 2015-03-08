using Plevian.Debugging;
using Plevian.Util;
using Plevian.Villages;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Integration;
using Plevian.Buildings;
using System.IO;

namespace Plevian.Maps
{
    class BuildingTexture
    {
        private static readonly Random r = new Random();
        //CityType cityType;
        public readonly BuildingType buildingType;
        public readonly int level;
        

        public BuildingTexture(BuildingType buildingType, int level)
        {
            this.buildingType = buildingType;
            this.level = level;
        }

        public override int GetHashCode()
        {
            return (buildingType.ToString() + level).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is BuildingTexture)
                return buildingType == ((BuildingTexture)obj).buildingType 
                    && level == ((BuildingTexture)obj).level;
            return false;
        }

        public String baseTexturePath()
        {
            return "..\\..\\GFX\\buildings\\" + buildingType + ".png";
        }

        public String texturePath()
        {
            return "..\\..\\GFX\\buildings\\" + buildingType + level + ".png";
        }
        
        public String defaultTexturePath()
        {
            return "..\\..\\GFX\\buildings\\default.png";
        }

        public Texture loadTexture()
        {
            try
            {
                return new Texture(texturePath());
            }
            catch(Exception e)
            {
                Logger.warn("Exception: " + e.Message + "; " + e.StackTrace);
                return new Texture(defaultTexturePath());
            }
        }
    }

    class Textures
    {
        public readonly Texture background = new Texture("..\\..\\GFX\\citybackground.png");
        private Dictionary<BuildingTexture, Texture> textures = new Dictionary<BuildingTexture,Texture>();

        public Sprite getSprite(Building building)
        {
            BuildingTexture bt = new BuildingTexture(building.type, building.level);
            if (!textures.ContainsKey(bt))
                textures.Add(bt, bt.loadTexture());
            Sprite sprite = new Sprite(textures[bt]);

            String positionPath = bt.texturePath() + ".txt";
            if (!File.Exists(positionPath))
                positionPath = bt.baseTexturePath() + ".txt";
            if (!File.Exists(positionPath))
                positionPath = bt.defaultTexturePath() + ".txt";
            String positionText = File.ReadAllText(positionPath);
            String[] positionSplit = positionText.Split(' ');
            sprite.Position = new SFML.Window.Vector2f(float.Parse(positionSplit[0]), float.Parse(positionSplit[1]));
            return sprite;
        }
    }

    class VillageView : System.Windows.Forms.Control
    {
        private static Textures textures = new Textures();
        

        private RenderWindow renderer;
        public Village village { get; set; }

        public VillageView(Game game)
        {
            Size = new System.Drawing.Size(400, 400);
            Location = new System.Drawing.Point(100, 100);

            renderer = new RenderWindow(Handle); // Only to avoid nulls. Will be recreated in next control resize (which is sent automatically when the window initializes)

            Resize += VillageView_Resize;
        }

        void VillageView_Resize(object sender, EventArgs e)
        {
            Logger.graphics("Resized MapView, new size: " + Size.Width + " " + Size.Height);
            renderer.Dispose();
            renderer = new RenderWindow(Handle);
        }

        public void handleEvents()
        {
            renderer.DispatchEvents(); // handle SFML events - NOTE this is still required when SFML is hosted in another window
        }

        public void render()
        {
            renderer.Clear(Utils.smoothSFMLColor());

            renderer.Draw(new Sprite(textures.background));
            foreach(var pair in village.buildings)
            {
                Building building = pair.Value;
                renderer.Draw(textures.getSprite(building));
                //drawBuilding(bSprite);
            }

            renderer.Display();
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
