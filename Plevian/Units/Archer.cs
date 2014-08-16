using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;

namespace Plevian.Units
{
    public class Archer : Unit
    {
        public Archer(int quanity = 0)
            : base(quanity)
        {
        }

        public static readonly int attackStrength = 35;

        public static readonly int defenseOnInfantry = 5;
        public static readonly int defenseOnCavalry = 10;
        public static readonly int defenseOnArchers = 20;

        public static readonly int movementSpeed = 1;
        public static readonly int lootCapacity = 20;

        public static readonly float recruitTime = 10.3f;

        public static readonly Resources recruitCost = new Food(40) + new Wood(100);
        public static readonly Resources upkeepCost = new Wood(1) + new Food(5);

        public static readonly UnitType unitType = UnitType.ARCHER;
        public static readonly UnitClass unitClass = UnitClass.ARCHER;

#region overrided functions
        public override Unit clone()
        {
            return new Archer(quanity);
        }

        public override int getAttackStrength() { return attackStrength; }

        public override int getDefenseOnInfantry() { return defenseOnInfantry; }
        public override int getDefenseOnCavalry() { return defenseOnCavalry; }
        public override int getDefenseOnArchers() { return defenseOnArchers; }

        public override int GetMovementSpeed() { return movementSpeed; }

        public override int getLootCapacity() { return lootCapacity; }

        public override Resources getRecruitCost() { return recruitCost; }
        public override float getRecruitTime() { return recruitTime; }
        public override Resources getUpkeepCost() { return upkeepCost; }

        public override UnitType getUnitType() { return unitType; }
        public override UnitClass getUnitClass() { return unitClass; }
#endregion
    }
}
