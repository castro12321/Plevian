using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;

namespace Plevian.Units
{
    abstract class Unit
    {
        public int quanity;

        public abstract int getAttackStrength();

        public abstract int getDefenseOnInfantry();
        public abstract int getDefenseOnCavalry();
        public abstract int getDefenseOnArchers();

        public abstract int GetMovementSpeed();

        public abstract int getLootCapacity();

        public abstract Resources getRecruitCost();
        public abstract Resources getUpkeepCost();

        public abstract UnitType getUnitType();
    }
}
