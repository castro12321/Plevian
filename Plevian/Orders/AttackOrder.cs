using Plevian.Maps;
using Plevian.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Orders
{
    class AttackOrder : Order
    {

        public AttackOrder(Location origin, Location destination, float timePerTile, Army army)
            : base(origin, destination, timePerTile, army)
        {

        }

        public override void onEnd()
        {
 	        throw new NotImplementedException();
        }

    }
}
