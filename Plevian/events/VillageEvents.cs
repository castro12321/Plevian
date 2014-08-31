using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Buildings;
using Plevian.Villages;

namespace Plevian.events
{
    public delegate void BuildingQueueItemAdded(Village village, BuildingQueueItem item);
    public delegate void BuildingQueueItemRemoved(Village village, BuildingQueueItem item);
}
