using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.RequirementS;
using Plevian.Buildings;

namespace Plevian.Units
{
    public class Ram : Unit
    {
        public Ram(int quanity = 0)
            : base(quanity)
        {
        }

        public static readonly int attackStrength = 1;

        public static readonly int defenseOnInfantry = 2;
        public static readonly int defenseOnCavalry = 2;
        public static readonly int defenseOnArchers = 5;

        public static readonly int movementSpeed = 1;
        public static readonly int lootCapacity = 0;

        public static readonly float recruitTime = 15.0f;

        public static readonly Resources recruitCost = new Food(100) + new Wood(500);
        public static readonly Resources upkeepCost = new Wood(50) + new Food(15);

        public static readonly UnitType unitType = UnitType.RAM;
        public static readonly UnitClass unitClass = UnitClass.SUPPORT;

#region overrided functions
        public override Unit clone()
        {
            return new Ram(quanity);
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

        public override Requirements getRequirements()
        {
            return new Requirements()
                + new BuildingRequirement(BuildingType.WORKSHOP, 1);
        }
#endregion
    }
}
