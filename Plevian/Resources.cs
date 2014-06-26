using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian
{
    class Resources
    {
        int food,
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
            Resources res = new Resources();
            res.wood = lh.wood + rh.wood;
            res.iron = lh.iron + rh.iron;
            res.stone = lh.stone + rh.stone;
            res.food = lh.food + rh.food;
            return res;
        }

        public static Resources operator -(Resources lh, Resources rh)
        {
            Resources res = new Resources();
            res.wood = lh.wood - rh.wood;
            res.iron = lh.iron - rh.iron;
            res.stone = lh.stone - rh.stone;
            res.food = lh.food - rh.food;
            return res;
        }

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
    }
}
