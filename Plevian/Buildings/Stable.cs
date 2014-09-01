using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.RequirementS;

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

        private static Requirements _requirements = null;
        public override Requirements requirements
        {
            get
            {
                if (_requirements == null)
                {
                    _requirements = new Requirements();
                    _requirements += new BuildingRequirement(BuildingType.TOWN_HALL, 3);
                    _requirements += new BuildingRequirement(BuildingType.BARRACKS, 5);
                }
                return _requirements;
            }
        }

        public override float getUnitTimeModifierFor(Units.UnitType type)
        {
            switch(type)
            {
                case Units.UnitType.KNIGHT: return 1.0f - (0.1f * level); // 10% per level
                default: return 1.0f;
            }
        }
    }
}
