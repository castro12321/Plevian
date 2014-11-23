using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Maps;
using Plevian.Units;
using Plevian.Debugging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plevian.Villages;
namespace Plevian.Orders
{
    public abstract class Order : INotifyPropertyChanged
    {
        public Village owner { get; private set; }
        public OrderType Type { get; private set; }
        public Army army { get; private set; }
        /// <summary>After setting it to true order will be deleted from orders list in village</summary>
        public bool completed { get; protected set; }
        public bool isGoingBack { get; protected set; }
        public GameTime OverallTime { get; private set; }
        public Tile origin { get; set; }
        public Tile Destination { get; private set; }
        protected GameTime endTime { get; set; }

        protected abstract void onEnd();

        public GameTime Duration
        {
            get
            {
                return GameTime.now.diffrence(endTime);
            }
        }

        public Order(Village owner, Tile origin, Tile destination, Army army, OrderType type)
        {
            if (owner == null)
                throw new ArgumentException("Owner is null");
            if (origin == null)
                throw new ArgumentException("origin is null");
            if (destination == null)
                throw new ArgumentException("destination is null");
            if (army == null)
                throw new ArgumentException("army is null");

            this.owner = owner;
            this.origin = origin;
            Destination = destination;
            this.army = army;
            this.completed = false;
            this.isGoingBack = false;
            this.Type = type;

            float distance = origin.location.distance(destination.location);
            OverallTime = (int)(army.getMovementSpeed() * distance);
            endTime = GameTime.now + OverallTime;
        }

        public override string ToString()
        {
            return "Arrival : " + endTime + ", duration : " + Duration;
        }

        public void tick()
        {
            Logger.s(this.ToString());
            if (!completed)
                NotifyPropertyChanged("Duration");

            if(GameTime.now >= endTime)
                onEnd();
        }

        public void turnBack()
        {
            isGoingBack = true;

            Tile temp = Destination;
            Destination = origin;
            origin = temp;

            float distance = origin.location.distance(Destination.location);
            OverallTime = (int)(army.getMovementSpeed() * distance);
            endTime = GameTime.now + OverallTime;

            NotifyPropertyChanged("Type");
            NotifyPropertyChanged("isGoingBack");
        }

        public virtual String getTooltipText()
        {
            return army.toString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

