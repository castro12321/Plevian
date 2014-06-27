using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Buildings
{
    public class TownHall : Building
    {
        public TownHall()
            : base(BuildingType.TOWN_HALL)
        {
        }

        public override String getDisplayName()
        {
            return "Ratusz";
        }

        public override Resources getProduction()
        {
            return new Resources();
        }

        public override int getMaxLevel()
        {
            return 10;
        }

        public override int getPriceForNextLevel()
        {
            switch(level)
            {
                case 0: return new Wood(150) + new Stone(50);
            }
        }
    }
}
