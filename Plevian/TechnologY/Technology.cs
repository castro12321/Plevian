using Plevian.RequirementS;
using Plevian.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.TechnologY
{
    public abstract class Technology
    {
        public abstract Requirements Requirements { get; }
        public abstract Resources Cost { get; }
        public abstract GameTime ResearchTime { get; }

        public String name
        {
            get
            {
                return GetType().Name;
            }
        }

        public override bool Equals(object obj)
        {
            Technology tech = obj as Technology;
            if (tech != null)
                return name == tech.name;
            return false;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override string ToString()
        {
            return name;
        }
    }
}