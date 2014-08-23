using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;

namespace Plevian.Units
{
    public class Trader : Unit
    {
        public Trader(int quanity = 0)
            : base(quanity)
        {
        }

        public static readonly int attackStrength = 5;

        public static readonly int defenseOnInfantry = 1;
        public static readonly int defenseOnCavalry = 1;
        public static readonly int defenseOnArchers = 1;

        public static readonly int movementSpeed = 3;
        public static readonly int lootCapacity = 500;

        public static readonly float recruitTime = 3.3f;

        public static readonly Resources recruitCost = new Food(500) + new Wood(150);
        public static readonly Resources upkeepCost = new Food(10);

        public static readonly UnitType unitType = UnitType.TRADER;
        public static readonly UnitClass unitClass = UnitClass.SUPPORT;

#region overrided functions
        public override Unit clone()
        {
            return new Trader(quanity);
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
