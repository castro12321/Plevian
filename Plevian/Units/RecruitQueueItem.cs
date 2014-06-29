using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Units
{
    class RecruitQueueItem
    {
        public readonly Unit unit;
        public readonly Seconds end;
        public int remainingQuanity { get { return unit.quanity; } set { unit.quanity = value; } }
        /// <summary>
        /// recruit time for invidual unit.
        /// </summary>
        public float recruitTime { get { return unit.getRecruitTime(); } }
        /// <summary>
        /// Time on unit now being recruted. It indicates how much time left for it's recruit
        /// </summary>
        public float timeCurrent;

        public RecruitQueueItem(Unit unit)
        {
            this.unit = unit;
            remainingQuanity = unit.quanity ;
            timeCurrent = recruitTime;
            end = new Seconds((int)(unit.quanity * recruitTime));
        }
    }
}
