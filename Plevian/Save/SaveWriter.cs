using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Save
{
    public class SaveWriter
    {
        private readonly String path;

        public SaveWriter(String path)
        {
            this.path = path + "\\";
            //if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        }

        public String getMapFile()
        {
            return path + "map.txt";
        }

        private String getGameTimeFile()
        {
            return path + "time.txt";
        }

        public GameTime getGameTime()
        {
            FileStream fs = new FileStream(getGameTimeFile(), FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4]; // ulong is 64 bits = 8 bytes
            fs.Read(buffer, 0, 4);
            int gameTime = BitConverter.ToInt32(buffer, 0);
            fs.Close();
            return new Seconds(gameTime);
        }
    }
}
