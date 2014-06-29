using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Buildings;
using Plevian.Resource;
using Plevian.Debugging;
using Plevian.Units;

namespace Plevian.Villages
{
    class Village
    {
        private Dictionary<BuildingType, Building> buildings = Building.getEmptyBuildingsList();
        private Dictionary<Units.UnitType, int> units = new Dictionary<Units.UnitType, int>();
        private Queue<BuildingQueueItem> buildingsQueue = new Queue<BuildingQueueItem>();
        private List<RecruitQueueItem> recruitQueue = new List<RecruitQueueItem>();
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
            Logger.c("village " + resources);
        }

        private void collectProduction()
        {
            foreach (KeyValuePair<BuildingType, Building> building in buildings)
            {
                Logger.c(building.Value.getDisplayName() + " produces " + building.Value.getProduction());
                addResources(building.Value.getProduction());
            }
        }

        private void finishBuilding()
        {
            Logger.c("queue: " + buildingsQueue.Count);
            if(buildingsQueue.Count > 0)
            {
                BuildingQueueItem queueItem = buildingsQueue.Peek();
                Logger.c("item: " + queueItem.toBuild.ToString() + " " + GameTime.time + "/" + queueItem.end);
                if (GameTime.time >= queueItem.end)
                {
                    Logger.c("Built! " + queueItem.toBuild.ToString());
                    buildings[queueItem.toBuild].upgrade();
                    buildingsQueue.Dequeue();
                }
            }
            else
            {
                Logger.c("Queue empty! Building town hall");
                build(BuildingType.TOWN_HALL);
            }
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

            resources -= neededResources;

            LocalTime buildTime = building.getConstructionTimeForNextLevel();
            LocalTime finishTime = GameTime.add(buildTime);
            buildingsQueue.Enqueue(new BuildingQueueItem(finishTime, buildingType));
        }

        /// <summary>
        /// Recruit units in city
        /// </summary>
        /// <param name="unitType">Type of unit to recruit</param>
        /// <param name="quanity">Quanity of units to recruit</param>
        /// <param name="recruitTime">Recruit time for invidual unit</param>
        public void recruit(UnitType unitType, int quanity, float recruitTime)
        {
           
        }
    }
}
