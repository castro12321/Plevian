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
        public int level { get; private set; }

        public abstract String getDisplayName();
        public abstract Resources getPriceForNextLevel();
        public abstract Resources getProduction();
        public abstract int getMaxLevel();

        public Building(BuildingType type)
        {
            this.type = type;
        }
    }
}
