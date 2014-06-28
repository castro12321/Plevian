using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Village
{
    class Village
    {
        private Dictionary<Buildings.BuildingType, Buildings.Building> buildings = Buildings.Building.getEmptyBuildingsList();
        private Dictionary<Units.UnitType, int> units = new Dictionary<Units.UnitType, int>();
        public Resources.Resources resources { get; private set; }
        
        public Village()
        {
        }

        public void setBuildings(Dictionary<Buildings.BuildingType, Buildings.Building> buildings)
        {
            this.buildings = buildings;
        }

        public void addResources(Resources.Resources add)
        {
            resources = resources + add;
        }

        public void takeResources(Resources.Resources take)
        {
            resources = resources - take;
        }

        /// <summary>
        /// Village tick called every second
        /// </summary>
        public void tick()
        {
            collectProduction();
            finishBuilding();
            finishRecruiting();
        }
        
        private void collectProduction()
        {
            foreach (KeyValuePair<Buildings.BuildingType, Buildings.Building> building in buildings)
                addResources(building.Value.getProduction());
        }

        private void finishBuilding()
        {
            // Check buildings queue
            // If something is done; yay
        }

        private void finishRecruiting()
        {
            // Check recruiting queue
            // If something is done; yay
        }

        public bool isBuilt(Buildings.BuildingType type)
        {
            return buildings[type].isBuilt();
        }

        /// <summary>
        /// Builds (or upgrades) building in the village
        /// </summary>
        /// <param name="buildingType"></param>
        public void build(Buildings.BuildingType buildingType)
        {
            Buildings.Building building = buildings[buildingType];

            Resources.Resources neededResources = building.getPriceForNextLevel();
            if (!resources.canAfford(neededResources))
                throw new Exceptions.NotEnoughResourcesException();

            LocalTime buildTime = building.getConstructionTimeForNextLevel();
            LocalTime finishTime = GameTime.add(buildTime);
        }
    }
}
