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

        public override int baseAttackStrength { get { return 35; } }

        public override int baseDefenseInfantry { get { return 5; } }
        public override int baseDefenseCavalry { get { return 10; } }
        public override int baseDefenseArchers { get { return 20; } }

        public override int baseMovementSpeed { get { return 1; } }
        public override int baseLootCapacity { get { return 20; } }

        public override Resources baseRecruitCost { get { return new Food(40) + new Wood(100); } }

        public override float baseRecruitTime { get { return 10f; } }
        public override Resources baseUpkeepCost { get { return new Wood(1) + new Food(5); } }

        public override UnitType unitType { get { return UnitType.ARCHER; } }
        public override UnitClass unitClass { get { return UnitClass.ARCHER; } }

        public override double getAiImportance() { return 0.7d; }

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
