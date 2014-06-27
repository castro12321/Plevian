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
        public Knigth()
        {

        }

        public static readonly int attackStrength = 20;

        public static readonly int defenseOnInfantry = 50;
        public static readonly int defenseOnCavalry = 5;
        public static readonly int defenseOnArchers = 10;

        public static readonly int movementSpeed = 25;
        public static readonly int lootCapacity = 50;

        public static readonly Resources.Resources recruitCost = new Iron(100) + new Food(50) + new Wood(25);
        public static readonly Resources.Resources upkeepCost = new Iron(1) + new Food(5);

        public static readonly UnitType unitType = UnitType.KNIGHT;

        public int getAttackStrength() { return attackStrength; }

        public virtual int getDefenseOnInfantry() { return defenseOnInfantry; }
        public virtual int getDefenseOnCavalry() { return defenseOnCavalry; }
        public virtual int getDefenseOnArchers() { return defenseOnArchers; }

        public virtual int GetMovementSpeed() { return movementSpeed; }

        public virtual int getLootCapacity() { return lootCapacity; }

        public virtual Resources.Resources getRecruitCost() { return recruitCost; }
        public virtual Resources.Resources getUpkeepCost() { return upkeepCost; }

        public virtual UnitType getUnitType() { return unitType; }
    }
}
