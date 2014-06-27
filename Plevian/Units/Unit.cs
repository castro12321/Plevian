using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resources;

namespace Plevian.Units
{
    class Unit
    {
        public virtual int getAttackStrength();

        public virtual int getDefenseOnInfantry();
        public virtual int getDefenseOnCavalry();
        public virtual int getDefenseOnArchers();

        public virtual int GetMovementSpeed();

        public virtual int getLootCapacity();

        public virtual Resources.Resources getRecruitCost();
        public virtual Resources.Resources getUpkeepCost();

        public virtual UnitType getUnitType();
    }
}
