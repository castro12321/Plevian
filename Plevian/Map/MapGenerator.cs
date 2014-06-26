using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Map
{
    class MapGenerator
    {
        public MapGenerator()
        {
            init(); // it can be deleted in future.
        }

        private void init() { }

        private Map Generate(int x, int y)
        {
            Map map = new Map(x, y);
            Random rand = new Random();
            foreach (TerrainTypes terType in (TerrainTypes[])Enum.GetValues(typeof(TerrainTypes)))
            {
                if (terType == TerrainTypes.PLAINS) continue;

                for(int i = 0;i < 10;++i)
                {
                    Location loc = new Location(rand.Next(x), rand.Next(y));
                    map.place(loc, terType);
                }
            }
            return map;
        }

    }
}
