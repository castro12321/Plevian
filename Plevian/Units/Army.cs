using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Exceptions;
using System.Collections;
using Plevian.Debugging;

namespace Plevian.Units
{
    public class Army : IEnumerator, IEnumerable
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

        public Dictionary<UnitType, Unit> getUnits()
        {
            return units;
        }

        public int getAttackStrength()
        {
            int attackStrength = 0;
            foreach (var pair in units)
                attackStrength += pair.Value.attackStrength * pair.Value.quantity;
            return attackStrength;
        }

        public int getDefenseInfantry()
        {
            int defenseInfantry = 0;
            foreach (var pair in units)
                defenseInfantry += pair.Value.defenseInfantry * pair.Value.quantity;
            return defenseInfantry;
        }

        public int getDefenseCavalry()
        {
            int defenseCavalry = 0;
            foreach (var pair in units)
                defenseCavalry += pair.Value.defenseCavalry * pair.Value.quantity;
            return defenseCavalry;
        }

        public int getDefenseArchers()
        {
            int defenseArchers = 0;
            foreach (var pair in units)
                defenseArchers += pair.Value.defenseArchers * pair.Value.quantity;
            return defenseArchers;
        }

        public float getMovementSpeed()
        {
            float slowest = 0f;
            foreach(var pair in units)
            {
                float speed = (float)pair.Value.movementSpeed;
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
                capacity += pair.Value.lootCapacity * pair.Value.quantity;
            }
            return capacity;
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

        public object Current
        {
            get
            {
                return units.Values.ElementAt(position);
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
            return (IEnumerator)this;
        }

        public override string ToString()
        {
            String s = "army: ";
            foreach (Unit unit in units.Values)
                s += unit.name + "=" + unit.quantity + "; ";
            return s;
        }
    }
}
