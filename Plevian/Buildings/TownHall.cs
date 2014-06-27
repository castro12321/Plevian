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

        public override Resources.Resources getProduction()
        {
            return new Resources.Resources();
        }

        public override int getMaxLevel()
        {
            return 6;
        }

        public override Resources.Resources getPriceForNextLevel()
        {
            switch(level)
            {
                case 0: return new Resources.Wood(150)  + new Resources.Stone(50);
                case 1: return new Resources.Wood(300)  + new Resources.Stone(100);
                case 3: return new Resources.Wood(1000) + new Resources.Stone(300);
                case 4: return new Resources.Wood(2500) + new Resources.Stone(750)  + new Resources.Iron(300);
                case 5: return new Resources.Wood(5000) + new Resources.Stone(2000) + new Resources.Iron(1000);
            }
            throw new KeyNotFoundException("Level not found");
        }
    }
}
