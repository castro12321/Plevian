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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plevian.events;

namespace Plevian.Villages
{
    public class Village : Tile, INotifyPropertyChanged
    {
        public Dictionary<BuildingType, Building> buildings = Building.getEmptyBuildingsList();
        public ObservableCollection<Order> orders = new ObservableCollection<Order>();
        public ObservableCollection<BuildingQueueItem> buildingsQueue = new ObservableCollection<BuildingQueueItem>();
        public Queue<RecruitQueueItem> recruitQueue = new Queue<RecruitQueueItem>();
        public GameTime recruitTimeEnd { get; private set; }
        public GameTime buildTimeEnd { get; private set; }
        public Army army { get; private set; }
        public readonly Resources resources;
        private string _name;
        private Player owner;

        public event BuildingQueueItemAdded buildingQueueItemAdded;
        public event BuildingBuilt buildingBuilt;

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
            resources = new Resources(9999, 9999, 9999, 9999);
            recruitTimeEnd = GameTime.now;
            buildTimeEnd = GameTime.now;
            this.name = name;
            army = new Army();
        }

        public void setBuildings(Dictionary<BuildingType, Building> buildings)
        {
            this.buildings = buildings;
        }

        /// <summary>
        /// Returns building of desired type. Returns building on success, otherwise null
        /// </summary>
        /// <param name="type">building type to retrieve</param>
        /// <returns>Building on success, otherwise null</returns>
        public Building getBuilding(BuildingType type)
        {
            if (!buildings.ContainsKey(type))
                return null;
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
            //Logger.village("queue: " + buildingsQueue.Count);
            while(buildingsQueue.Count > 0)
            {
                BuildingQueueItem queueItem = buildingsQueue[0];
                //Logger.village("item: " + queueItem.toBuild.ToString() + " " + GameTime.now + "/" + queueItem.end);
                if (GameTime.now < queueItem.end)
                    break;

                //Logger.village("Built! " + queueItem.toBuild.ToString());
                queueItem.toBuild.upgrade();
                buildingsQueue.RemoveAt(0);
                if (buildingBuilt != null)
                    buildingBuilt(this, queueItem.toBuild);
            }
        }

        private void finishRecruiting()
        {
            while (recruitQueue.Count > 0)
            {
                RecruitQueueItem queueItem = recruitQueue.Peek();
                if (GameTime.now < queueItem.end)
                    break;

                Unit toRecruit = queueItem.toRecruit;
                if (army.contain(toRecruit.getUnitType()))
                    army.get(toRecruit.getUnitType()).quanity++;
                else
                {
                    Unit clone = toRecruit.clone();
                    clone.quanity = 1;
                    army += clone;
                }
                recruitQueue.Dequeue();
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
            if (!building.requirements.isFullfilled(this))
                throw new Exception("Requirements not met for " + building);

            if (buildingsQueue.Count == 0)
                buildTimeEnd = GameTime.now;

            Resources neededResources = building.getPriceForNextLevel();
            if (!resources.canAfford(neededResources))
                throw new Exceptions.ExceptionNotEnoughResources();

            takeResources(neededResources);

            GameTime buildTime = building.getConstructionTimeForNextLevel();

            GameTime startTime = buildTimeEnd.copy();
            buildTimeEnd += buildTime;

            Building toBuild = buildings[buildingType];
            int level = toBuild.level + 1;
            foreach(var build in buildingsQueue)
            {
                if (build.toBuild.type == toBuild.type)
                    level++;
            }
            BuildingQueueItem item = new BuildingQueueItem(startTime, buildTimeEnd.copy(), toBuild, level);
            buildingsQueue.Add(item);

            if (buildingQueueItemAdded != null)
                buildingQueueItemAdded(this, item);
        }

        /// <summary>
        /// Recruit units in city
        /// </summary>
        public void recruit(Unit unit)
        {
            if (unit.quanity == 0)
                throw new Exception("Cannot recruit 0 units");
            if (!unit.getRequirements().isFullfilled(this))
                throw new Exception("Requirements not met for " + unit);

            // Reset recruit counter if needed
            if (recruitQueue.Count == 0)
                recruitTimeEnd = GameTime.now;

            // Take money
            Resources neededResources = unit.getWholeUnitCost();
            if (!resources.canAfford(neededResources))
                throw new Exceptions.ExceptionNotEnoughResources();
            takeResources(neededResources);

            GameTime startTime = recruitTimeEnd.copy();

            Unit newUnit = unit.clone();
            newUnit.quanity = 1;

            float recruitTimeFromNow = 0;
            int unitsToRecruit = unit.quanity;
            while(unitsToRecruit --> 0)
            {
                recruitTimeFromNow += unit.getRecruitTime();
                recruitQueue.Enqueue(new RecruitQueueItem(startTime, recruitTimeEnd + new Seconds((int)recruitTimeFromNow), newUnit));
            }
            recruitTimeEnd += new Seconds((int)recruitTimeFromNow);
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

        public int getBaseDefense()
        {
            Building building = getBuilding(BuildingType.WALL);
            if(building != null)
            {
                Wall wall = building as Wall;
                return wall.getBaseDefense();
            }
            return 0;
        }

        public float getDefense()
        {
            Building building = getBuilding(BuildingType.WALL);
            if (building != null)
            {
                Wall wall = building as Wall;
                return wall.getDefense();
            }
            return 1f;
        }

        public override string ToString()
        {
            return name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string name
        { 
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public bool canBuild(BuildingType type)
        {
            int level = getBuildingLevel(type, true);
            if (level >= buildings[type].getMaxLevel() || !buildings[type].requirements.isFullfilled(this))
                return false;
            return true;
        }

        public int getBuildingLevel(BuildingType type, bool includeQueue = false)
        {
            int level = buildings[type].level;
            if(includeQueue)
                foreach (var queue in buildingsQueue)
                {
                    if (buildings[type].type == queue.toBuild.type)
                        level++;
                }
            return level;
        }
    }
}
