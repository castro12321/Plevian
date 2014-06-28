using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Plevian
{
    public class Save
    {
        private readonly String path;

        public Save(String path)
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
            byte[] buffer = new byte[8]; // ulong is 64 bits = 8 bytes
            fs.Read(buffer, 0, 8);
            ulong gameTime = BitConverter.ToUInt64(buffer, 0);
            fs.Close();
            return new GameTime(new LocalTime(gameTime));
        }
    }
}
