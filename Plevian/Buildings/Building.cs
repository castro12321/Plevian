using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.RequirementS;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plevian.Units;

namespace Plevian.Buildings
{
    public abstract class Building : INotifyPropertyChanged
    {
        public readonly BuildingType type;
        private int _level;

        public int level 
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                NotifyPropertyChanged();
            }
        }

        public abstract String getDisplayName();
        public abstract GameTime getConstructionTimeFor(int level);
        public abstract Resources getPriceFor(int level);
        public abstract Resources getProductionFor(int level);
        public abstract int getMaxLevel();
        public virtual float getBuildingTimeModifierFor(BuildingType type)
        {
            return 1;
        }
        public virtual float getUnitTimeModifierFor(UnitType type)
        {
            return 1;
        }

        public abstract Requirements requirements { get; }

        public Building(BuildingType type)
        {
            this.type = type;
            this.level = 0;
        }

        public void upgrade()
        {
            level++;
        }

        public Resources getProduction()
        {
            return getProductionFor(level);
        }

        /// <summary>
        /// </summary>
        /// <returns>Price for current level</returns>
        public Resources getPrice()
        {
            return getPriceFor(level);
        }

        /// <summary>
        /// </summary>
        /// <returns>Price for next level</returns>
        public Resources getPriceForNextLevel()
        {
            return getPriceFor(level + 1);
        }

        public GameTime getConstructionTime()
        {
            return getConstructionTimeFor(level);
        }

        public GameTime getConstructionTimeForNextLevel()
        {
            return getConstructionTimeFor(level+1);
        }

        public bool isBuilt()
        {
            return level > 0;
        }

        public static Dictionary<BuildingType, Building> getEmptyBuildingsList()
        {
            Dictionary<BuildingType, Building> buildings = new Dictionary<BuildingType, Building>();
            buildings.Add(BuildingType.BARRACKS, new Barracks());
            buildings.Add(BuildingType.STABLE, new Stable());
            buildings.Add(BuildingType.TOWN_HALL, new TownHall());
            buildings.Add(BuildingType.FARM, new Farm());
            buildings.Add(BuildingType.LUMBER_MILL, new LumberMill());
            buildings.Add(BuildingType.MINE, new Mine());
            buildings.Add(BuildingType.WALL, new Wall());
            buildings.Add(BuildingType.WORKSHOP, new Workshop());
            return buildings;
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
