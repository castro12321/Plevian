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

namespace Plevian.Villages
{
    public class Village : Tile
    {
        private Dictionary<BuildingType, Building> buildings = Building.getEmptyBuildingsList();
        public ObservableCollection<Order> orders = new ObservableCollection<Order>();
        public Queue<BuildingQueueItem> buildingsQueue = new Queue<BuildingQueueItem>();
        public Queue<RecruitQueueItem> recruitQueue = new Queue<RecruitQueueItem>();
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
            //Logger.village("queue: " + buildingsQueue.Count);
            while(buildingsQueue.Count > 0)
            {
                BuildingQueueItem queueItem = buildingsQueue.Peek();
                //Logger.village("item: " + queueItem.toBuild.ToString() + " " + GameTime.now + "/" + queueItem.end);
                if (GameTime.now < queueItem.end)
                    break;

                //Logger.village("Built! " + queueItem.toBuild.ToString());
                buildings[queueItem.toBuild].upgrade();
                buildingsQueue.Dequeue();
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
            if (unit.quanity == 0)
                throw new Exception("Cannot recruit 0 units");

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
        public override string ToString()
        {
            return name;
        }
    }
}
