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
        }

        public void place(Location where, TerrainTypes type)
        {
            fields[where.x, where.y] = type;
        }
    }
}
