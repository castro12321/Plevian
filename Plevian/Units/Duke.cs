using Plevian.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian.Units
{
    public class Duke : Unit
    {
        public Duke(int quanity = 0)
            : base(quanity)
        {
        }

        public static readonly int attackStrength = 100;

        public static readonly int defenseOnInfantry = 150;
        public static readonly int defenseOnCavalry = 50;
        public static readonly int defenseOnArchers = 100;

        public static readonly int movementSpeed = 3;
        public static readonly int lootCapacity = 0;

        public static readonly float recruitTime = 60f;

        public static readonly Resources recruitCost = new Iron(100) + new Food(50) + new Wood(25);
        public static readonly Resources upkeepCost = new Iron(1) + new Food(5);

        public static readonly UnitType unitType = UnitType.DUKE;
        public static readonly UnitClass unitClass = UnitClass.INFANTRY;


#region overrided functions
        public override Unit clone()
        {
            return new Knight(quanity);
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
