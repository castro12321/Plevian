using Plevian.Buildings;
using Plevian.TechnologY;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.RequirementS
{
    public class TechnologyRequirement : Requirement
    {
        public readonly Technology technology;

        public TechnologyRequirement(Technology technology)
        {
            this.technology = technology;
        }

        public override bool isFullfilled(Village village)
        {
            return village.Owner.technologies.isDiscovered(technology);
        }

        public override string ToString()
        {
            return "Requirement - technology " + technology;
        }
    }
}
