using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Resources
{
    public class Resources
    {
        public readonly int 
            food,
            wood,
            iron,
            stone;
        
        public Resources(int food, int wood, int iron, int stone)
        {
            this.food = food;
            this.wood = wood;
            this.iron = iron;
            this.stone = stone;
        }

        public Resources()
        {
            wood = food = iron = stone = 0;
        }

        public static Resources operator +(Resources lh, Resources rh)
        {
            int food = lh.food + rh.food;
            int wood = lh.wood + rh.wood;
            int iron = lh.iron + rh.iron;
            int stone = lh.stone + rh.stone;
            return new Resources(food, wood, iron, stone);
        }

        public static Resources operator -(Resources lh, Resources rh)
        {
            int food = lh.food - rh.food;
            int wood = lh.wood - rh.wood;
            int iron = lh.iron - rh.iron;
            int stone = lh.stone - rh.stone;
            return new Resources(food, wood, iron, stone);
        }

        /* Probably will be unused. Uncomment later
        public static bool operator >(Resources lh, Resources rh)
        {
            return (lh.food > rh.food && lh.stone > rh.stone && lh.wood > rh.wood && lh.iron > rh.iron);
        }

        public static bool operator >=(Resources lh, Resources rh)
        {
            return (lh.food >= rh.food && lh.stone >= rh.stone && lh.wood >= rh.wood && lh.iron >= rh.iron);
        }

        public static bool operator <(Resources lh, Resources rh)
        {
            return (lh.food < rh.food || lh.stone < rh.stone || lh.wood < rh.wood || lh.iron < rh.iron);
        }

        public static bool operator <=(Resources lh, Resources rh)
        {
            return (lh.food <= rh.food || lh.stone <= rh.stone || lh.wood <= rh.wood || lh.iron <= rh.iron);
        }
        */
    }
}
