using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Buildings;
using Plevian.Resource;
using Plevian.Debugging;
using Plevian.Units;
using Plevian.Maps;

namespace Plevian.Villages
{
    public class Village : Tile
    {
        private Dictionary<BuildingType, Building> buildings = Building.getEmptyBuildingsList();
        //private Dictionary<Units.UnitType, int> units = new Dictionary<Units.UnitType, int>();
        private Queue<BuildingQueueItem> buildingsQueue = new Queue<BuildingQueueItem>();
        private List<RecruitQueueItem> recruitQueue = new List<RecruitQueueItem>();
        public GameTime recruitTimeEnd { get; private set; }
        public GameTime buildTimeEnd { get; private set; }
        public Army army { get; private set; }
        public Resources resources { get; private set; }

        public Village(Location location)
            : base(location, TerrainType.VILLAGE)
        {
            resources = new Resources(999, 999, 999, 999);
            recruitTimeEnd = GameTime.now;
            buildTimeEnd = GameTime.now;
            army = new Army();
        }

        public void setBuildings(Dictionary<BuildingType, Building> buildings)
        {
            this.buildings = buildings;
        }

        public Building getBuilding(BuildingType type)
        {
            return buildings[type];
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
            Logger.village("village resources " + resources);
        }

        private void collectProduction()
        {
            foreach (KeyValuePair<BuildingType, Building> building in buildings)
            {
                Logger.village(building.Value.getDisplayName() + " produces " + building.Value.getProduction());
                addResources(building.Value.getProduction());
            }
        }

        private void finishBuilding()
        {
            Logger.village("queue: " + buildingsQueue.Count);
            if(buildingsQueue.Count > 0)
            {
                BuildingQueueItem queueItem = buildingsQueue.Peek();
                Logger.village("item: " + queueItem.toBuild.ToString() + " " + GameTime.now + "/" + queueItem.end);
                if (GameTime.now >= queueItem.end)
                {
                    Logger.village("Built! " + queueItem.toBuild.ToString());
                    buildings[queueItem.toBuild].upgrade();
                    buildingsQueue.Dequeue();
                }
            }
            else
            {
                Logger.village("Queue empty! Building town hall");
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

                        if (army.contain(queue.unit.getUnitType()))
                            army.get(queue.unit.getUnitType()).quanity++;
                        else
                        {
                            Unit clone = queue.unit.clone();
                            clone.quanity = 1;
                            army += clone;
                        }

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
            if (buildingsQueue.Count == 0)
                buildTimeEnd = GameTime.now;

            Resources neededResources = building.getPriceForNextLevel();
            if (!resources.canAfford(neededResources))
                throw new Exceptions.ExceptionNotEnoughResources();

            resources -= neededResources;

            GameTime buildTime = building.getConstructionTimeForNextLevel();
            buildTimeEnd += buildTime;
            buildingsQueue.Enqueue(new BuildingQueueItem(buildTimeEnd, buildingType));
        }

        /// <summary>
        /// Recruit units in city
        /// </summary>
        /// <param name="unitType">unit to recruit</param>
        /// <param name="quanity">Quanity of units to recruit</param>
        /// <param name="recruitTime">Recruit time for invidual unit</param>
        public void recruit(Unit unit)
        {
            Resources neededResources = unit.getWholeUnitCost();
            if (!resources.canAfford(neededResources))
                throw new Exception("Not enough resources");
            if (recruitQueue.Count == 0)
                recruitTimeEnd = GameTime.now;
            resources -= unit.getWholeUnitCost();
            RecruitQueueItem newQueue = new RecruitQueueItem(unit);
            recruitTimeEnd += newQueue.duration;
            recruitQueue.Add(newQueue);
        }
    }
}
