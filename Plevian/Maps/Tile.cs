using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Maps
{
    public class Tile
    {
        public Location location { get; private set; }

        public readonly TerrainType type;

        public Tile(Location location, TerrainType type)
        {
            this.location = location;
            this.type = type;
        }
    }
}
