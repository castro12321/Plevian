using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Plevian.Map
{
    /// <summary>
    /// Provides naive and slow map reading
    /// </summary>
    class MapFileReader
    {
        public Map read(Save save)
        {
            FileStream fs = new FileStream(save.getMapFile(), FileMode.Open, FileAccess.Read);

            int sizeX = fs.ReadByte();
            int sizeY = fs.ReadByte();
            fs.ReadByte(); // Omit '\n'

            Map map = new Map(sizeX, sizeY);

            int x = 0;
            int y = 0;
            int type;
            while((type = fs.ReadByte()) != -1)
            {
                if(type == '\n')
                {
                    x = 0;
                    y += 1;
                }
                else
                {
                    TerrainTypes tType = (TerrainTypes)type;
                    Location location = new Location(x, y);
                    map.place(location, tType);
                }
            }

            fs.Close();
            return map;
        }
    }
}
