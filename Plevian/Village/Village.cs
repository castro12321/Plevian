using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Village
{
    class Village
    {
        private Dictionary<Buildings.BuildingType, Buildings.Building> buildings = Buildings.Building.getEmptyBuildingsList();
        private Dictionary<Units.UnitType, int> units = new Dictionary<Units.UnitType, int>();
        public Resources.Resources resources { get; private set; }
        
        public Village()
        {
        }

        public void setBuildings(Dictionary<Buildings.BuildingType, Buildings.Building> buildings)
        {
            this.buildings = buildings;
        }

        public void addResources(Resources.Resources add)
        {
            resources = resources + add;
        }

        public void takeResources(Resources.Resources take)
        {
            resources = resources - take;
        }
        
        public void collectProduction()
        {
            foreach (KeyValuePair<Buildings.BuildingType, Buildings.Building> building in buildings)
                addResources(building.Value.getProduction());
        }

        public bool isBuilt(Buildings.BuildingType type)
        {
            return buildings[type].isBuilt();
        }

        public void build(Buildings.BuildingType buildingType)
        {
            if (isBuilt(buildingType))
                return;


        }

        public void upgrade(Buildings.BuildingType buildingType)
        {
            if(!isBuilt(buildingType))

        }
    }
}
