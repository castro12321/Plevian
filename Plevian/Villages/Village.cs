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
using Plevian.TechnologY;

namespace Plevian.Villages
{
    public class Queues
    {
        /// <summary>
        /// Base QueueItem class for other queue items (BuildingQueueItem, RecruitQueueItem, TechnologyQueueItem)
        /// </summary>
        public class QueueItem
        {
            public readonly String Name;
            public readonly String Extra;
            public readonly GameTime Start;
            public readonly GameTime End;

            public QueueItem(String Name, String Extra, GameTime Start, GameTime End)
            {
                this.Name = Name;
                this.Extra = Extra;
                this.Start = Start;
                this.End = End;
            }
        }

        private Village owner;

        public Queues(Village owner)
        {
            this.owner = owner;
        }

        /// <summary>Called when when new item has been added to the queue</summary>
        /// <param name="village">May be useful for WPF</param>
        /// <param name="item">Item added to the queue</param>
        public delegate void QueueItemAdded(Village village, QueueItem item);
        /// <summary>Called when the time passed for the queue item</summary>
        public delegate void QueueItemFinished(Village village, QueueItem item);
        /// <summary>Called when an item is being removed before the time pass. For example, user canceled the task</summary>
        public delegate void QueueItemRemoved(Village village, QueueItem item);

        // Events that village and WPF may listen to.
        public event QueueItemAdded queueItemAdded;
        /// <summary>
        /// Event for the village to listen. (finishBuilding, finishRecruiting and finishResearching merges into FinishQueueItem(Village village, QueueItem item))
        /// When an item is finished, village checks which (BuildingQueueItem, RecruitQueueItem, TechnologyQueueItem)
        /// and then takes an appropriate action
        /// For example:
        /// BuildingQueueItem buildingItem = queueItem as BuildingQueueItem;
        /// if(buildingItem != null)
        /// { ... }
        /// then check for RecruitQueueItem and TechnologyQueueItem like above
        /// </summary>
        public event QueueItemFinished queueItemFinished;
        public event QueueItemRemoved queueItemRemoved;

        public ObservableCollection<QueueItem> queue = new ObservableCollection<QueueItem>();
        public ObservableCollection<BuildingQueueItem> buildingQueue = new ObservableCollection<BuildingQueueItem>();
        public ObservableCollection<RecruitQueueItem> recruitQueue = new ObservableCollection<RecruitQueueItem>();
        public ObservableCollection<ResearchQueueItem> researchQueue = new ObservableCollection<ResearchQueueItem>();

        public void Add(QueueItem item)
        {
            queue.Add(item);
            if (item is BuildingQueueItem) buildingQueue.Add(item as BuildingQueueItem);
            else if (item is RecruitQueueItem) recruitQueue.Add(item as RecruitQueueItem);
            else if (item is ResearchQueueItem) researchQueue.Add(item as ResearchQueueItem);
            
            sort();
            if (queueItemAdded != null)
                queueItemAdded(owner, item);
        }

        public void Remove(QueueItem item)
        {
            queue.Remove(item);
            if (item is BuildingQueueItem) buildingQueue.Remove(item as BuildingQueueItem);
            else if (item is RecruitQueueItem) recruitQueue.Remove(item as RecruitQueueItem);
            else if (item is ResearchQueueItem) researchQueue.Remove(item as ResearchQueueItem);

            sort();
            if (queueItemRemoved != null)
                queueItemRemoved(owner, item);
        }

        /// <summary>
        /// Called by the village every tick
        /// </summary>
        public void CompleteAvailableItems()
        {
            while(queue.Count > 0)
            {
                QueueItem item = queue[0];
                if (GameTime.now < item.End)
                    return;

                queue.RemoveAt(0);
                if (item is BuildingQueueItem) buildingQueue.RemoveAt(0);
                else if (item is RecruitQueueItem) recruitQueue.RemoveAt(0);
                else if (item is ResearchQueueItem) researchQueue.RemoveAt(0);

                if (queueItemFinished != null)
                    queueItemFinished(owner, item);
            }
        }

