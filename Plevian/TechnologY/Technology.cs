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
        public bool researched { get; set; }
        public abstract Requirements Requirements { get; }
        public abstract Resources Price { get; }
        public abstract GameTime ResearchTime { get; }

        public String Name
        {
            get
            {
                return GetType().Name.Replace("Technology", "");
            }
        }

        public override bool Equals(object obj)
        {
            Technology tech = obj as Technology;
            if (tech != null)
                return Name == tech.Name;
            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}