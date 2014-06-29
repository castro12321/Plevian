using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;

namespace Plevian.Buildings
{
    class Stable : Building
    {
        public Stable()
            : base(BuildingType.STABLE)
        {
        }

        public override String getDisplayName()
        {
            return "Stajnia";
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
            switch (level)
            {
                case 1: return new Wood(500);
                case 2: return new Wood(750) + new Stone(25);
                case 3: return new Wood(1000) + new Stone(200);
                case 4: return new Wood(4000) + new Stone(400) + new Iron(40);
                case 5: return new Wood(10000) + new Stone(800) + new Iron(100);
            }
            throw new KeyNotFoundException("Level not found");
        }

        public override LocalTime getConstructionTimeFor(int level)
        {
            switch (level)
            {
                case 1: return new LocalTime(15);
                case 2: return new LocalTime(30);
                case 3: return new LocalTime(60);
                case 4: return new LocalTime(120);
                case 5: return new LocalTime(240);
            }
            throw new KeyNotFoundException("Level not found");
        }
    }
}
