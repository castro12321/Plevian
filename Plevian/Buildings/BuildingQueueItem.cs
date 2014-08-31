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
        public readonly Building toBuild;
        public int level;

        public BuildingQueueItem(GameTime start, GameTime end, Building toBuild, int level)
        {
            this.start = start;
            this.end = end;
            this.toBuild = toBuild;
            this.level = level;
        }
    }
}
