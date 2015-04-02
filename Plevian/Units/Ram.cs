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
        public Ram(int quantity = 0)
            : base(quantity)
        {
        }

#region overrided properties
        public override Unit clone()
        {
            return new Ram(quantity);
        }

        public override int baseAttackStrength { get { return 1; } }

        public override int baseDefenseInfantry { get { return 2; } }
        public override int baseDefenseCavalry { get { return 2; } }
        public override int baseDefenseArchers { get { return 5; } }

        public override int baseMovementSpeed { get { return 1; } }
        public override int baseLootCapacity { get { return 0; } }

        public override Resources baseRecruitCost { get { return new Food(100) + new Wood(500); } }

        public override float baseRecruitTime { get { return 15f; } }
        public override Resources baseUpkeepCost { get { return new Wood(50) + new Food(15); } }

        public override UnitType unitType { get { return UnitType.RAM; } }
        public override UnitClass unitClass { get { return UnitClass.SUPPORT; } }
        public override UnitPurpose unitPurpose { get { return UnitPurpose.MILITARY; } }

        public override string name { get { return "Taran"; } }

        public override Requirements requirements
        {
            get
            {
                return new Requirements()
                            + new BuildingRequirement(BuildingType.WORKSHOP, 1);
            }
        }
#endregion
    }
}
