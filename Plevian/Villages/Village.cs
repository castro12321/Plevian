using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Buildings;
using Plevian.Resource;

namespace Plevian.Villages
{
    class Village
    {
        private Dictionary<BuildingType, Building> buildings = Building.getEmptyBuildingsList();
        private Dictionary<Units.UnitType, int> units = new Dictionary<Units.UnitType, int>();
        public Resources resources { get; private set; }

        public Village()
        {
            resources = new Resources(999, 999, 999, 999);
        }

        public void setBuildings(Dictionary<BuildingType, Building> buildings)
        {
            this.buildings = buildings;
        }

        public void addResources(Resources add)
        {
            resources = resources + add;
        }

        public void takeResources(Resources take)
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
            foreach (KeyValuePair<BuildingType, Building> building in buildings)
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

        public bool isBuilt(BuildingType type)
        {
            return buildings[type].isBuilt();
        }

        /// <summary>
        /// Builds (or upgrades) building in the village
        /// </summary>
        /// <param name="buildingType"></param>
        public void build(BuildingType buildingType)
        {
            Building building = buildings[buildingType];

            Resources neededResources = building.getPriceForNextLevel();
            if (!resources.canAfford(neededResources))
                throw new Exceptions.ExceptionNotEnoughResources();

            LocalTime buildTime = building.getConstructionTimeForNextLevel();
            LocalTime finishTime = GameTime.add(buildTime);
        }
    }
}
