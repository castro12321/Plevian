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
            foreach (var key in lh.units)
            {
               
            }

            return army;
        }



    }
}
