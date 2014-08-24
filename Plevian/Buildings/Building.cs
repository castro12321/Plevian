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
        public abstract GameTime getConstructionTimeFor(int level);
        public abstract Resources getPriceFor(int level);
        public abstract Resources getProductionFor(int level);
        public abstract int getMaxLevel();

        public Building(BuildingType type)
        {
            this.type = type;
            this.level = 0;
        }

        public void upgrade()
        {
            level++;
        }

        public Resources getProduction()
        {
            return getProductionFor(level);
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

        public GameTime getConstructionTime()
        {
            return getConstructionTimeFor(level);
        }

        public GameTime getConstructionTimeForNextLevel()
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
            buildings.Add(BuildingType.BARRACKS, new Barracks());
            buildings.Add(BuildingType.STABLE, new Stable());
            buildings.Add(BuildingType.TOWN_HALL, new TownHall());
            buildings.Add(BuildingType.FARM, new Farm());
            buildings.Add(BuildingType.LUMBER_MILL, new LumberMill());
            buildings.Add(BuildingType.MINE, new Mine());
            return buildings;
        }
    }
}
