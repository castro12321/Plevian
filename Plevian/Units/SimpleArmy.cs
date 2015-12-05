using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Units
{
    public class SimpleArmy : List<Unit>
    {
        public Dictionary<UnitType, List<Unit>> GetUnitsByType()
        {
            Dictionary<UnitType, List<Unit>> unitsByType = new Dictionary<UnitType, List<Unit>>();
            foreach(Unit unit in this)
            {
                if(!unitsByType.ContainsKey(unit.unitType))
                    unitsByType[unit.unitType] = new List<Unit>();
                unitsByType[unit.unitType].Add(unit);
            }
            return unitsByType;
        }

        public static SimpleArmy FromArmy(Army army)
        {
            SimpleArmy simpleArmy = new SimpleArmy();
            foreach(Unit unit in army.getAllUnits())
            {
                for(int i = 0; i < unit.quantity; i++)
                {
                    Unit cloned = unit.clone();
                    cloned.quantity = 1;
                    simpleArmy.Add(cloned);
                }
            }
            return simpleArmy;
        }

        public Army ToArmy()
        {
            Army army = new Army();
            foreach (Unit unit in this)
                army.add(unit.clone());
            return army;
        }

        public SimpleArmy Clone()
        {
            SimpleArmy clone = new SimpleArmy();
            foreach(Unit unit in this)
                clone.Add(unit.clone());
            return clone;
        }
    }
}
