using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Buildings
{
    public class BuildingQueueItem : Plevian.Villages.Queues.QueueItem
    {
        public readonly Building toBuild;
        public int level;

        public BuildingQueueItem(GameTime start, GameTime end, Building toBuild, int level)
            : base(toBuild.getDisplayName(), level.ToString(), start, end)
        {
            this.toBuild = toBuild;
            this.level = level;
        }
    }
}
