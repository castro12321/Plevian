using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.RequirementS
{
    public abstract class Requirement
    {
        public abstract bool isFullfilled(Village village);
    }
}
