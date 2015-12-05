using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Resource;
using Plevian.RequirementS;
using System.Windows;
using System.ComponentModel;
using Plevian.Players;

namespace Plevian.Units
{
    public abstract class Unit : DependencyObject
    {
        public static readonly DependencyProperty QuantityProperty =
            DependencyProperty.Register("quantity", typeof(int),
            typeof(Unit), new FrameworkPropertyMetadata(0));
        public int quantity
        {
            get { return (int)GetValue(QuantityProperty); }
            set { SetValue(QuantityProperty, value); }
        }

        public Unit(int quantity = 0)
        {
            this.quantity = quantity;
        }

        public Resources getWholeUnitCost()
        {
            return baseRecruitCost * quantity;
        }

#region abstract properties
        public abstract int baseAttackStrength { get; }
        public abstract double chanceOfInjury { get; }
        
        public abstract int baseDefenseInfantry { get; }
        public abstract int baseDefenseCavalry { get; }
        public abstract int baseDefenseArchers { get; }
        //public abstract int getDefenseAgains(UnitType unitType);

        public abstract int baseMovementSpeed { get;  }
        public abstract int baseLootCapacity { get; }

        public abstract Resources baseRecruitCost { get;  }

        public abstract float baseRecruitTime { get; }
        public abstract Resources baseUpkeepCost { get; }

        public abstract UnitType unitType { get; }
        public abstract UnitClass unitClass { get;  }
        public abstract UnitPurpose unitPurpose { get; }

        public abstract Requirements requirements { get;  }
        /// <summary>How important is this unit in the village? The lower the more important. 1 being the most important</summary>
        public virtual int getAiResourceModifier() { return 15; }
        /// <summary>Chance of the unit being recruit (in % where 0=0%; 1=100%)</summary>

        public abstract string name { get; }

        
#endregion

        public override string ToString()
        {
            return Enum.GetName(typeof(UnitType), unitType) + "(" + quantity + ")";
        }

        public abstract Unit clone();

    }
}
