using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Village
{
    class Village
    {
        private List<Buildings.Building> buildings = new List<Buildings.Building>();
        private Dictionary<Units.UnitType, int> units = new Dictionary<Units.UnitType, int>();
        public Resources.Resources resources { get; private set; }
        
        public Village()
        {
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
            foreach (Buildings.Building building in buildings)
                addResources(building.getProduction());
        }

        public void build(Buildings.Building building)
        {

        }
    }
}
