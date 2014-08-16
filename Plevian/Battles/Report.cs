using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Units;

namespace Plevian.Battles
{
    public class Report
    {
        public readonly Dictionary<UnitType, int> attackerArmy, defenderArmy,
                                  attackerLosses, defenderLosses;
        public readonly float attackerLuck;
        public readonly float defenderPercentageDefense;
        public readonly BattleState battleResult;
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
        public Report(Dictionary<UnitType, int> atA, Dictionary<UnitType, int> defA, Dictionary<UnitType, int> atL, Dictionary<UnitType, int> defL, float luck, float defencePercentage, BattleState result)
        {
            attackerArmy = atA;
            defenderArmy = defA;
            attackerLosses = atL;
            defenderLosses = defL;

            attackerLuck = luck;
            defenderPercentageDefense = defencePercentage;
            battleResult = result;
        }

        public string toString()
        {
            string stringAttacker = "Attacker\n";
            foreach(var pair in attackerArmy)
            {
                UnitType type = pair.Key;

                //int startQuanity = pair.Value.
            }
            return "Not implemented";
        }





        
    }
}
