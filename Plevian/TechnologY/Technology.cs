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
        /// <summary>How important is this technology in the village? The lower the more important. 1 being the most important</summary>
        public virtual int getAiImportance() { return 6; }

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