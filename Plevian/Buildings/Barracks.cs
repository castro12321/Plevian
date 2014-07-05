using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;

namespace Plevian.Buildings
{
    class Barracks : Building
    {
        public Barracks()
            : base(BuildingType.BARRACKS)
        {
        }

        public override String getDisplayName()
        {
            return "Koszary";
        }

        public override Resources getProductionFor(int level)
        {
            return new Resources(0, 0, 0, 0);
        }

        public override int getMaxLevel()
        {
            return 5;
        }

        public override Resources getPriceFor(int level)
        {
            switch(level)
            {
                case 1: return new Wood(150)  + new Stone(50);
                case 2: return new Wood(300)  + new Stone(100);
                case 3: return new Wood(1000) + new Stone(300);
                case 4: return new Wood(2500) + new Stone(750)  + new Iron(300);
                case 5: return new Wood(5000) + new Stone(2000) + new Iron(1000);
            }
            throw new KeyNotFoundException("Level not found");
        }

        public override GameTime getConstructionTimeFor(int level)
        {
            switch (level)
            {
                case 1: return new Seconds(15);
                case 2: return new Seconds(30);
                case 3: return new Seconds(60);
                case 4: return new Seconds(120);
                case 5: return new Seconds(240);
            }
            throw new KeyNotFoundException("Level not found");
        }
    }
}
