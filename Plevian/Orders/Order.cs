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
        public static Seconds timePerTile = new Seconds(10);

        Location origin;
        Location destination;
        Seconds duration;
        GameTime endTime;

        public Order(Location origin, Location destination)
        {
            this.origin = origin;
            this.destination = destination;

            float distance = origin.distance(destination);
            duration = new Seconds((int) ( (float)timePerTile.seconds * distance));
            endTime = GameTime.now + duration;
          
        }


    }
}
