using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Maps
{
    public class Map
    {
        /// <summary>
        /// Number of tiles in single row
        /// </summary>
        public readonly int sizeX;
        /// <summary>
        /// Number of tiles in single column
        /// </summary>
        public readonly int sizeY;
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

        public Tile tileAt(Location where)
        {
            try
            {
                return fields[where.x, where.y];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }



        public Tile[,] getMap()
        {
            return fields;
        }

        private static Random random = new Random();
        public Tile FindEmptyTile()
        {
            Location randomLocation = new Location(random.Next(sizeX), random.Next(sizeY));
            Tile randomTile = tileAt(randomLocation);
            if (randomTile.type == TerrainType.PLAINS)
                return randomTile;
            return FindEmptyTile();
        }
    }
}
