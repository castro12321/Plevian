using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.RequirementS;

namespace Plevian.Buildings
{
    class Wall : Building
    {
        public Wall()
            : base(BuildingType.WALL)
        {
        }

        public override String getDisplayName()
        {
            return "Mur";
        }
        
        public int getBaseDefense()
        {
            switch(level)
            {
                case 0: return 0;
                case 1: return 10;
                case 2: return 30;
                case 3: return 50;
                case 4: return 100;
                case 5: return 250;
            }
            return 0;
        }

        public float getDefense()
        {
            switch(level)
            {
                case 1: return 1.01f;
                case 2: return 1.02f;
                case 3: return 1.04f;
                case 4: return 1.08f;
                case 5: return 1.16f;
            }
            return 1f;
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
                case 1: return new Wood(150) + new Stone(250);
                case 2: return new Wood(300) + new Stone(500);
                case 3: return new Wood(1000) + new Stone(750);
                case 4: return new Wood(2500) + new Stone(1000) + new Iron(300);
                case 5: return new Wood(5000) + new Stone(2000) + new Iron(1000);
            }
            throw new KeyNotFoundException("Level not found");
        }

        public override GameTime getConstructionTimeFor(int level)
        {
            switch (level)
            {
                case 1: return new GameTime(15);
                case 2: return new GameTime(30);
                case 3: return new GameTime(60);
                case 4: return new GameTime(120);
                case 5: return new GameTime(240);
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
                    _requirements += new BuildingRequirement(BuildingType.TOWN_HALL, 1);
                    _requirements += new BuildingRequirement(BuildingType.BARRACKS, 1);
                }
                return _requirements;
            }
        }
    }
}
