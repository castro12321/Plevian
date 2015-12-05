using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Exceptions;
using System.Collections;
using Plevian.Debugging;
using Plevian.Resource;

namespace Plevian.Units
{
    public class Army : IEnumerable<Unit>
    {
        private Dictionary<UnitType, Unit> units = new Dictionary<UnitType, Unit>();
        
        public Army()
        {
            clear();
        }

        public Army add(Army army)
        {
            foreach (var pair in army.units)
                get(pair.Key).quantity += pair.Value.quantity;
            return this;
        }

        public Army add(Unit unit)
        {
            get(unit.unitType).quantity += unit.quantity;
            return this;
        }

        public bool canRemove(Army army)
        {
            foreach(var pair in army.units)
                if(get(pair.Key).quantity < pair.Value.quantity)
                    return false;
            return true;
        }

        public bool canRemove(Unit unit)
        {
            Army army = new Army();
            army.add(unit);
            return canRemove(army);
        }

        public Army remove(Army army)
        {
            foreach(var pair in army.units)
            {
                Unit our = get(pair.Key);
                if (our.quantity < pair.Value.quantity)
                    throw new ExceptionNotEnoughUnits();
                our.quantity -= pair.Value.quantity;
            }
            return this;
        }

        public Dictionary<UnitType, Unit> getUnitsByType()
        {
            return units;
        }

        public IEnumerable<Unit> getAllUnits()
        {
            return units.Values;
        }

        public int getAttackStrength()
        {
            int attackStrength = 0;
            foreach (var pair in units)
                attackStrength += pair.Value.baseAttackStrength * pair.Value.quantity;
            return attackStrength;
        }

        public int getDefenseInfantry()
        {
            int defenseInfantry = 0;
            foreach (var pair in units)
                defenseInfantry += pair.Value.baseDefenseInfantry * pair.Value.quantity;
            return defenseInfantry;
        }

        public int getDefenseCavalry()
        {
            int defenseCavalry = 0;
            foreach (var pair in units)
                defenseCavalry += pair.Value.baseDefenseCavalry * pair.Value.quantity;
            return defenseCavalry;
        }

        public int getDefenseArchers()
        {
            int defenseArchers = 0;
            foreach (var pair in units)
                defenseArchers += pair.Value.baseDefenseArchers * pair.Value.quantity;
            return defenseArchers;
        }

        public float getMovementSpeed()
        {
            float slowest = 0f;
            foreach(var pair in units)
            {
                float speed = (float)pair.Value.baseMovementSpeed;
                if (speed > slowest)
                    slowest = speed;
            }
            return slowest;
        }

        public bool contains(UnitType unitType)
        {
            if (units.ContainsKey(unitType))
                return units[unitType].quantity > 0;
            return false;
        }

        public Unit get(UnitType unitType)
        {
            return units[unitType];
        }

        /// <summary>
        /// </summary>
        /// <returns>returns number of units in whole army</returns>
        public int size()
        {
            int size = 0;
            foreach (var pair in units)
                size += pair.Value.quantity;
            return size;
        }

        public void clear()
        {
            units.Clear();
            List<Unit> allUnits = UnitFactory.createAllUnits();
            foreach (Unit unit in allUnits)
                units.Add(unit.unitType, unit);
        }

        public int getUnitClassCount(UnitClass unitClass)
        {
            int size = 0;
            foreach (var pair in units)
                if (pair.Value.unitClass == unitClass) size += pair.Value.quantity;
            return size;
        }

        public int getLootCapacity()
        {
            int capacity = 0;
            foreach(var pair in units)
            {
                capacity += pair.Value.baseLootCapacity * pair.Value.quantity;
            }
            return capacity;
        }

        public Resources Upkeep
        {
            get
            {
                Resources upkeep = new Resources();
                foreach (var pair in units)
                {
                    //Logger.log("(" + pair.Value.quantity + ") " + pair.Key + " --> " + pair.Value.baseUpkeepCost);
                    upkeep += pair.Value.baseUpkeepCost * pair.Value.quantity;
                }
                return upkeep;
            }
        }

        public string toString()
        {
            string str = "";
            str = "Army listing :\n";
            foreach (var pair in units)
            {
                string name = Enum.GetName(typeof(UnitType), pair.Key);
                str += name + " - " + pair.Value.quantity + "\n";
            }
            return str;
        }

        private int position = -1;

        public int Count
        {
            get
            {
                return units.Count;
            }
        }

        public Unit this[String type]
        {
            get
            {
                UnitType uType = (UnitType)Enum.Parse(typeof(UnitType), type);
                return get(uType);
            }
        }

        public Unit this[UnitType type]
        {
            get { return get(type); }
        }

        public void Reset()
        {
            position = 0;
        }

        public IEnumerator<Unit> GetEnumerator()
        {
            return units.Values.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return units.GetEnumerator();
        }

        public string ToStringMinimal()
        {
            return toString().Replace("\n", "; ");
        }

        public override string ToString()
        {
            String army = "Army (" + size() + " units):\n";
            foreach (Unit unit in units.Values)
                //army += unit.name + "=" + unit.quantity + "\n";
                army += unit.ToString() + "\n";
            return army;
        }

        public Army clone()
        {
            return new Army().add(this);
        }
    }
}
