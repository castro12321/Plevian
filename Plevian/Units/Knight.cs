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

        public override double chanceOfInjury { get { return 0.3 ; } }
        public override int attackStrength { get { return 40; } }

        public override int defenseInfantry { get { return 50; } }
        public override int defenseCavalry { get { return 5; } }
        public override int defenseArchers { get { return 10; } }

        public override int movementSpeed { get { return 2; } }
        public override int lootCapacity { get { return 50; } }

        public override Resources recruitCost { get { return new Iron(100) + new Food(50) + new Wood(25); } }

        public override float recruitTime { get { return 15f; } }
        public override Resources upkeepCost { get { return new Iron(1) + new Food(5); } }

        public override UnitType unitType { get { return UnitType.KNIGHT; } }
        public override UnitClass unitClass { get { return UnitClass.CAVALRY; } }

        public override double getAiImportance() { return 0.5d; }

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
