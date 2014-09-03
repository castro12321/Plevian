using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Units
{
    public static class UnitFactory
    {
        public static Unit createUnit(UnitType type, int quanity)
        {
            switch(type)
            {
                case UnitType.ARCHER :  return new Archer(quanity);
                case UnitType.KNIGHT :  return new Knight(quanity);
                case UnitType.WARRIOR : return new Warrior(quanity);
                case UnitType.SETTLER : return new Settler(quanity);
                case UnitType.DUKE :    return new Duke(quanity);
                case UnitType.TRADER:   return new Trader(quanity);
                case UnitType.RAM:      return new Ram(quanity);
            }

            throw new NotSupportedException("UnitFactory cannot create " + type + "; " + quanity);
        }

        public Army createArmyWithAllUnits()
        {
            Army army = new Army();
                foreach (UnitType type in (UnitType[]) Enum.GetValues(typeof(UnitType)))
                {
                    army += createUnit(type);
                }
            return army;
        }
    }
}
