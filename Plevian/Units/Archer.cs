using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.RequirementS;
using Plevian.Buildings;
using Plevian.TechnologY;

namespace Plevian.Units
{
    public class Archer : Unit
    {
        public Archer(int quantity = 0)
            : base(quantity)
        {
        }



#region overrided properties
        public override Unit clone()
        {
            return new Archer(quantity);
        }

        public override int baseAttackStrength { get { return 100; } }
        public override double chanceOfInjury { get { return 0.25; } }

        public override int baseDefenseInfantry { get { return 100; } }
        public override int baseDefenseCavalry { get { return 50; } }
        public override int baseDefenseArchers { get { return 200; } }

        public override int baseMovementSpeed { get { return 1; } }
        public override int baseLootCapacity { get { return 20; } }

        public override Resources baseRecruitCost { get { return new Food(40) + new Wood(100); } }

        public override float baseRecruitTime { get { return 10f; } }
        public override Resources baseUpkeepCost { get { return new Wood(1) + new Food(5); } }

        public override UnitType unitType { get { return UnitType.ARCHER; } }
        public override UnitClass unitClass { get { return UnitClass.ARCHER; } }
        public override UnitPurpose unitPurpose { get { return UnitPurpose.MILITARY; } }

        public override string name { get { return "Lucznik"; } }

        public override Requirements requirements
        {
            get
            {
                return new Requirements()
                            + new BuildingRequirement(BuildingType.BARRACKS, 2);
            }
        }
#endregion
    }
}
