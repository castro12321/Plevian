using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.RequirementS;

namespace Plevian.Units
{
    public abstract class Unit
    {
        public int quanity;

        public Unit(int quanity = 0)
        {
            this.quanity = quanity;
        }

        public Resources getWholeUnitCost()
        {
            return recruitCost * quanity;
        }

#region abstract properties
        public abstract int attackStrength { get; }
        
        public abstract int defenseInfantry { get; }
        public abstract int defenseCavalry { get; }
        public abstract int defenseArchers { get; }

        public abstract int movementSpeed { get;  }
        public abstract int lootCapacity { get; }

        public abstract Resources recruitCost { get;  }

        public abstract float recruitTime { get; }
        public abstract Resources upkeepCost { get; }

        public abstract UnitType unitType { get; }
        public abstract UnitClass unitClass { get;  }

        public abstract Requirements requirements { get;  }

        public abstract string name { get; }

        
#endregion

        public override string ToString()
        {
            return Enum.GetName(typeof(UnitType), unitType) + "(" + quanity + ")";
        }

        public abstract Unit clone();

    }
}
