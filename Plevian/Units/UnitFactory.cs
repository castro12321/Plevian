﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Units
{
    public static class UnitFactory
    {
        public static Unit createUnit(UnitType type, int quantity)
        {
            switch(type)
            {
                case UnitType.ARCHER :  return new Archer(quantity);
                case UnitType.KNIGHT :  return new Knight(quantity);
                case UnitType.WARRIOR : return new Warrior(quantity);
                case UnitType.SETTLER : return new Settler(quantity);
                case UnitType.DUKE :    return new Duke(quantity);
                case UnitType.TRADER:   return new Trader(quantity);
                case UnitType.RAM:      return new Ram(quantity);
            }

            throw new NotSupportedException("UnitFactory cannot create " + type + "; " + quantity);
        }

        public static Army createArmyWithAllUnits()
        {
            Army army = new Army();
                foreach (UnitType type in (UnitType[]) Enum.GetValues(typeof(UnitType)))
                {
                    army += createUnit(type, 1);
                }
            return army;
        }
    }
}
