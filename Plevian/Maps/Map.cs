using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Maps
{
    public class Map
    {
        public readonly int sizeX, sizeY;
        private Tile[,] fields;

        public Map(int x, int y)
        {
            this.sizeX = x;
            this.sizeY = y;
            fields = new Tile[x, y];
            for (int _x = 0; _x < x; ++_x)
                for (int _y = 0; _y < y; ++_y)
                {
                    Location loc = new Location(_x, _y);
                    place(new Tile(loc, TerrainType.PLAINS));
                }
        }

        public void place(Tile tile)
        {
            fields[tile.location.x, tile.location.y] = tile;
        }

        public Tile typeAt(Location where)
        {
            return fields[where.x, where.y];
        }

        public Tile[,] getMap()
        {
            return fields;
        }
    }
}
