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
using Plevian.Players;
using Plevian.Orders;
using System.Collections.ObjectModel;

namespace Plevian.Villages
{
    public class Village : Tile
    {
        private Dictionary<BuildingType, Building> buildings = Building.getEmptyBuildingsList();
        public ObservableCollection<Order> orders = new ObservableCollection<Order>();
        public Queue<BuildingQueueItem> buildingsQueue = new Queue<BuildingQueueItem>();
        public List<RecruitQueueItem> recruitQueue = new List<RecruitQueueItem>();
        public GameTime recruitTimeEnd { get; private set; }
        public GameTime buildTimeEnd { get; private set; }
        public Army army { get; private set; }
        public readonly Resources resources;
        public string name { get; private set; }
        private Player owner;

        public Player Owner
        {
            get
            {
                return owner;
            }

            set
            {
                owner = value;
                // TODO: do sth?
            }
        }


        public Village(Location location, Player owner, string name )
            : base(location, TerrainType.VILLAGE)
        {
            Owner = owner;
            resources = new Resources(999, 999, 999, 999);
            recruitTimeEnd = GameTime.now;
            buildTimeEnd = GameTime.now;
            this.name = name;
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
            resources.Add(add);
        }

        public void takeResources(Resources take)
        {
            resources.Substract(take);
        }

        /// <summary>
        /// Village tick called every second
        /// </summary>
        public void tick()
        {
            collectProduction();
            OrdersTick();
            finishBuilding();
            finishRecruiting();
            //Logger.village("village resources " + resources);
        }
        private void OrdersTick()
        {
            for(int i = 0;i < orders.Count; ++i)
            {
                Order order = orders[i];
                if(order.completed)
                {
                    orders.RemoveAt(i);
                    continue;
                }

                order.tick();
            }
        }

        private void collectProduction()
        {
            foreach (KeyValuePair<BuildingType, Building> building in buildings)
            {
                //Logger.village(building.Value.getDisplayName() + " produces " + building.Value.getProduction());
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
        }

        private void finishRecruiting()
        {
            if (recruitQueue.Count == 0) return;
            // TODO: don't assume the tick is 1 second; use GameTime diff
            float second = 1f;
            RecruitQueueItem queue = recruitQueue[0];

            if (queue.timeCurrent > 1f)
            {
                queue.timeCurrent -= 1;
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

            takeResources(neededResources);

            GameTime buildTime = building.getConstructionTimeForNextLevel();

            GameTime startTime = buildTimeEnd.copy();
            buildTimeEnd += buildTime;
            buildingsQueue.Enqueue(new BuildingQueueItem(startTime, buildTimeEnd.copy(), buildingType));
        }

        /// <summary>
        /// Recruit units in city
        /// </summary>
        public void recruit(Unit unit)
        {
            Resources neededResources = unit.getWholeUnitCost();
            if (!resources.canAfford(neededResources))
                throw new Exception("Not enough resources");
            if (unit.quanity == 0)
                return;
            if (recruitQueue.Count == 0)
                recruitTimeEnd = GameTime.now;
            takeResources(unit.getWholeUnitCost());
            RecruitQueueItem newQueue = new RecruitQueueItem(unit, recruitTimeEnd.copy());
            recruitTimeEnd += newQueue.duration;
            recruitQueue.Add(newQueue);
        }

        public void addOrder(Order order)
        {
            if (army.canDivide(order.army))
            {
                orders.Add(order);
                army -= order.army;
            }
            else
            {
                throw new Exception("Army not big enough!");
            }
        }

        public void addArmy(Army army)
        {
            this.army += army;
        }

        public void takeArmy(Army army)
        {
            this.army -= army;
        }

        public void addUnit(Unit unit)
        {
            if (army.contain(unit.getUnitType()))
                army.get(unit.getUnitType()).quanity+= unit.quanity;
            else
            {
                army += unit;
            }

        }

        public void turnBackAllOrders()
        {
            foreach(Order order in orders)
            {
                if (order.isGoingBack == false)
                    order.turnBack();
            }
        }
        public override string ToString()
        {
            return name;
        }
    }
}
