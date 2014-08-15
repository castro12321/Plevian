using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Units
{
    public class RecruitQueueItem
    {
        public readonly Unit unit;
        public readonly GameTime startRecruiting;
        public readonly GameTime endRecruiting;
        public readonly Seconds duration;
        public int remainingQuanity { get { return unit.quanity; } set { unit.quanity = value; } }
        /// <summary>
        /// recruit time for invidual unit.
        /// </summary>
        public float recruitTime { get { return unit.getRecruitTime(); } }
        /// <summary>
        /// Time on unit now being recruted. It indicates how much time left for it's recruit
        /// </summary>
        public float timeCurrent;

        public RecruitQueueItem(Unit unit, GameTime startRecruiting)
        {
            this.unit = unit;
            this.startRecruiting = startRecruiting;
            remainingQuanity = unit.quanity;
            timeCurrent = recruitTime;
            duration = new Seconds((int)(unit.quanity * recruitTime));
            this.endRecruiting = startRecruiting + duration;
        }
    }
}
