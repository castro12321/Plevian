using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Map
{
    class Map
    {
        public readonly int sizeX, sizeY;
        private TerrainType[,] fields;

        public Map(int x, int y)
        {
            this.sizeX = x;
            this.sizeY = y;
            fields = new TerrainType[x, y];
            for (int _x = 0; _x < x; ++_x)
                for (int _y = 0; _y < y; ++_y)
                    place(new Location(_x, _y), TerrainType.PLAINS);
        }

        public void place(Location where, TerrainType type)
        {
            fields[where.x, where.y] = type;
        }

        public TerrainType typeAt(Location where)
        {
            return fields[where.x, where.y];
        }

        public TerrainType[,] getMap()
        {
            return fields;
        }
    }
}
