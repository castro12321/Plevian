using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.RequirementS;

namespace Plevian.Buildings
{
    class Mine : Building
    {
        public Mine()
            : base(BuildingType.MINE)
        {
        }

        public override String getDisplayName()
        {
            return "Kopalnia";
        }

        public override Resources getProductionFor(int level)
        {
            switch(level)
            {
                case 0: return new Resources(0, 0, 0, 0);
                case 1: return new Resources(0, 0, 1, 3);
                case 2: return new Resources(0, 0, 5, 15);
                case 3: return new Resources(0, 0, 10, 30);
                case 4: return new Resources(0, 0, 25, 100);
                case 5: return new Resources(0, 0, 100, 500);
            }
            throw new KeyNotFoundException("Level not found");
        }

        public override int getMaxLevel()
        {
            return 5;
        }

        public override Resources getPriceFor(int level)
        {
            switch(level)
            {
                case 1: return new Wood(100);
                case 2: return new Wood(500);
                case 3: return new Wood(2000);
                case 4: return new Wood(10000) + new Stone(1500);
                case 5: return new Wood(25000) + new Stone(2500) + new Iron(500);
            }
            throw new KeyNotFoundException("Level not found");
        }

        public override GameTime getConstructionTimeFor(int level)
        {
            switch (level)
            {
                case 1: return new GameTime(10);
                case 2: return new GameTime(25);
                case 3: return new GameTime(50);
                case 4: return new GameTime(120);
                case 5: return new GameTime(360);
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
                }
                return _requirements;
            }
        }

        public override int getAiResourceModifier()
        {
            return 1;
        }
    }
}
