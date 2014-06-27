using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Buildings
{
    public abstract class Building
    {
        public readonly BuildingType type;
        public int level { get; private set; }

        public abstract String getDisplayName();
        public abstract LocalTime getConstructionTimeFor(int level);
        public abstract Resources.Resources getPriceFor(int level);
        public abstract Resources.Resources getProduction();
        public abstract int getMaxLevel();

        public Building(BuildingType type)
        {
            this.type = type;
        }

        public void upgrade()
        {
            level++;
        }

        /// <summary>
        /// </summary>
        /// <returns>Price for current level</returns>
        public Resources.Resources getPrice()
        {
            return getPriceFor(level);
        }

        /// <summary>
        /// </summary>
        /// <returns>Price for next level</returns>
        public Resources.Resources getPriceForNextLevel()
        {
            return getPriceFor(level + 1);
        }

        public LocalTime getConstructionTime()
        {
            return getConstructionTimeFor(level);
        }

        public LocalTime getConstructionTimeForNextLevel()
        {
            return getConstructionTimeFor(level+1);
        }
    }
}
