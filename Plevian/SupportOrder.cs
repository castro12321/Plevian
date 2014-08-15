﻿using Plevian.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Orders
{
    class SupportOrder : Order
    {

        public SupportOrder(Location origin, Location destination, float timePerTile)
            : base(origin, destination, timePerTile)
        {

        }
    }
}
