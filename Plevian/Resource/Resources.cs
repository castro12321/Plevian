using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Plevian.Resource
{
    // TODO: przerobic na ten drugi system z notify zamiast dependencyproperty + overrideowac gethashcode()
    public class Resources : DependencyObject
    {
        public static readonly DependencyProperty FoodProperty =
            DependencyProperty.Register("food", typeof(int),
            typeof(Resources), new FrameworkPropertyMetadata(0));
        public int food
        {
            get { return (int)GetValue(FoodProperty); }
            set { SetValue(FoodProperty, value); }
        }

        public static readonly DependencyProperty WoodProperty =
            DependencyProperty.Register("wood", typeof(int),
            typeof(Resources), new FrameworkPropertyMetadata(0));
        public int wood
        {
            get { return (int)GetValue(WoodProperty); }
            set { SetValue(WoodProperty, value); }
        }

        public static readonly DependencyProperty IronProperty =
            DependencyProperty.Register("iron", typeof(int),
            typeof(Resources), new FrameworkPropertyMetadata(0));
        public int iron
        {
            get { return (int)GetValue(IronProperty); }
            set { SetValue(IronProperty, value); }
        }

        public static readonly DependencyProperty StoneProperty =
            DependencyProperty.Register("stone", typeof(int),
            typeof(Resources), new FrameworkPropertyMetadata(0));
        public int stone
        {
            get { return (int)GetValue(StoneProperty); }
            set { SetValue(StoneProperty, value); }
        }
        
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

        public void Add(Resources toAdd)
        {
            Resources added = this + toAdd;
            food = added.food;
            wood = added.wood;
            iron = added.iron;
            stone = added.stone;
        }

        public void Substract(Resources toSubstract)
        {
            Resources substracted = this - toSubstract;
            food = substracted.food;
            wood = substracted.wood;
            iron = substracted.iron;
            stone = substracted.stone;
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

        public static bool operator == (Resources lh, Resources rh)
        {
            if (object.Equals(lh, null) || object.Equals(rh, null))
                return object.Equals(lh, null) && object.Equals(rh, null);
            return lh.food == rh.food
                && lh.wood == rh.wood
                && lh.iron == rh.iron
                && lh.stone == rh.stone;
        }

        public static bool operator != (Resources lh, Resources rh)
        {
            return !(lh == rh);
        }

        public override string ToString()
        {
            return "F: " + food + " W:" + wood + " S:" + stone + " I: " + iron;
        }

        /// <summary>
        /// Sums all resources
        /// </summary>
        /// <returns>Return food+iron+wood+stone</returns>
        public int sumResources()
        {
            return food + iron + wood + stone;
        }

        public int howMuchAfford(Resources cost)
        {
            int q = cost.wood / wood;
            if (cost.food / food < q)
                q = cost.food / food;
            if (cost.iron / iron < q)
                q = cost.iron / iron;
            if (cost.stone / food < q)
                q = cost.stone / stone;
            return q;
        }

        public void Clear()
        {
            food = wood = iron = stone = 0;
        }
    }
}
