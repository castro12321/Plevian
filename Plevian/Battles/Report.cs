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
        public enum BattleResult
        {
            AttackerVictory,
            DefenderVictory,
            Draw,
            Error,
        }

        public SimpleArmy attackerBefore, attackerLosses, attackerAfter, defenderBefore, defenderLosses, defenderAfter;
        public BattleResult result;
        public Resources loot;
        public bool villageCaptured = false;
        
        public override string ToString()
        {
            String _return = "";

            Dictionary<UnitType, List<Unit>> 
                attackerBeforeByType = attackerBefore.GetUnitsByType(), 
                attackerLossesByType = attackerLosses.GetUnitsByType(),
                defenderBeforeByType = defenderBefore.GetUnitsByType(), 
                defenderLossesByType = defenderLosses.GetUnitsByType();

            //string _return = "Luck " + (int)(attackerLuck * 100) + "%\n";
            _return += "\nAttacker\n";
            foreach(var pair in attackerBeforeByType)
            {
                int before = pair.Value.Count;
                int losses = 0;
                if (attackerLossesByType.ContainsKey(pair.Key))
                    losses = attackerLossesByType[pair.Key].Count;
                _return += pair.Key + " >> " + before + " - " + losses + " = " + (before - losses) + "\n";
            }

            _return += "\nDefender\n";
            foreach (var pair in defenderBeforeByType)
            {
                int before = pair.Value.Count;
                int losses = 0;
                if (defenderLossesByType.ContainsKey(pair.Key))
                    losses = defenderLossesByType[pair.Key].Count;
                _return += pair.Key + " >> " + before + " - " + losses + " = " + (before - losses) + "\n";
            }

            if(loot != null && villageCaptured == false)
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
