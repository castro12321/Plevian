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
    public class Ram : Unit
    {
        public Ram(int quanity = 0)
            : base(quanity)
        {
        }

#region overrided properties
        public override Unit clone()
        {
            return new Ram(quanity);
        }

        public override int attackStrength { get { return 1; } }

        public override int defenseInfantry { get { return 2; } }
        public override int defenseCavalry { get { return 2; } }
        public override int defenseArchers { get { return 5; } }

        public override int movementSpeed { get { return 1; } }
        public override int lootCapacity { get { return 0; } }

        public override Resources recruitCost { get { return new Food(100) + new Wood(500); } }

        public override float recruitTime { get { return 15f; } }
        public override Resources upkeepCost { get { return new Wood(50) + new Food(15); } }

        public override UnitType unitType { get { return UnitType.RAM; } }
        public override UnitClass unitClass { get { return UnitClass.SUPPORT; } }

        public override string name { get { return "Taran"; } }

        public override Requirements requirements
        {
            get
            {
                return new Requirements()
                            + new BuildingRequirement(BuildingType.WORKSHOP, 1);
            }
        }
#endregion
    }
}
