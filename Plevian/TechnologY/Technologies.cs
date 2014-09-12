using Plevian.Buildings;
using Plevian.RequirementS;
using Plevian.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.TechnologY
{
    public class Technologies
    {
        public List<Technology> technologies = new List<Technology>();

        public Technologies()
        {
            technologies.Add(new TechnologyFire());
            technologies.Add(new TechnologyFoo());
            technologies.Add(new TechnologyLasers());
            technologies.Add(new TechnologyNukes());
        }

        private Technology find(Technology technology)
        {
            return technologies.Find(x => x.Equals(technology));
        }

        public void discover(Technology technology)
        {
            if (!isDiscovered(technology))
                find(technology).researched = true;
        }

        public bool isDiscovered(Technology technology)
        {
            return find(technology).researched;
        }
    }

    public class TechnologyFire : Technology
    {
        public override Resources Price
        {
            get { return new Wood(50); }
        }

        public override Requirements Requirements
        {
            get { return new Requirements(); }
        }

        public override GameTime ResearchTime
        {
            get { return new Seconds(30); }
        }
    }

    public class TechnologyFoo : Technology
    {
        public override Resources Price
        {
            get { return new Wood(100) + new Food(150); }
        }

        public override Requirements Requirements
        {
            get { return new Requirements(); }
        }

        public override GameTime ResearchTime
        {
            get { return new Seconds(30); }
        }
    }

    public class TechnologyLasers : Technology
    {
        public override Resources Price
        {
            get { return new Stone(500) + new Iron(1500); }
        }
        
        public override Requirements Requirements
        {
            get { return new Requirements()
                       + new BuildingRequirement(BuildingType.WORKSHOP, 3); }
        }

        public override GameTime ResearchTime
        {
            get { return new Seconds(30); }
        }
    }

    public class TechnologyNukes : Technology
    {
        public override Resources Price
        {
            get { return new Stone(1000) + new Iron(3000); }
        }

        public override Requirements Requirements
        {
            get
            {
                return new Requirements()
                     + new BuildingRequirement(BuildingType.WORKSHOP, 5)
                     + new TechnologyRequirement(new TechnologyLasers());
            }
        }

        public override GameTime ResearchTime
        {
            get { return new Seconds(60); }
        }
    }
}
