﻿using System;
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

        public override int baseAttackStrength { get { return 50; } }
        public override double chanceOfInjury { get { return 0.6; } }

        public override int baseDefenseInfantry { get { return 50; } }
        public override int baseDefenseCavalry { get { return 50; } }
        public override int baseDefenseArchers { get { return 50; } }

        public override int baseMovementSpeed { get { return 3; } }
        public override int baseLootCapacity { get { return 50; } }

        public override Resources baseRecruitCost { get { return new Food(300) + new Wood(300) + new Iron(100) + new Stone(300); } }

        public override float baseRecruitTime { get { return 30f; } }
        public override Resources baseUpkeepCost { get { return new Food(10); } }

        public override UnitType unitType { get { return UnitType.SETTLER; } }
        public override UnitClass unitClass { get { return UnitClass.SUPPORT; } }
        public override UnitPurpose unitPurpose { get { return UnitPurpose.ECONOMIC; } }

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
