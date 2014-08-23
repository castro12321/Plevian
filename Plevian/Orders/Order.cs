﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Maps;
using Plevian.Units;
using Plevian.Debugging;
namespace Plevian.Orders
{
    public abstract class Order
    {
        public Army army { get; protected set; }
        /// <summary>
        /// After setting it to true order will be deleted from orders list in village
        /// </summary>
        public bool completed { get; protected set; }
        public bool isGoingBack { get; protected set; }
        public OrderType type { get; protected set; }

        protected Tile origin;
        protected Tile destination;
        protected Seconds duration;
        protected GameTime endTime;
        
        
        protected float timePerTile;


        public Order(Tile origin, Tile destination, Army army, OrderType type)
        {
            this.origin = origin;
            this.destination = destination;
            this.army = army;
            this.timePerTile = army.getMovementSpeed() / 10;
            this.completed = false;
            this.isGoingBack = false;
            this.type = type;

            float distance = origin.location.distance(destination.location);
            duration = new Seconds((int)(timePerTile * distance));
            endTime = GameTime.now + duration;

        }

        public override string ToString()
        {
            return "Arrival : " + endTime + ", duration : " + duration;
        }

        public void tick()
        {
            Logger.s(this.ToString());
            if (!completed)
                duration.seconds--;
            if (duration.seconds <= 0)
                onEnd();
            
        }

        public void turnBack()
        {
            isGoingBack = true;

            float distance = origin.location.distance(destination.location);
            Seconds newDuration = new Seconds((int)(timePerTile * distance));
            newDuration.seconds -= duration.seconds;
            duration = newDuration;

            endTime = GameTime.now + duration;

            Tile temp = destination;
            destination = origin;
            origin = destination;
        }


        protected abstract void onEnd();


    }
}

