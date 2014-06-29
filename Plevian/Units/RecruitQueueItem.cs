using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Units
{
    class RecruitQueueItem
    {
        public readonly UnitType unitType;
        public readonly LocalTime end;
        public readonly int remainingQuanity;
        /// <summary>
        /// recruit time for invidual unit.
        /// </summary>
        public readonly float recruitTime;
        /// <summary>
        /// Time on unit now being recruted. It indicates how much time left for it's recruit
        /// </summary>
        public float timeCurrent;

        public RecruitQueueItem(UnitType type, int quanity, int recruitTime)
        {
            unitType = type;
            remainingQuanity = quanity;
            this.recruitTime = recruitTime;
            timeCurrent = recruitTime;
            end = new LocalTime((ulong)(quanity * recruitTime));
        }
    }
}
