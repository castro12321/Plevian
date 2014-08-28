using Plevian.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Units
{
    public class RecruitQueueItem
    {
        public readonly GameTime start;
        public readonly GameTime end;
        public readonly Unit toRecruit;

        public RecruitQueueItem(GameTime start, GameTime end, Unit toRecruit)
        {
            this.start = start;
            this.end = end;
            this.toRecruit = toRecruit;
        }
    }
}
