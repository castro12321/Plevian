using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Units;

namespace Plevian.Battles
{
    public class Battle
    {
        Army attacker, defender;
        private float attackerLuck, defenderPercentageDefense;
        private int defenderBaseDefense;
        Dictionary<UnitType, int> attArmy = new Dictionary<UnitType, int>();
        Dictionary<UnitType, int> defArmy = new Dictionary<UnitType,int>();
        Dictionary<UnitType, int> attLosses = new Dictionary<UnitType,int>();
        Dictionary<UnitType, int> defLosses = new Dictionary<UnitType,int>();

        int attackStrength, defenseInf, defenseCav, defenseArch;
        int attSize, attInf, attCav, attArch;
        int defSize, defInf, defCav, defArch;
        float infP, archP, cavP;

        BattleState battleState = BattleState.Draw;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="defender"></param>
        /// <param name="luck">1f - normal luck, 0f- Attacker can't attack</param>
        /// <param name="percentageDefense">1f - normal defense, 0f- defender can't defend</param>
        /// <param name="baseDefense">0f - min Value</param>
        public Battle(Army attacker, Army defender, float luck, float percentageDefense, int baseDefense)
        {
            this.attacker = attacker;
            this.defender = defender;
            attackerLuck = luck;
            defenderPercentageDefense = percentageDefense;
            defenderBaseDefense = baseDefense;

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
                foreach (var pair in defender.getUnits())
                {
                    defLosses.Add(pair.Key, pair.Value.quanity);
                }
                defender.getUnits().Clear();

                float attackerLosses = ((float)overallDefense / (float)attackStrength) + ((float)Math.Sqrt(attackStrength)/(float)overallDefense);
                if (attackerLosses > 0f && attackerLosses <= 1f)
                {
                    foreach (var pair in attacker.getUnits())
                    {
                        float losses = (float)pair.Value.quanity / (float)attSize * attackerLosses;
                        attLosses.Add(pair.Key, (int)(pair.Value.quanity * losses));
                        pair.Value.quanity -= (int)(pair.Value.quanity * losses);
                    }
                }
            }
            else
            {
                foreach (var pair in attacker.getUnits())
                {
                    attLosses.Add(pair.Key, pair.Value.quanity);
                }
                attacker.getUnits().Clear();

                float defenderLosses = ((float)attackStrength / (float)overallDefense) + ((float)Math.Sqrt(overallDefense) / (float)attackStrength);
                if (defenderLosses > 0f && defenderLosses <= 1f)
                {
                    foreach (var pair in defender.getUnits())
                    {
                        float losses = (float)pair.Value.quanity / (float)defSize * defenderLosses;
                        defLosses.Add(pair.Key, (int)(pair.Value.quanity * losses));
                        pair.Value.quanity -= (int)(pair.Value.quanity * losses);
                    }
                }
            }
            
            battleState = chooseWinner();
            return new Report(attArmy, defArmy, attLosses, defLosses, attackerLuck, defenderPercentageDefense, battleState);
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
            attackStrength = (int)((float)attackStrength * attackerLuck);


            defenseInf = (int)(((float)defender.getDefenseInfantry() + defenderBaseDefense ) * (defenderPercentageDefense));
            defenseCav = (int)(((float)defender.getDefenseInfantry() + defenderBaseDefense) * (defenderPercentageDefense));
            defenseArch = (int)(((float)defender.getDefenseInfantry() + defenderBaseDefense) * (defenderPercentageDefense));

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
            infP = ((float)attInf / (float)attSize);
            archP = ((float)attArch / (float)attSize);
            cavP = ((float)attCav / (float)attSize);

            defenseInf = (int)((float)defenseInf * infP);
            defenseCav = (int)((float)defenseCav * cavP);
            defenseArch = (int)((float)defenseArch * archP);
        }

    }
}
