using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Maps
{
    public class MapGenerator
    {
        public MapGenerator()
        {
            init(); // it can be deleted in future.
        }

        private void init() { }

        public Map Generate(int x, int y)
        {
            Map map = new Map(x, y);
            Random rand = new Random();
            foreach (TerrainType terType in (TerrainType[])Enum.GetValues(typeof(TerrainType)))
            {
                if (terType == TerrainType.PLAINS) continue;
                if (terType == TerrainType.VILLAGE) continue; // They are loaded later when loading villages

                for(int i = 0;i < 10;++i)
                {
                    Location loc = new Location(rand.Next(x), rand.Next(y));
                    map.place(new Tile(loc, terType));
                }
            }
            return map;
        }

    }
}
