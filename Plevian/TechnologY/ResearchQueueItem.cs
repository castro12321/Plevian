using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.TechnologY
{
    public class ResearchQueueItem
    {
        public readonly GameTime start;
        public readonly GameTime end;
        public readonly Technology researched;

        public ResearchQueueItem(GameTime start, GameTime end, Technology researched)
        {
            this.start = start;
            this.end = end;
            this.researched = researched;
        }
    }
}
