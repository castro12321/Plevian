using Plevian.Villages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.RequirementS 
{
    public class Requirements : IEnumerator, IEnumerable
    {
        private List<Requirement> requirementsList = new List<Requirement>();
        public ReadOnlyCollection<Requirement> RequirementsList { get { return requirementsList.AsReadOnly(); } }

        public bool isFullfilled(Village village)
        {
            foreach(Requirement req in requirementsList)
                if (!req.isFullfilled(village))
                    return false;
            return true;
        }

        public void addRequirement(Requirement requirement)
        {
            requirementsList.Add(requirement);
        }

        public static Requirements operator +(Requirements lh, Requirement rh)
        {
            Requirements _ret = new Requirements();
            foreach (Requirement req in lh.RequirementsList)
                _ret.requirementsList.Add(req);
            _ret.requirementsList.Add(rh);
            return _ret;
        }

        private int position = -1;

        public int Count
        {
            get
            {
                return requirementsList.Count;
            }
        }

        public Requirement this[int i]
        {
            get { return requirementsList[i];  }
            protected set { requirementsList[i] = value; }
        }

        public object Current
        {
            get
            {
                return requirementsList[position];
            }
        }

        public bool MoveNext()
        {
            position++;
            return (position < Count);
        }

        public void Reset()
        {
            position = 0;
        }

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator) this;
        }
    }
}