        public bool isResearching(Technology technology)
        {
            foreach (ResearchQueueItem item in researchQueue)
                if (item.researched == technology)
                    return true;
            return false;
        }

        private void sort()
        {
            var ordered = queue.OrderBy(item => item.End.time).ToList();
            queue.Clear();
            foreach (QueueItem item in ordered)
                queue.Add(item);
            // Don't sort building/recruit/research queues on its own
            // They are sorted by default because they are finished in the same order they were placed
        }
    }

    public class Village : Tile, INotifyPropertyChanged
    {
        public Dictionary<BuildingType, Building> buildings = Building.getEmptyBuildingsList();
        public ObservableCollection<Order> orders = new ObservableCollection<Order>();

        public Queues queues { get; private set; }
        public GameTime recruitTimeEnd { get; private set; }
        public GameTime buildTimeEnd { get; private set; }
        public GameTime researchTimeEnd { get; private set; }
        public Army army { get; private set; }
        public readonly Resources resources;
        private string _name;
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
            this.queues = new Queues(this);
            this.Owner = owner;
            this.resources = new Resources(999, 999, 999, 999);
            this.recruitTimeEnd = GameTime.now;
            this.buildTimeEnd = GameTime.now;
            this.name = name;
            this.army = new Army();

            queues.queueItemFinished += queues_queueItemFinished;
        }

        void queues_queueItemFinished(Village village, Queues.QueueItem item)
        {
            if (this != village)
                throw new Exception("Shouldn't happen but to be sure");

            BuildingQueueItem buildingQueueItem = item as BuildingQueueItem;
            if(buildingQueueItem != null)
            {
                buildingQueueItem.toBuild.upgrade();
            }

            RecruitQueueItem recruitQueueITem = item as RecruitQueueItem;
            if(recruitQueueITem != null)
            {
                army.add(recruitQueueITem.toRecruit);
            }

            ResearchQueueItem researchQueueItem = item as ResearchQueueItem;
            if(researchQueueItem != null)
            {
                owner.technologies.discover(researchQueueItem.researched);
            }
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
            queues.CompleteAvailableItems();
        }

