using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;

namespace Plevian.Buildings
{
    public abstract class Building
    {
        public readonly BuildingType type;
        public int level { get; private set; }

        public abstract String getDisplayName();
        public abstract LocalTime getConstructionTimeFor(int level);
        public abstract Resources getPriceFor(int level);
        public abstract Resources getProduction();
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
        public Resources getPrice()
        {
            return getPriceFor(level);
        }

        /// <summary>
        /// </summary>
        /// <returns>Price for next level</returns>
        public Resources getPriceForNextLevel()
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

        public bool isBuilt()
        {
            return level > 0;
        }

        public static Dictionary<BuildingType, Building> getEmptyBuildingsList()
        {
            Dictionary<BuildingType, Building> buildings = new Dictionary<BuildingType, Building>();
            buildings.Add(BuildingType.TOWN_HALL, new TownHall());
            return buildings;
        }
    }
}
