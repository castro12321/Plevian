using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Units
{
    class Army
    {
        private Dictionary<UnitType, Unit> units = new Dictionary<UnitType, Unit>();
        
        public Army()
        {}

        public static Army operator + ( Army lh, Army rh )
        {
            Army army = new Army();
            foreach (var pair in lh.units)
            {
                army.units.Add(pair.Key, pair.Value);
            }

            foreach (var pair in rh.units)
            {
                if (army.units[pair.Key] != null)
                {
                    army.units[pair.Key].quanity += pair.Value.quanity;
                }
                else
                {
                    army.units.Add(pair.Key, pair.Value);
                }
            }
            return army;
        }

        public static Army operator -(Army lh, Army rh)
        {
            return new Army();
        }

        public static bool operator >=(Army lh, Army rh)
        {
            foreach (var pair in lh.units)
            {
                if (rh.units[pair.Key] == null) return false;
                if (rh.units[pair.Key].quanity > pair.Value.quanity) return false;
            }
            return true;
        }



    }
}
