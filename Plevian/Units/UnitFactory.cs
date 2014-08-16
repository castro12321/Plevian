﻿using System;
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
                case UnitType.ARCHER :
                    return new Archer(quanity);
                case UnitType.KNIGHT :
                    return new Knight(quanity);
                case UnitType.WARRIOR :
                    return new Warrior(quanity);
                case UnitType.SETTLER :
                    return new Settler(quanity);
            }

            throw new Exception("Wrong unit specified in UnitFactory");
        }
    }
}