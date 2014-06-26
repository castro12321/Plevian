using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Map
{
    class Map
    {
        private TerrainTypes[,] fields;


        public Map(int x, int y)
        {
            fields = new TerrainTypes[x, y];
            for (int _x = 0; _x < x; ++_x)
                for (int _y = 0; _y < y; ++_y)
                    place(new Location(_x, _y), TerrainTypes.PLAINS);
        }

        public void place(Location where, TerrainTypes type)
        {
            fields[where.x, where.y] = type;
        }
    }
}
