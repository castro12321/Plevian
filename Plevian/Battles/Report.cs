using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Units;
using Plevian.Resource;

namespace Plevian.Battles
{
    public class Report
    {
        public readonly Dictionary<UnitType, int> attackerArmy, defenderArmy,
                                  attackerLosses, defenderLosses;
        public readonly float attackerLuck;
        public readonly float defenderPercentageDefense;
        public readonly BattleState battleResult;
        public Resources loot;
        public bool villageCaptured = false;
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

        public override string ToString()
        {
            string _return = "Luck " + (int)(attackerLuck * 100) + "%\n";
            _return += "\nAttacker\n";
            foreach(var pair in attackerArmy)
            {
                UnitType type = pair.Key;
                string unitName = Enum.GetName(typeof(UnitType), type);
                int losses = 0;
                if(attackerLosses.ContainsKey(type))
                    losses = attackerLosses[type];
                int startQuantity = pair.Value;
                int endQuantity = pair.Value - losses;
                _return += type + " >> " + startQuantity + " - " + losses + " = " + endQuantity + "\n";
            }

            _return += "\nDefender\n";
            foreach (var pair in defenderArmy)
            {
                UnitType type = pair.Key;
                string unitName = Enum.GetName(typeof(UnitType), type);
                int losses = 0;
                if (defenderLosses.ContainsKey(type))
                    losses = defenderLosses[type];
                int startQuantity = pair.Value;
                int endQuantity = pair.Value - losses;
                _return += type + " >> " + startQuantity + " - " + losses + " = " + endQuantity + "\n";
            }

            if( !Object.ReferenceEquals(loot,null) && villageCaptured == false)
            {
                _return += "\nLoot : \n";
                _return += loot.ToString();
            }

            if(villageCaptured)
            {
                _return += "\nVillage has been captured\n";
            }

            return _return;
        }





        
    }
}
