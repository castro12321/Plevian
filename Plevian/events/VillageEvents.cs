﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Buildings;
using Plevian.Villages;
using Plevian.TechnologY;

namespace Plevian.events
{
    public delegate void BuildingQueueItemAdded(Village village, BuildingQueueItem item);
    public delegate void BuildingBuilt(Village vilalge, Building building);
    public delegate void BuildingQueueItemRemoved(Village village, BuildingQueueItem item);

    public delegate void TechnologyQueueItemAdded(Village village, ResearchQueueItem item);
    public delegate void TechnologyResearched(Village village, Technology technology);
}
