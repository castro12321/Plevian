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
        public Army army { get; protected set; }
        /// <summary>
        /// After setting it to true order will be deleted from orders list in village
        /// </summary>
        public Tile Destination { get; private set; }
        public OrderType Type { get; protected set; }
        public bool completed { get; protected set; }
        public bool isGoingBack { get; protected set; }
        public GameTime OverallTime { get; private set; }
        protected Tile origin { get; set; }
        protected GameTime endTime { get; set; }

        protected abstract void onEnd();

        public GameTime Duration
        {
            get
            {
                return GameTime.now.diffrence(endTime);
            }
        }

        public Order(Tile origin, Tile destination, Army army, OrderType type)
        {
            if (origin == null)
                throw new ArgumentException("origin is null");
            if (destination == null)
                throw new ArgumentException("destination is null");
            if (army == null)
                throw new ArgumentException("army is null");

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
        }

        public virtual String getTooltipText()
        {
            int sum = 0;
            string tooltip = "";
            foreach (var pair in army.getUnits())
            {
                string unitName = Enum.GetName(typeof(UnitType), pair.Key);
                tooltip += "\n" + pair.Value.ToString();
                sum += pair.Value.quantity;
            }

            tooltip = "Units : " + sum + tooltip;
            return tooltip;
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

