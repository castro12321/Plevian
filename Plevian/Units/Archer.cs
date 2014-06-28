using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resources;

namespace Plevian.Units
{
    class Archer : Unit
    {
        public Archer()
        {
        }

        public static readonly int attackStrength = 35;

        public static readonly int defenseOnInfantry = 5;
        public static readonly int defenseOnCavalry = 10;
        public static readonly int defenseOnArchers = 20;

        public static readonly int movementSpeed = 25;
        public static readonly int lootCapacity = 20;

        public static readonly Resources.Resources recruitCost = new Food(40) + new Wood(100);
        public static readonly Resources.Resources upkeepCost = new Wood(1) + new Food(5);

        public static readonly UnitType unitType = UnitType.ARCHER;

        public override int getAttackStrength() { return attackStrength; }

        public override int getDefenseOnInfantry() { return defenseOnInfantry; }
        public override int getDefenseOnCavalry() { return defenseOnCavalry; }
        public override int getDefenseOnArchers() { return defenseOnArchers; }

        public override int GetMovementSpeed() { return movementSpeed; }

        public override int getLootCapacity() { return lootCapacity; }

        public override Resources.Resources getRecruitCost() { return recruitCost; }
        public override Resources.Resources getUpkeepCost() { return upkeepCost; }

        public override UnitType getUnitType() { return unitType; }
    }
}
