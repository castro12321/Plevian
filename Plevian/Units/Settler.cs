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
    public class Settler : Unit
    {
        public Settler(int quantity = 0)
            : base(quantity)
        {
        }

#region overrided properties
        public override Unit clone()
        {
            return new Settler(quantity);
        }

        public override double chanceOfInjury { get { return 0.6; } }
        public override int attackStrength { get { return 5; } }

        public override int defenseInfantry { get { return 1; } }
        public override int defenseCavalry { get { return 1; } }
        public override int defenseArchers { get { return 1; } }

        public override int movementSpeed { get { return 3; } }
        public override int lootCapacity { get { return 50; } }

        public override Resources recruitCost { get { return new Food(300) + new Wood(300) + new Iron(100) + new Stone(300); } }

        public override float recruitTime { get { return 30f; } }
        public override Resources upkeepCost { get { return new Food(10); } }

        public override UnitType unitType { get { return UnitType.SETTLER; } }
        public override UnitClass unitClass { get { return UnitClass.SUPPORT; } }

        public override string name { get { return "Osadnik"; } }

        public override Requirements requirements
        {
            get
            {
                return new Requirements()
                            + new BuildingRequirement(BuildingType.TOWN_HALL, 3);
            }
        }
#endregion
    }
}
