using Plevian.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.TechnologY
{
    public class TechnologyTile : Tile
    {
        public readonly Technology technology;
        public bool hovered = false;

        public TechnologyTile(Location location, Technology technology)
            : base(location, TerrainType.TEHNOLOGY)
        {
            this.technology = technology;
        }
    }
}
