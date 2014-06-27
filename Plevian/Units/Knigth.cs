using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resources;

namespace Plevian.Units
{
    class Knigth : Unit
    {
        public static readonly int attackStrength = 20;

        public static readonly int defenseOnInfantry = 50;
        public static readonly int defenseOnCavalry = 5;
        public static readonly int defenseOnArchers = 10;




        public int getAttackStrength() { return attackStrength; }

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
