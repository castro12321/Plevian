using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;

namespace Plevian.Units
{
    public class Knight : Unit
    {
        public Knight(int quanity = 0)
            : base(quanity)
        {
        }

        public static readonly int attackStrength = 20;

        public static readonly int defenseOnInfantry = 50;
        public static readonly int defenseOnCavalry = 5;
        public static readonly int defenseOnArchers = 10;

        public static readonly int movementSpeed = 2;
        public static readonly int lootCapacity = 50;

        public static readonly float recruitTime = 15.8f;

        public static readonly Resources recruitCost = new Iron(100) + new Food(50) + new Wood(25);
        public static readonly Resources upkeepCost = new Iron(1) + new Food(5);

        public static readonly UnitType unitType = UnitType.KNIGHT;
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
