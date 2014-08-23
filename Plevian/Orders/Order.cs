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
        
        

        private OrderType _type;
        protected Tile origin;
        protected Tile _destination;
        protected Seconds duration;
        protected GameTime endTime;
        private bool _completed;
        private bool _isGoingBack;
        protected float timePerTile;


        public Order(Tile origin, Tile destination, Army army, OrderType type)
        {
            this.origin = origin;
            Destination = destination;
            this.army = army;
            this.timePerTile = army.getMovementSpeed();
            this.completed = false;
            this.isGoingBack = false;
            this.Type = type;

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
            {
                Duration.seconds--;
                NotifyPropertyChanged("Duration");
            }
            if (Duration.seconds <= 0)
                onEnd();
            
            
        }

        public void turnBack()
        {
            isGoingBack = true;

            Seconds newDuration = OverallTime.copy() as Seconds;
            newDuration.seconds -= duration.seconds;
            duration = newDuration;

            endTime = GameTime.now + duration;

            Tile temp = Destination;
            Destination = origin;
            origin = temp;
        }

        public virtual String getTooltipText()
        {
            int sum = 0;
            string tooltip = "";
            foreach (var pair in army.getUnits())
            {
                string unitName = Enum.GetName(typeof(UnitType), pair.Key);
                tooltip += "\n" + pair.Value.ToString();
                sum += pair.Value.quanity;
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

        protected abstract void onEnd();

        public Seconds Duration
        {
            private set
            {
                duration = value;
                NotifyPropertyChanged();
            }

            get
            {
                return duration;
            }

        }

        public Tile Destination
        {
            private set
            {
                _destination = value;
                NotifyPropertyChanged();
            }
            get
            {
                return _destination;
            }
        }

        public OrderType Type 
        {
            get
            {
                return _type;
            }
            protected set
            {
                _type = value;
                NotifyPropertyChanged();
            }
        }


        private Seconds _OverallTime;
        public Seconds OverallTime
        {
            get
            {
                if(_OverallTime == null)
                {
                    float distance = origin.location.distance(Destination.location);
                    _OverallTime = new Seconds((int)(timePerTile * distance));
                }
                return _OverallTime;
            }
        }

        public bool completed
        {
            get
            {
                return _completed;
            }
                protected set
            {
                _completed = value;
                NotifyPropertyChanged();
            }
        }
        public bool isGoingBack 
        {
            get
            {
                return _isGoingBack;
            }
            protected set
            {
                _isGoingBack = value;
                NotifyPropertyChanged();
            }
        }


    }
}

