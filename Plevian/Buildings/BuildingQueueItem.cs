using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Buildings
{
    public class BuildingQueueItem
    {
        public readonly GameTime start;
        public readonly GameTime end;
        public readonly BuildingType toBuild;

        public BuildingQueueItem(GameTime start, GameTime end, BuildingType toBuild)
        {
            this.start = start;
            this.end = end;
            this.toBuild = toBuild;
        }
    }
}
