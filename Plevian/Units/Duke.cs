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
        public override double chanceOfInjury { get { return 0.6; } }
        public override int attackStrength { get { return 100; } }

        public override int defenseInfantry { get { return 150; } }
        public override int defenseCavalry { get { return 50; } }
        public override int defenseArchers { get { return 100; } }

        public override int movementSpeed { get { return 3; } }
        public override int lootCapacity { get { return 0; } }

        public override Resources recruitCost { get { return new Iron(100) + new Food(50) + new Wood(25); } }

        public override float recruitTime { get { return 60f; } }
        public override Resources upkeepCost { get { return new Iron(1) + new Food(5); } }

        public override UnitType unitType { get { return UnitType.DUKE; } }
        public override UnitClass unitClass { get { return UnitClass.INFANTRY; } }

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
