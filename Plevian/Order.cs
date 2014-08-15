using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Maps;
using Plevian.Units;
namespace Plevian.Orders
{
    public class Order
    {
        Location origin;
        Location destination;
        Seconds duration;
        GameTime endTime;
        Army army;



        public Order(Location origin, Location destination, float timePerTile, Army army)
        {
            this.origin = origin;
            this.destination = destination;
            this.army = army;

            float distance = origin.distance(destination);
            duration = new Seconds((int) (timePerTile * distance));
            endTime = GameTime.now + duration;
          
        }


        public abstract onEnd();


    }
}
