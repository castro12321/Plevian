using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Units;

namespace Plevian.Battles
{
    class Report
    {
        Dictionary<UnitType, int> attackerArmy, defenderArmy,
                                  attackerLosses, defenderLosses;
        float attackerLuck;
        float defenderDefense;

        public Report(Dictionary<UnitType, int> atA, Dictionary<UnitType, int> defA, Dictionary<UnitType, int> atL, Dictionary<UnitType, int> defL, float luck, float defense)
        {
            attackerArmy = atA;
            defenderArmy = defA;
            attackerLosses = atL;
            defenderLosses = defL;

            attackerLuck = luck;
            defenderDefense = defense;
        }

        public string toString()
        {
            return "Not implemented";
        }





        
    }
}
