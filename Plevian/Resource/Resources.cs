using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Plevian.Resource
{
    // TODO: przerobic na ten drugi system z notify zamiast dependencyproperty + overrideowac gethashcode()
    public class Resources : INotifyPropertyChanged
    {
        private int _food;
        public int food
        {
            get { return _food; }
            set { _food = value; NotifyPropertyChanged(); }
        }

        private int _wood;
        public int wood
        {
            get { return _wood; }
            set { _wood = value; NotifyPropertyChanged(); }
        }

        private int _iron;
        public int iron
        {
            get { return _iron; }
            set { _iron = value; NotifyPropertyChanged(); }
        }

        private int _stone;
        public int stone
        {
            get { return _stone; }
            set { _stone = value; NotifyPropertyChanged(); }
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

        public void Clear()
        {
            food = wood = iron = stone = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
