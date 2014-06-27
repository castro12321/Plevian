using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Buildings
{
    public abstract class Building
    {
        public readonly BuildingType type;

        public abstract String getDisplayName();
        public abstract int getPriceForNextLevel();
        public abstract Resources getProduction();

        public Building(BuildingType type)
        {
            this.type = type;
        }
    }
}
