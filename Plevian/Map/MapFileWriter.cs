﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Plevian.Map
{
    /// <summary>
    /// Provides naive and slow map saving
    /// </summary>
    class MapFileWriter
    {
        public void save(Map map, Save save)
        {
            FileStream fs = new FileStream(save.getMapFile(), FileMode.OpenOrCreate, FileAccess.Write);

            fs.WriteByte((byte)map.sizeX);
            fs.WriteByte((byte)map.sizeY);
            fs.WriteByte((byte)'\n');

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
