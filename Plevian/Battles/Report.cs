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
        BattleState battleResult;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="atA">Attacking army</param>
        /// <param name="defA">Defending army</param>
        /// <param name="atL">Attacker's losses</param>
        /// <param name="defL">Defender's losses</param>
        /// <param name="luck">luck</param>
        /// <param name="defense">defense</param>
        /// <param name="result">result</param>
        public Report(Dictionary<UnitType, int> atA, Dictionary<UnitType, int> defA, Dictionary<UnitType, int> atL, Dictionary<UnitType, int> defL, float luck, float defense, BattleState result)
        {
            attackerArmy = atA;
            defenderArmy = defA;
            attackerLosses = atL;
            defenderLosses = defL;

            attackerLuck = luck;
            defenderDefense = defense;
            battleResult = result;
        }

        public string toString()
        {
            return "Not implemented";
        }





        
    }
}
