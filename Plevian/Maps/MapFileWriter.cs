﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Plevian.Maps
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

            TerrainType[,] fields = map.getMap();
            for (int row = 0; row < map.sizeY; ++row)
            {
                for (int col = 0; col < map.sizeX; ++col)
                {
                    TerrainType type = fields[col, row];
                    fs.WriteByte((byte)type);
                }
                fs.WriteByte((byte)'\n');
            }

            fs.Close();
        }
    }
}