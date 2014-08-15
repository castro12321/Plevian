using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Maps
{
    public class Location
    {
        public readonly int x, y;

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public float distance(Location other)
        {
            return (float)Math.Sqrt(Math.Pow(other.x - x, 2) + Math.Pow(other.y - y, 2));
        }
    }
}
