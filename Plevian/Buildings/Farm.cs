using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.RequirementS;

namespace Plevian.Buildings
{
    class Farm : Building
    {
        public Farm()
            : base(BuildingType.FARM)
        {
        }

        public override String getDisplayName()
        {
            return "Farma";
        }

        public override Resources getProductionFor(int level)
        {
            switch(level)
            {
                case 0: return new Resources(1, 0, 0, 0);
                case 1: return new Resources(10, 0, 0, 0);
                case 2: return new Resources(50, 0, 0, 0);
                case 3: return new Resources(200, 0, 0, 0);
                case 4: return new Resources(1000, 0, 0, 0);
                case 5: return new Resources(5000, 0, 0, 0);
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
                case 1: return new Wood(100) + new Food(50);
                case 2: return new Wood(350) + new Food(150);
                case 3: return new Wood(1000) + new Food(300) + new Stone(200);
                case 4: return new Wood(2500) + new Stone(1500) + new Stone(400) + new Iron(100);
                case 5: return new Wood(5000) + new Stone(2500) + new Stone(1000) + new Iron(500);
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

        public override int getAiImportance()
        {
            return 1;
        }
    }
}
