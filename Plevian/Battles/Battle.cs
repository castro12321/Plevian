using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Units;

namespace Plevian.Battles
{
    class Battle
    {
        Army attacker, defender;
        private float attackerLuck, defenderDefense;
        Dictionary<UnitType, int> attArmy, defArmy;
        Dictionary<UnitType, int> attLosses, defLosses;

        int attackStrength, defenseInf, defenseCav, defenseArch;
        int attSize, attInf, attCav, attArch;
        int defSize, defInf, defCav, defArch;
        float infP, archP, cavP;

        BattleState battleState = BattleState.Draw;

        public Battle(Army attacker, Army defender, float luck, float defense)
        {
            this.attacker = attacker;
            this.defender = defender;
            attackerLuck = luck;
            defenderDefense = defense;

            listUnits();
        }

        public Report makeBattle()
        {
            if (battleState == BattleState.Error)
                throw new Exception("Battle error");
            if (battleState != BattleState.Draw)
                throw new Exception("You cannot make same battle twice!");
            
            getBasicParams();
            getPercentages();

            int overallDefense = defenseInf + defenseCav + defenseArch;

            if (attackStrength > overallDefense)
            {
                //defender is losing everything
                defender.getUnits().Clear();

                float attackerLosses = 1.0f - (float)(attackStrength - overallDefense) / (float)overallDefense;
                foreach (var pair in attacker.getUnits())
                {
                    float losses = (float)pair.Value.quanity / (float)attSize * attackerLosses;
                    attLosses.Add(pair.Key, (int)(pair.Value.quanity * losses));
                    pair.Value.quanity -= (int)(pair.Value.quanity * losses);

                }
            }
            else
            {
                attacker.getUnits().Clear();

                float defenderLosses = 1.0f - (float)(overallDefense - attackStrength) / (float)attackStrength;
                foreach (var pair in defender.getUnits())
                {
                    float losses = (float)pair.Value.quanity / (float)defSize * defenderLosses;
                    defLosses.Add(pair.Key, (int)(pair.Value.quanity * losses));
                    pair.Value.quanity -= (int)(pair.Value.quanity * losses);
                }
            }
            
            battleState = chooseWinner();
            return new Report(attArmy, defArmy, attLosses, defLosses, attackerLuck, defenderDefense, battleState);
        }

        private void listUnits()
        {
            foreach (var pair in attacker.getUnits())
            {
                attArmy.Add(pair.Key, pair.Value.quanity);
            }

            foreach (var pair in defender.getUnits())
            {
                defArmy.Add(pair.Key, pair.Value.quanity);
            }
        }

        private BattleState chooseWinner()
        {
            if (attacker.size() == 0)
            {
               return BattleState.DefenderVictory;
            }
            else
            {
                return BattleState.AttackerVictory;
            }
        }

        private void getBasicParams()
        {
            attackStrength = attacker.getAttackStrength();
            defenseInf = defender.getDefenseInfantry();
            defenseCav = defender.getDefenseCavalry();
            defenseArch = defender.getDefenseArchers();

            attSize = attacker.size();
            attInf = attacker.getUnitClassCount(UnitClass.INFANTRY);
            attCav = attacker.getUnitClassCount(UnitClass.CAVALRY);
            attArch = attacker.getUnitClassCount(UnitClass.ARCHER);

            defSize = defender.size();
            defInf = defender.getUnitClassCount(UnitClass.INFANTRY);
            defCav = defender.getUnitClassCount(UnitClass.CAVALRY);
            defArch = defender.getUnitClassCount(UnitClass.ARCHER);
        }

        private void getPercentages()
        {
            infP = (float)(attInf / attSize);
            archP = (float)(attArch / attSize);
            cavP = (float)(attCav / attSize);

            defenseInf = (int)(defenseInf * infP);
            defenseCav = (int)(defenseCav * cavP);
            defenseArch = (int)(defenseArch * archP);
        }

    }
}
