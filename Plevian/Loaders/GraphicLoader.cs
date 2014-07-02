using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
namespace Plevian.Loaders
{
    class GraphicLoader
    {
        private static GraphicLoader loader;
        private static Dictionary<string, Texture> textures = new Dictionary<string,Texture>();


        public static void init()
        {

        }

        private static void loadStandardTextures()
        {

        }

        public static void addTexture(string name, Texture texture)
        {
            if (textures.ContainsKey(name))
                throw new Exception("Trying to add " + name + " to GraphicLoader when this texture exist!");
            textures.Add(name, texture);
        }

        public static void addTexture(string name, string filename)
        {
            if (textures.ContainsKey(name))
                throw new Exception("Trying to add " + name + " to GraphicLoader when this texture exist!");
            Texture texture = new Texture(filename);
            textures.Add(name, texture);
        }

        public static void addTexture(string filename, bool cutExtension = true)
        {
            throw new NotImplementedException();
        }

        public static Texture get(string name)
        {
            return textures[name]; //May throw exception when texture is not loaded into memory.
        }



    }
}
