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

        public override int attackStrength { get { return 10; } }

        public override int defenseInfantry { get { return 25; } }
        public override int defenseCavalry { get { return 4; } }
        public override int defenseArchers { get { return 5; } }

        public override int movementSpeed { get { return 1; } }
        public override int lootCapacity { get { return 75; } }

        public override Resources recruitCost { get { return new Food(50) + new Wood(50); } }

        public override float recruitTime { get { return 5.0f; } }
        public override Resources upkeepCost { get { return new Food(5); } }

        public override UnitType unitType { get { return UnitType.WARRIOR; } }
        public override UnitClass unitClass { get { return UnitClass.INFANTRY; } }

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
