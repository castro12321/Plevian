﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Plevian.Save;

namespace Plevian.Maps
{
    /// <summary>
    /// Provides naive and slow map reading
    /// </summary>
    public class MapFileReader
    {
        public Map read(SaveWriter save)
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
                    TerrainType tType = (TerrainType)type;
                    Location location = new Location(x, y);
                    map.place(new Tile(location, tType));
                }
            }

            fs.Close();
            return map;
        }
    }
}
