using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.TechnologY
{
    public class ResearchQueueItem : Plevian.Villages.VillageQueues.QueueItem
    {
        public readonly Technology researched;

        public ResearchQueueItem(GameTime start, GameTime end, Technology researched)
            : base(researched.Name, null, start, end)
        {
            this.researched = researched;
        }
    }
}
