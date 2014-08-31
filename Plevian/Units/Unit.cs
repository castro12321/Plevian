using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.RequirementS;

namespace Plevian.Units
{
    public abstract class Unit
    {
        public int quanity;

        public Unit(int quanity = 0)
        {
            this.quanity = quanity;
        }

        public Resources getWholeUnitCost()
        {
            return getRecruitCost() * quanity;
        }

#region abstract functions
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
        public abstract UnitClass getUnitClass();

        public abstract Requirements getRequirements();

        public abstract Unit clone();
#endregion

        public override string ToString()
        {
            return Enum.GetName(typeof(UnitType), getUnitType()) + "(" + quanity + ")";
        }

    }
}
