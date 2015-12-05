using Plevian.Buildings;
using Plevian.RequirementS;
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
        public Duke(int quantity = 0)
            : base(quantity)
        {
        }


#region overrided properties
        public override Unit clone()
        {
            return new Duke(quantity);
        }

        public override int baseAttackStrength { get { return 75; } }
        public override double chanceOfInjury { get { return 0.6; } }
        
        public override int baseDefenseInfantry { get { return 200; } }
        public override int baseDefenseCavalry { get { return 200; } }
        public override int baseDefenseArchers { get { return 200; } }

        public override int baseMovementSpeed { get { return 3; } }
        public override int baseLootCapacity { get { return 0; } }

        public override Resources baseRecruitCost { get { return new Iron(100) + new Food(50) + new Wood(25); } }

        public override float baseRecruitTime { get { return 60f; } }
        public override Resources baseUpkeepCost { get { return new Iron(1) + new Food(5); } }

        public override UnitType unitType { get { return UnitType.DUKE; } }
        public override UnitClass unitClass { get { return UnitClass.INFANTRY; } }
        public override UnitPurpose unitPurpose { get { return UnitPurpose.MILITARY; } }

        public override string name { get { return "Wojewoda XD?"; } }

        public override Requirements requirements
        {
            get
            {
                return new Requirements()
                            + new BuildingRequirement(BuildingType.BARRACKS, 3) 
                            + new BuildingRequirement(BuildingType.TOWN_HALL, 5);
            }
        }
#endregion

    }
}
