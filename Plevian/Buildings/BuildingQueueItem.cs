using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Buildings
{
    public class BuildingQueueItem
    {
        public readonly GameTime end;
        public readonly BuildingType toBuild;
        // for units add int amount;

        public BuildingQueueItem(GameTime end, BuildingType toBuild)
        {
            this.end = end;
            this.toBuild = toBuild;
        }
    }
}
