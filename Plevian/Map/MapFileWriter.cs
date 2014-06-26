using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Plevian.Map
{
    class MapFileWriter
    {
        public void save(Map map, Save save)
        {
            FileStream fs = new FileStream(save.getMapFile(), FileMode.OpenOrCreate, FileAccess.Write);

            TerrainTypes[,] fields = map.getMap();
            for (int row = 0; row < map.sizeY; ++row)
            {
                for (int col = 0; col < map.sizeX; ++col)
                {
                    TerrainTypes type = fields[col, row];
                    fs.WriteByte((byte)type);
                }
                fs.WriteByte((byte)'\n');
            }

            fs.Close();
        }
    }
}