        private void OrdersTick()
        {
            for(int i = 0; i < orders.Count; ++i)
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

        private GameTime lastCheck = GameTime.now;
        private void collectProduction()
        {
            GameTime diff = GameTime.now.diffrence(lastCheck);
            lastCheck = GameTime.now;

            foreach (KeyValuePair<BuildingType, Building> building in buildings)
            {
                //Logger.village(building.Value.getDisplayName() + " produces " + building.Value.getProduction());
                addResources(building.Value.getProduction() * diff.time * 3); // TODO: Remove the "* 3" modifier
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

            if (queues.buildingQueue.Count == 0)
                buildTimeEnd = GameTime.now;

            Resources neededResources = getPriceForNextLevel(building);
            if (!resources.canAfford(neededResources))
                throw new Exceptions.ExceptionNotEnoughResources();

            takeResources(neededResources);

            GameTime buildTime = building.getConstructionTimeForNextLevel();
            foreach (Building b in buildings.Values)
                buildTime *= b.getBuildingTimeModifierFor(buildingType);

            GameTime startTime = buildTimeEnd.copy();
            buildTimeEnd += buildTime;

            Building toBuild = buildings[buildingType];
            int level = toBuild.level + 1;
            foreach(BuildingQueueItem build in queues.buildingQueue)
            {
                if (build.toBuild.type == toBuild.type)
                    level++;
            }
            BuildingQueueItem item = new BuildingQueueItem(startTime, buildTimeEnd.copy(), toBuild, level);
            queues.Add(item);
        }

        /// <summary>
        /// Recruit units in city
        /// </summary>
        public void recruit(Unit unit)
        {
            if (!canRecruit(unit))
                throw new Exception("Cannot recruit " + unit);

            // Reset recruit counter if needed
            if (queues.recruitQueue.Count == 0)
                recruitTimeEnd = GameTime.now;

            // Take money
            Resources neededResources = unit.getWholeUnitCost();
            if (!resources.canAfford(neededResources))
                throw new Exceptions.ExceptionNotEnoughResources();
            takeResources(neededResources);

            GameTime startTime = recruitTimeEnd.copy();

            Unit newUnit = unit.clone();
            newUnit.quantity = 1;

            float recruitTimeFromNow = 0;
            float unitRecruitTime = unit.recruitTime / 10; // TODO: Remove recruit time modifier ("/ 10")
            foreach (Building b in buildings.Values)
                unitRecruitTime *= b.getUnitTimeModifierFor(unit.unitType);
            int unitsToRecruit = unit.quantity;
            while(unitsToRecruit --> 0)
            {
                recruitTimeFromNow += unitRecruitTime;
                RecruitQueueItem queueItem = new RecruitQueueItem(startTime, recruitTimeEnd + new GameTime((int)recruitTimeFromNow), newUnit);
                queues.Add(queueItem);
            }
            recruitTimeEnd += new GameTime((int)recruitTimeFromNow);
        }

        public void research(Technology technology)
        {
            if (!technology.Requirements.isFullfilled(this))
                throw new Exception("Requirements not met for " + technology);

            // Reset recruit counter if needed
            if (queues.researchQueue.Count == 0)
                researchTimeEnd = GameTime.now;

            // Take money
            Resources neededResources = technology.Price;
            if (!resources.canAfford(neededResources))
                throw new Exceptions.ExceptionNotEnoughResources();
            takeResources(neededResources);

            GameTime startTime = researchTimeEnd.copy();
            researchTimeEnd += technology.ResearchTime;

            ResearchQueueItem queueItem = new ResearchQueueItem(startTime, researchTimeEnd, technology);
            queues.Add(queueItem);
        }

        public void addOrder(Order order)
        {
            if (army.canRemove(order.army))
            {
                orders.Add(order);
                army.remove(order.army);
            }
            else
            {
                throw new Exception("Army not big enough!");
            }
        }

        public void addArmy(Army army)
        {
            this.army.add(army);
        }

        public void takeArmy(Army army)
        {
            this.army.add(army);
        }

        public void addUnit(Unit unit)
        {
            if (army.contains(unit.unitType))
                army.get(unit.unitType).quantity += unit.quantity;
            else
                army.add(unit);

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
            bool hasMaxLevel = (level >= buildings[type].getMaxLevel());
            if (hasMaxLevel)
                return false;
            bool requirementsFullfiled = buildings[type].requirements.isFullfilled(this);
            if (!requirementsFullfiled)
                return false;
            bool hasResources = resources.canAfford(getPriceForNextLevel(type));
            if(!hasResources)
                return false;
            return true;
        }

        public bool canRecruit(Unit unit)
        {
            if (unit.quantity == 0)
                return false;
            if (!unit.requirements.isFullfilled(this))
                return false;
            if (!resources.canAfford(unit.getWholeUnitCost()))
                return false;
            return true;
        }

        public bool canResearch(Technology technology)
        {
            return !owner.technologies.isDiscovered(technology)
                && !queues.isResearching(technology)
                && technology.Requirements.isFullfilled(this)
                && resources.canAfford(technology.Price);
        }

        public int getBuildingLevel(BuildingType type, bool includeQueue = false)
        {
            int level = buildings[type].level;
            if(includeQueue)
                foreach (BuildingQueueItem queue in queues.buildingQueue)
                {
                    if (buildings[type].type == queue.toBuild.type)
                        level++;
                }
            return level;
        }

        protected Resources getPriceForNextLevel(Building building)
        {
            return building.getPriceFor(getBuildingLevel(building.type, true) + 1);
        }

        public Resources getPriceForNextLevel(BuildingType building)
        {
            return getPriceForNextLevel(buildings[building]);
        }

       
    }
}
