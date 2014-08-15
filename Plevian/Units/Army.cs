using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Exceptions;

namespace Plevian.Units
{
    public class Army
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
                    army.units[pair.Key].quanity += pair.Value.quanity;
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
            UnitType unitType = rh.getUnitType();
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
                int quanity = rh.quanity;
                army.units[unitType].quanity += quanity;
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
                army.units[pair.Key].quanity -= pair.Value.quanity;

                if (army.units[pair.Key].quanity == 0) army.units.Remove(pair.Key);
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
                if (units[pair.Key].quanity < pair.Value.quanity) return false;
            }
            return true;
        }

        public int getAttackStrength()
        {
            int attackStrength = 0;
            foreach (var pair in units)
            {
                attackStrength += pair.Value.getAttackStrength() * pair.Value.quanity;
            }
            return attackStrength;
        }

        public int getDefenseInfantry()
        {
            int defenseInfantry = 0;
            foreach (var pair in units)
            {
                defenseInfantry += pair.Value.getDefenseOnInfantry() * pair.Value.quanity;
            }
            return defenseInfantry;
        }

        public int getDefenseCavalry()
        {
            int defenseCavalry = 0;
            foreach (var pair in units)
            {
                defenseCavalry += pair.Value.getDefenseOnCavalry() * pair.Value.quanity;
            }
            return defenseCavalry;
        }

        public int getDefenseArchers()
        {
            int defenseArchers = 0;
            foreach (var pair in units)
            {
                defenseArchers += pair.Value.getDefenseOnArchers() * pair.Value.quanity;
            }
            return defenseArchers;
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
                size += pair.Value.quanity;
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
                if (pair.Value.getUnitClass() == unitClass) size += pair.Value.quanity;
            return size;
        }

        public string toString()
        {
            string str = "";
            str = "Army listing :\n";
            foreach (var pair in units)
            {
                string name = Enum.GetName(typeof(UnitType), pair.Key);
                str += name + " - " + pair.Value.quanity + "\n";
            }
            return str;
        }

        
    }
}
