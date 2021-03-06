﻿using System;
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
        private long _food;
        public long food
        {
            get { return _food; }
            set { _food = value; NotifyPropertyChanged(); }
        }

        private long _wood;
        public long wood
        {
            get { return _wood; }
            set { _wood = value; NotifyPropertyChanged(); }
        }

        private long _iron;
        public long iron
        {
            get { return _iron; }
            set { _iron = value; NotifyPropertyChanged(); }
        }

        private long _stone;
        public long stone
        {
            get { return _stone; }
            set { _stone = value; NotifyPropertyChanged(); }
        }

        public Resources(long food, long wood, long iron, long stone)
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
            long food = lh.food + rh.food;
            long wood = lh.wood + rh.wood;
            long iron = lh.iron + rh.iron;
            long stone = lh.stone + rh.stone;
            return new Resources(food, wood, iron, stone);
        }

        public static Resources operator -(Resources lh, Resources rh)
        {
            long food = lh.food - rh.food;
            long wood = lh.wood - rh.wood;
            long iron = lh.iron - rh.iron;
            long stone = lh.stone - rh.stone;
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

        public static Resources operator *(Resources lh, Resources rh)
        {
            long wood = (long)(lh.wood * rh.wood);
            long food = (long)(lh.food * rh.food);
            long iron = (long)(lh.iron * rh.iron);
            long stone = (long)(lh.stone * rh.stone);
            return new Resources(food, wood, iron, stone);
        }

        public static Resources operator *(Resources lh, float rh)
        {
            long wood = (long)(lh.wood * rh);
            long food = (long)(lh.food * rh);
            long iron = (long)(lh.iron * rh);
            long stone = (long)(lh.stone * rh);
            return new Resources(food, wood, iron, stone);
        }

        public static Resources operator *(Resources lh, long rh)
        {
            return lh * (float)rh;
        }

        public static Resources operator /(Resources lh, Resources rh)
        {
            long wood = (long)(lh.wood / rh.wood);
            long food = (long)(lh.food / rh.food);
            long iron = (long)(lh.iron / rh.iron);
            long stone = (long)(lh.stone / rh.stone);
            return new Resources(food, wood, iron, stone);
        }

        public static Resources operator /(Resources lh, float rh)
        {
            long wood = (long)(lh.wood / rh);
            long food = (long)(lh.food / rh);
            long iron = (long)(lh.iron / rh);
            long stone = (long)(lh.stone / rh);
            return new Resources(food, wood, iron, stone);
        }

        public static Resources operator /(Resources lh, long rh)
        {
            return lh / (float)rh;
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

        public override bool Equals(object obj)
        {
            Resources rh = obj as Resources;
            if(rh != null)
                return this == rh;
            return false;
        }

        public static bool operator != (Resources lh, Resources rh)
        {
            return !(lh == rh);
        }

        public override string ToString()
        {
            return "F:" + food + " W:" + wood + " S:" + stone + " I:" + iron;
        }

        /// <summary>
        /// Sums all resources
        /// </summary>
        /// <returns>Return food+iron+wood+stone</returns>
        public long sumResources()
        {
            return food + iron + wood + stone;
        }

        public int howMuchAfford(Resources cost)
        {
            long q = 24000000; // wut?
            if(cost.wood != 0)
                q = wood / cost.wood;
            if (cost.food != 0 && food / cost.food < q)
                q = food / cost.food;
            if (cost.iron != 0 && iron / cost.iron < q)
                q = iron / cost.iron;
            if (cost.stone != 0 && stone / cost.stone < q)
                q = stone / cost.stone;
            if(q == 24000000)
                return 0;
            return (int)q;
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

        public override int GetHashCode()
        {
            // TODO: Better hashCode algorithm
            return food.GetHashCode();
        }
    }
}
