using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;

namespace Plevian.Units
{
    class Warrior : Unit
    {
        public Warrior()
        {
        }

        public static readonly int attackStrength = 10;

        public static readonly int defenseOnInfantry = 25;
        public static readonly int defenseOnCavalry = 4;
        public static readonly int defenseOnArchers = 5;

        public static readonly int movementSpeed = 28;
        public static readonly int lootCapacity = 75;

        public static readonly Resources recruitCost = new Food(50) + new Wood(50);
        public static readonly Resources upkeepCost = new Food(5);

        public static readonly UnitType unitType = UnitType.WARRIOR;

        public override int getAttackStrength() { return attackStrength; }

        public override int getDefenseOnInfantry() { return defenseOnInfantry; }
        public override int getDefenseOnCavalry() { return defenseOnCavalry; }
        public override int getDefenseOnArchers() { return defenseOnArchers; }

        public override int GetMovementSpeed() { return movementSpeed; }

        public override int getLootCapacity() { return lootCapacity; }

        public override Resources getRecruitCost() { return recruitCost; }
        public override Resources getUpkeepCost() { return upkeepCost; }

        public override UnitType getUnitType() { return unitType; }
    }
}
