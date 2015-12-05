using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.Buildings;
using Plevian.RequirementS;

namespace Plevian.Units
{
    public class Warrior : Unit
    {
        public Warrior(int quantity = 0)
            : base(quantity)
        {
        }


#region overrided functions
        public override Unit clone()
        {
            return new Warrior(quantity);
        }

        public override int baseAttackStrength { get { return 100; } }
        public override double chanceOfInjury { get { return 0.6; } }

        public override int baseDefenseInfantry { get { return 100; } }
        public override int baseDefenseCavalry { get { return 200; } }
        public override int baseDefenseArchers { get { return 50; } }

        public override int baseMovementSpeed { get { return 1; } }
        public override int baseLootCapacity { get { return 50; } }

        public override Resources baseRecruitCost { get { return new Food(50) + new Wood(50); } }

        public override float baseRecruitTime { get { return 15.0f; } }
        public override Resources baseUpkeepCost { get { return new Food(5); } }

        public override UnitType unitType { get { return UnitType.WARRIOR; } }
        public override UnitClass unitClass { get { return UnitClass.INFANTRY; } }
        public override UnitPurpose unitPurpose { get { return UnitPurpose.MILITARY; } }

        public override string name { get { return "Wojownik"; } }

        public override Requirements requirements
        {
            get
            {
                return new Requirements();
            }
        }
#endregion
    }
}
