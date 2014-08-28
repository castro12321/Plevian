using Plevian.Villages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.RequirementS 
{
    public class Requirements : IEnumerator, IEnumerable
    {
        List<Requirement> _requirements = new List<Requirement>();
        int position = -1;

        public bool isFullfilled(Village village)
        {
            foreach( var req in _requirements )
            {
                if (!req.isFullfilled(village))
                    return false;
            }
            return true;
        }

        public void addRequirement(Requirement requirement)
        {
            _requirements.Add(requirement);
        }

        public static Requirements operator +(Requirements lh, Requirement rh)
        {
            Requirements _ret = new Requirements();
            foreach (Requirement req in lh)
                _ret._requirements.Add(req);
            _ret._requirements.Add(rh);
            return _ret;
        }

        public int Count
        {
            get
            {
                return _requirements.Count;
            }
        }

        public Requirement this[int i]
        {
            get { return _requirements[i];  }
            protected set { _requirements[i] = value; }
        }

       

        public object Current
        {
            get
            {
                return _requirements[position];
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
