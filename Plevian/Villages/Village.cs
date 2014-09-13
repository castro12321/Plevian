﻿using System;
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
            public readonly GameTime Start;
            public readonly GameTime End;

            public QueueItem(GameTime Start, GameTime End)
            {
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

        public void Add(QueueItem item)
        {
            queue.Add(item);
            sort();
            if (queueItemAdded != null)
                queueItemAdded(owner, item);
        }

        public void Remove(QueueItem item)
        {
            queue.Remove(item);
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
                if (queueItemFinished != null)
                    queueItemFinished(owner, item);
            }
        }

        private void sort()
        {
            queue.GroupBy(item => item.End);
        }
    }

    public class Village : Tile, INotifyPropertyChanged
    {
        public Dictionary<BuildingType, Building> buildings = Building.getEmptyBuildingsList();
        public ObservableCollection<Order> orders = new ObservableCollection<Order>();

        public Queues queues { get; private set; }
/*
        public ObservableCollection<BuildingQueueItem> buildingsQueue = new ObservableCollection<BuildingQueueItem>();
        public ObservableCollection<RecruitQueueItem> recruitQueue = new ObservableCollection<RecruitQueueItem>();
        public ObservableCollection<ResearchQueueItem> researchQueue = new ObservableCollection<ResearchQueueItem>();
*/
        public GameTime recruitTimeEnd { get; private set; }
        public GameTime buildTimeEnd { get; private set; }
        public GameTime researchTimeEnd { get; private set; }
        public Army army { get; private set; }
        public readonly Resources resources;
        private string _name;
        private Player owner;
/*
        public event BuildingQueueItemAdded buildingQueueItemAdded;
        public event BuildingBuilt buildingBuilt;
        public event TechnologyQueueItemAdded technologyQueueItemAdded;
        public event TechnologyResearched technologyResearched;
*/
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
            this.resources = new Resources(99999, 99999, 99999, 99999);
            this.recruitTimeEnd = GameTime.now;
            this.buildTimeEnd = GameTime.now;
            this.name = name;
            this.army = new Army();
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
            finishResearching();
            if(name == "Capital")
                Logger.log(name + " army " + army);
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
                RecruitQueueItem queueItem = recruitQueue[0];
                if (GameTime.now < queueItem.end)
                    break;

                Unit toRecruit = queueItem.toRecruit;
                if (army.contains(toRecruit.unitType))
                    army.get(toRecruit.unitType).quantity++;
                else
                {
                    Unit clone = toRecruit.clone();
                    clone.quantity = 1;
                    army.add(clone);
                }
                recruitQueue.RemoveAt(0);
            }
        }

        private void finishResearching()
        {
            while (researchQueue.Count > 0)
            {
                ResearchQueueItem queueItem = researchQueue[0];
                if (GameTime.now < queueItem.end)
                    break;

                owner.technologies.discover(queueItem.researched);
                researchQueue.RemoveAt(0);
                if (technologyResearched != null)
                    technologyResearched(this, queueItem.researched);
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
            if (unit.quantity == 0)
                throw new Exception("Cannot recruit 0 units");
            if (!unit.requirements.isFullfilled(this))
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
            newUnit.quantity = 1;

            float recruitTimeFromNow = 0;
            float unitRecruitTime = unit.recruitTime;
            foreach (Building b in buildings.Values)
                unitRecruitTime *= b.getUnitTimeModifierFor(unit.unitType);
            int unitsToRecruit = unit.quantity;
            while(unitsToRecruit --> 0)
            {
                recruitTimeFromNow += unitRecruitTime;
                RecruitQueueItem queueItem = new RecruitQueueItem(startTime, recruitTimeEnd + new Seconds((int) recruitTimeFromNow), newUnit);
                recruitQueue.Add(queueItem);
            }
            recruitTimeEnd += new Seconds((int)recruitTimeFromNow);
        }

        public void research(Technology technology)
        {
            if (!technology.Requirements.isFullfilled(this))
                throw new Exception("Requirements not met for " + technology);

            // Reset recruit counter if needed
            if (researchQueue.Count == 0)
                researchTimeEnd = GameTime.now;

            // Take money
            Resources neededResources = technology.Price;
            if (!resources.canAfford(neededResources))
                throw new Exceptions.ExceptionNotEnoughResources();
            takeResources(neededResources);

            GameTime startTime = researchTimeEnd.copy();
            researchTimeEnd += technology.ResearchTime;

            ResearchQueueItem queueItem = new ResearchQueueItem(startTime, researchTimeEnd, technology);
            researchQueue.Add(queueItem);

            if (technologyQueueItemAdded != null)
                technologyQueueItemAdded(this, queueItem);
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
            bool requirementsFullfiled = buildings[type].requirements.isFullfilled(this);
            bool hasResources = false;
            if(hasMaxLevel == false)
                hasResources = resources.canAfford(getPriceForNextLevel(type));
            if (hasMaxLevel == false && requirementsFullfiled && hasResources)
                return true;
            return false;
        }

        public bool canResearch(Technology technology)
        {
            return technology.Requirements.isFullfilled(this)
                && resources.canAfford(technology.Price);
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
