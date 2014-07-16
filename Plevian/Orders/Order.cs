using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Maps;
namespace Plevian.Orders
{
    public class Order
    {
        Location origin;
        Location destination;
        Seconds duration;
        GameTime endTime;

        public Order(Location origin, Location destination, float timePerTile)
        {
            this.origin = origin;
            this.destination = destination;

            float distance = origin.distance(destination);
            duration = new Seconds((int) (timePerTile * distance));
            endTime = GameTime.now + duration;
          
        }


        public abstract onEnd(


    }
}
