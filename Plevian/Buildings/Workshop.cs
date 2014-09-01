using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.RequirementS;
using Plevian.Units;

namespace Plevian.Buildings
{
    class Workshop : Building
    {
        public Workshop()
            : base(BuildingType.WORKSHOP)
        {
        }

        public override String getDisplayName()
        {
            return "Warsztat";
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
                case 1: return new Wood(300)  + new Stone(100);
                case 2: return new Wood(500)  + new Stone(200);
                case 3: return new Wood(1000) + new Stone(500);
                case 4: return new Wood(2500) + new Stone(1000) + new Iron(500);
                case 5: return new Wood(9500) + new Stone(3000) + new Iron(1500);
            }
            throw new KeyNotFoundException("Level not found");
        }

        public override GameTime getConstructionTimeFor(int level)
        {
            switch (level)
            {
                case 1: return new Seconds(25);
                case 2: return new Seconds(50);
                case 3: return new Seconds(90);
                case 4: return new Seconds(150);
                case 5: return new Seconds(240);
            }
            throw new KeyNotFoundException("Level not found");
        }

        private static Requirements _requirements = null;
        public override Requirements requirements
        {
            get
            {
                if (_requirements == null)
                {
                    _requirements = new Requirements();
                    _requirements += new BuildingRequirement(BuildingType.TOWN_HALL, 5);
                }
                return _requirements;
            }
        }

        public override float getUnitTimeModifierFor(Units.UnitType type)
        {
            switch (type)
            {
                case UnitType.RAM:
                    return 1.0f - (0.10f * level); // 10% per level
                default: return 1.0f;
            }
        }
    }
}
