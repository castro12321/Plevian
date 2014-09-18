using Plevian.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Units
{
    public class RecruitQueueItem : Plevian.Villages.Queues.QueueItem
    {
        public readonly Unit toRecruit;

        public RecruitQueueItem(GameTime start, GameTime end, Unit toRecruit)
            : base(start, end)
        {
            this.toRecruit = toRecruit;
        }
    }
}
