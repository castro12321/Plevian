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
        private Resources.Resources resources = new Resources.Resources();
    }
}
