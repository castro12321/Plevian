using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Buildings;
using Plevian.RequirementS;
using Plevian.Resource;

namespace Plevian.TechnologY
{
    public class TechnologyLeatherArmors : Technology
    {
        public override Resources Price
        {
            get { return new Food(350) + new Wood(1000); }
        }

        public override Requirements Requirements
        {
            get
            {
                return new Requirements()
                     + new BuildingRequirement(BuildingType.WORKSHOP, 1);
            }
        }

        public override GameTime ResearchTime
        {
            get { return new GameTime(100); }
        }
    }

    public class TechnologyChainArmors : Technology
    {
        public override Resources Price
        {
            get { return new Food(750) + new Wood(1500) + new Iron(1500); }
        }

        public override Requirements Requirements
        {
            get
            {
                return new Requirements()
                     + new BuildingRequirement(BuildingType.WORKSHOP, 3)
                     + new TechnologyRequirement(new TechnologyLeatherArmors());
            }
        }

        public override GameTime ResearchTime
        {
            get { return new GameTime(300); }
        }
    }

    public class TechnologyPlateArmors : Technology
    {
        public override Resources Price
        {
            get { return new Food(1800) + new Wood(2000) + new Iron(3500); }
        }

        public override Requirements Requirements
        {
            get
            {
                return new Requirements()
                     + new BuildingRequirement(BuildingType.WORKSHOP, 5)
                     + new TechnologyRequirement(new TechnologyChainArmors());
            }
        }

        public override GameTime ResearchTime
        {
            get { return new GameTime(1000); }
        }
    }
}
