using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Exceptions;
using System.Collections;

namespace Plevian.Units
{
    public class Army : IEnumerator, IEnumerable
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
                if (army.units.ContainsKey(pair.Key))
                {
                    army.units[pair.Key].quantity += pair.Value.quantity;
                }
                else
                {
                    army.units.Add(pair.Key, pair.Value);
                }
            }
            return army;
        }

        public static Army operator +(Army lh, Unit rh)
        {
            Army army = new Army();
            UnitType unitType = rh.unitType;
            foreach (var pair in lh.units)
            {
                army.units.Add(pair.Key, pair.Value);
            }

            if (!army.units.ContainsKey(unitType))
            {
                army.units.Add(unitType, rh);
            }
            else
            {
                int quantity = rh.quantity;
                army.units[unitType].quantity += quantity;
            }

            return army;

        }

        public static Army operator -(Army lh, Army rh)
        {
            if( lh.canDivide(rh) == false) throw new ExceptionNotEnoughUnits();
            Army army = new Army();

            foreach (var pair in lh.units)
            {
                army.units.Add(pair.Key, pair.Value);
            }
            
            foreach( var pair in rh.units )
            {
                army.units[pair.Key].quantity -= pair.Value.quantity;

                if (army.units[pair.Key].quantity == 0) army.units.Remove(pair.Key);
            }

            return army;
        }

        /// <summary>
        /// Checks wheter the army( this ) is able to divide into another army
        /// </summary>
        /// <param name="other">another army into which this will be divided to</param>
        /// <returns></returns>
        public bool canDivide(Army other)
        {
            foreach (var pair in other.units)
            {
                if (units.ContainsKey(pair.Key) == false) return false;
                if (units[pair.Key].quantity < pair.Value.quantity) return false;
            }
            return true;
        }

        public int getAttackStrength()
        {
            int attackStrength = 0;
            foreach (var pair in units)
            {
                attackStrength += pair.Value.attackStrength * pair.Value.quantity;
            }
            return attackStrength;
        }

        public int getDefenseInfantry()
        {
            int defenseInfantry = 0;
            foreach (var pair in units)
            {
                defenseInfantry += pair.Value.defenseInfantry * pair.Value.quantity;
            }
            return defenseInfantry;
        }

        public int getDefenseCavalry()
        {
            int defenseCavalry = 0;
            foreach (var pair in units)
            {
                defenseCavalry += pair.Value.defenseCavalry * pair.Value.quantity;
            }
            return defenseCavalry;
        }

        public int getDefenseArchers()
        {
            int defenseArchers = 0;
            foreach (var pair in units)
            {
                defenseArchers += pair.Value.defenseArchers * pair.Value.quantity;
            }
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

        public bool contain(UnitType unitType)
        {
            return units.ContainsKey(unitType);
        }

        public Unit get(UnitType unitType)
        {
            if (contain(unitType) == false) throw new KeyNotFoundException("Army doesn't contain selected unit!!!");
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

        public Dictionary<UnitType, Unit> getUnits()
        {
            return units;
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
