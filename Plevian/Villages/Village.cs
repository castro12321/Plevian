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
    public class Village
    {
        private Dictionary<BuildingType, Building> buildings = Building.getEmptyBuildingsList();
        private Dictionary<Units.UnitType, int> units = new Dictionary<Units.UnitType, int>();
        private Queue<BuildingQueueItem> buildingsQueue = new Queue<BuildingQueueItem>();
        private List<RecruitQueueItem> recruitQueue = new List<RecruitQueueItem>();
        private GameTime recruitTimeEnd = GameTime.now;
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
                Logger.c("item: " + queueItem.toBuild.ToString() + " " + GameTime.now + "/" + queueItem.end);
                if (GameTime.now >= queueItem.end)
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
            if (recruitQueue.Count == 0) return;
            float second = 1f;
            RecruitQueueItem queue = recruitQueue[0];

            if (queue.timeCurrent > 1f)
            {
                queue.timeCurrent -= 1;
                queue.remainingQuanity -= 1;
            }
            else
            {
                while (second > 0f && recruitQueue.Count > 0)
                {
                    while (second >= queue.timeCurrent && queue.remainingQuanity > 0)
                    {
                        second -= queue.timeCurrent;
                        //add builded soldier
                        queue.timeCurrent = queue.recruitTime;
                        queue.remainingQuanity--;
                    }
                    if (queue.remainingQuanity > 0)
                    {
                        queue.timeCurrent -= second;
                        second = 0f;
                    }
                    else
                    {
                        recruitQueue.RemoveAt(0);
                        if (recruitQueue.Count == 0) return;
                        queue = recruitQueue[0];
                    }
                }
            }
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

            GameTime buildTime = building.getConstructionTimeForNextLevel();
            GameTime finishTime = GameTime.now + buildTime;
            buildingsQueue.Enqueue(new BuildingQueueItem(finishTime, buildingType));
        }

        /// <summary>
        /// Recruit units in city
        /// </summary>
        /// <param name="unitType">unit to recruit</param>
        /// <param name="quanity">Quanity of units to recruit</param>
        /// <param name="recruitTime">Recruit time for invidual unit</param>
        public void recruit(Unit unit)
        {
            if (recruitQueue.Count == 0) recruitTimeEnd = GameTime.now;
            RecruitQueueItem newQueue = new RecruitQueueItem(unit);
            recruitTimeEnd += newQueue.end;
            recruitQueue.Add(newQueue);
        }
    }
}
