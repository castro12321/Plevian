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

        public Boolean canAfford(Resources price)
        {
            return this >= price;
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

        public static Resources operator *(Resources lh, float rh)
        {
            
            int wood  = (int)(lh.wood * rh);
            int food = (int)(lh.food * rh);
            int iron = (int)(lh.iron * rh);
            int stone = (int)(lh.stone * rh);
            return new Resources(food, wood, iron, stone);
        }

        public static Resources operator *(Resources lh, int rh)
        {
            return lh * (float)rh;
        }
    }
}
