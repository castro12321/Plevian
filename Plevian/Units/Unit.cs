using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;

namespace Plevian.Units
{
    public abstract class Unit
    {
        public int quanity;

        public Unit(int quanity = 0)
        {
            this.quanity = quanity;
        }


        public abstract int getAttackStrength();

        public abstract int getDefenseOnInfantry();
        public abstract int getDefenseOnCavalry();
        public abstract int getDefenseOnArchers();

        public abstract int GetMovementSpeed();
        public abstract int getLootCapacity();

        public abstract Resources getRecruitCost();
        public abstract float getRecruitTime();
        public abstract Resources getUpkeepCost();
        
        public abstract UnitType getUnitType();

        public abstract Unit clone();
    }
}
