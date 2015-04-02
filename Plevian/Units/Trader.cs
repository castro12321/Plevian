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
    public class Trader : Unit
    {
        public Trader(int quantity = 0)
            : base(quantity)
        {
        }

#region overrided properties
        public override Unit clone()
        {
            return new Trader(quantity);
        }

        public override int baseAttackStrength { get { return 5; } }

        public override int baseDefenseInfantry { get { return 1; } }
        public override int baseDefenseCavalry { get { return 1; } }
        public override int baseDefenseArchers { get { return 1; } }

        public override int baseMovementSpeed { get { return 3; } }
        public override int baseLootCapacity { get { return 500; } }

        public override Resources baseRecruitCost { get { return new Food(500) + new Wood(150); } }

        public override float baseRecruitTime { get { return 30.0f; } }
        public override Resources baseUpkeepCost { get { return new Food(10); } }

        public override UnitType unitType { get { return UnitType.TRADER; } }
        public override UnitClass unitClass { get { return UnitClass.SUPPORT; } }
        public override UnitPurpose unitPurpose { get { return UnitPurpose.ECONOMIC; } }

        public override string name { get { return "Handlarz"; } }

        public override Requirements requirements
        {
            get
            {
                return new Requirements()
                            + new BuildingRequirement(BuildingType.TOWN_HALL, 2);
            }
        }
#endregion
    }
}
