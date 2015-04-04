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
    public class Knight : Unit
    {
        public Knight(int quantity = 0)
            : base(quantity)
        {
        }

#region overrided properties
        public override Unit clone()
        {
            return new Knight(quantity);
        }
        
        public override int baseAttackStrength { get { return 40; } }
        public override double chanceOfInjury { get { return 0.3 ; } }

        public override int baseDefenseInfantry { get { return 50; } }
        public override int baseDefenseCavalry { get { return 5; } }
        public override int baseDefenseArchers { get { return 10; } }

        public override int baseMovementSpeed { get { return 2; } }
        public override int baseLootCapacity { get { return 50; } }

        public override Resources baseRecruitCost { get { return new Iron(100) + new Food(50) + new Wood(25); } }

        public override float baseRecruitTime { get { return 15f; } }
        public override Resources baseUpkeepCost { get { return new Iron(1) + new Food(5); } }

        public override UnitType unitType { get { return UnitType.KNIGHT; } }
        public override UnitClass unitClass { get { return UnitClass.CAVALRY; } }
        public override UnitPurpose unitPurpose { get { return UnitPurpose.MILITARY; } }

        public override string name { get { return "Rycerz"; } }

        public override Requirements requirements
        {
            get
            {
                return new Requirements()
                            + new BuildingRequirement(BuildingType.STABLE, 1);
            }
        }
#endregion
    }
}
