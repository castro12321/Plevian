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

        public void discover(Technology technology)
        {
            if (!technologies.Contains(technology))
                technologies.Add(technology);
        }

        // Uh, forget()? xD
    }

    public class TechnologyLasers : Technology
    {
        public override Resources Cost
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
        public override Resources Cost
        {
            get { return new Stone(1000) + new Iron(3000); }
        }

        public override Requirements Requirements
        {
            get
            {
                return new Requirements()
                     + new BuildingRequirement(BuildingType.WORKSHOP, 5);
            }
        }

        public override GameTime ResearchTime
        {
            get { return new Seconds(60); }
        }
    }
}
