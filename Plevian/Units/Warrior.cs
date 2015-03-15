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

        public override int baseAttackStrength { get { return 10; } }

        public override int baseDefenseInfantry { get { return 25; } }
        public override int baseDefenseCavalry { get { return 4; } }
        public override int baseDefenseArchers { get { return 5; } }

        public override int baseMovementSpeed { get { return 1; } }
        public override int baseLootCapacity { get { return 50; } }

        public override Resources baseRecruitCost { get { return new Food(50) + new Wood(50); } }

        public override float baseRecruitTime { get { return 15.0f; } }
        public override Resources baseUpkeepCost { get { return new Food(5); } }

        public override UnitType unitType { get { return UnitType.WARRIOR; } }
        public override UnitClass unitClass { get { return UnitClass.INFANTRY; } }

        public override double getAiImportance() { return 0.8d; }

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
