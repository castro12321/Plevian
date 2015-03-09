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
    public class Trader : Unit
    {
        public Trader(int quantity = 0)
            : base(quantity)
        {
        }

#region overrided properties
        public override Unit clone()
        {
            return new Trader(quantity);
        }

        public override double chanceOfInjury { get { return 0.6; } }
        public override int attackStrength { get { return 5; } }

        public override int defenseInfantry { get { return 1; } }
        public override int defenseCavalry { get { return 1; } }
        public override int defenseArchers { get { return 1; } }

        public override int movementSpeed { get { return 3; } }
        public override int lootCapacity { get { return 500; } }

        public override Resources recruitCost { get { return new Food(500) + new Wood(150); } }

        public override float recruitTime { get { return 30.0f; } }
        public override Resources upkeepCost { get { return new Food(10); } }

        public override UnitType unitType { get { return UnitType.TRADER; } }
        public override UnitClass unitClass { get { return UnitClass.SUPPORT; } }

        public override string name { get { return "Handlarz"; } }

        public override Requirements requirements
        {
            get
            {
                return new Requirements()
                            + new BuildingRequirement(BuildingType.TOWN_HALL, 2);
            }
        }
#endregion
    }
}
