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

        //int attackStrength, defenseInf, defenseCav, defenseArch;
        int attInfSize, attArchSize, attCavSize;
        int defInfSize, defArchSize, defCavSize;

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
            armySize();
            countAttack();
            countDefense();
        }

        public Report makeBattle()
        {
            if (battleState == BattleState.Error)
                throw new Exception("Battle error");
            if (battleState != BattleState.Draw)
                throw new Exception("You cannot make same battle twice!");

            if (defSize < attSize)  //nie uwzglednilem obrony miasta, mur itp.
            {
                foreach (var pair in defender.getUnits())
                {
                    defLosses.Add(pair.Key, pair.Value.quantity);
                }
                defender.clear();

                int i = 0;
                foreach (var pair in attacker.getUnits())
                {
                    if (attInf < defInf && pair.Key == UnitType.WARRIOR)
                    {
                        int killInf = Convert.ToInt32((pair.Value.chanceOfInjury + 0.1) * attInfSize);
                        attLosses.Add(pair.Key, killInf);
                        pair.Value.quantity -= killInf;
                    }
                    else if (pair.Key == UnitType.WARRIOR)
                    {
                        int killInf = Convert.ToInt32(pair.Value.chanceOfInjury * attInfSize);
                        attLosses.Add(pair.Key, killInf);
                        pair.Value.quantity -= killInf;
                    }

                    if (attCav < defCav && pair.Key == UnitType.KNIGHT)
                    {
                        int killCav = Convert.ToInt32((pair.Value.chanceOfInjury + 0.1) * attCavSize);
                        attLosses.Add(pair.Key, killCav);
                        pair.Value.quantity -= killCav;
                    }
                    else if (pair.Key == UnitType.KNIGHT)
                    {
                        int killCav = Convert.ToInt32(pair.Value.chanceOfInjury * attCavSize);
                        attLosses.Add(pair.Key, killCav);
                        pair.Value.quantity -= killCav;
                    }

                    if (attArch < defArch && pair.Key == UnitType.ARCHER)
                    {
                        int killArch = Convert.ToInt32((pair.Value.chanceOfInjury + 0.1) * attArchSize);
                        attLosses.Add(pair.Key, killArch);
                        pair.Value.quantity -= killArch;
                    }
                    else if (pair.Key == UnitType.ARCHER)
                    {
                        int killArch = Convert.ToInt32(pair.Value.chanceOfInjury * attArchSize);
                        attLosses.Add(pair.Key, killArch);
                        pair.Value.quantity -= killArch;
                    }
                }
            }
            else
            {
                foreach (var pair in attacker.getUnits())
                {
                    attLosses.Add(pair.Key, pair.Value.quantity);
                }
                attacker.clear();

                foreach (var pair in defender.getUnits())
                {
                    if (defInf < attInf && pair.Key == UnitType.WARRIOR)
                    {
                        int killInf = Convert.ToInt32((pair.Value.chanceOfInjury + 0.1) * defInfSize);
                        defLosses.Add(pair.Key, killInf);
                        pair.Value.quantity -= killInf;
                    }
                    else if (pair.Key == UnitType.WARRIOR)
                    {
                        int killInf = Convert.ToInt32(pair.Value.chanceOfInjury * defInfSize);
                        defLosses.Add(pair.Key, killInf);
                        pair.Value.quantity -= killInf;
                    }

                    if (defCav < attCav && pair.Key == UnitType.KNIGHT)
                    {
                        int killCav = Convert.ToInt32((pair.Value.chanceOfInjury + 0.1) * defCavSize);
                        defLosses.Add(pair.Key, killCav);
                        pair.Value.quantity -= killCav;
                    }
                    else if (pair.Key == UnitType.KNIGHT)
                    {
                        int killCav = Convert.ToInt32(pair.Value.chanceOfInjury * defCavSize);
                        defLosses.Add(pair.Key, killCav);
                        pair.Value.quantity -= killCav;
                    }

                    if (defArch < attArch && pair.Key == UnitType.ARCHER)
                    {
                        int killArch = Convert.ToInt32((pair.Value.chanceOfInjury + 0.1) * defArchSize);
                        defLosses.Add(pair.Key, killArch);
                        pair.Value.quantity -= killArch;
                    }
                    else if (pair.Key == UnitType.ARCHER)
                    {
                        int killArch = Convert.ToInt32(pair.Value.chanceOfInjury * defArchSize);
                        defLosses.Add(pair.Key, killArch);
                        pair.Value.quantity -= killArch;
                    }
                }
            }

            battleState = chooseWinner();
            return new Report(attArmy, defArmy, attLosses, defLosses, defenderPercentageDefense, battleState);
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

        private void listUnits() //pobiera jednostki do listy
        {
            foreach (var pair in attacker.getUnits())
            {
                attArmy.Add(pair.Key, pair.Value.quantity);
            }

            foreach (var pair in defender.getUnits())
            {
                defArmy.Add(pair.Key, pair.Value.quantity);
            }
        }

        private void armySize()
        {
            foreach (var pair in attArmy)
            {
                switch (pair.Key)
                {
                    case UnitType.ARCHER: attArchSize += pair.Value; break;
                    case UnitType.KNIGHT: attCavSize += pair.Value; break;
                    case UnitType.WARRIOR: attInfSize += pair.Value; break;
                }
            }

            foreach (var pair in defArmy)
            {
                switch (pair.Key)
                {
                    case UnitType.ARCHER: defArchSize += pair.Value; break;
                    case UnitType.KNIGHT: defCavSize += pair.Value; break;
                    case UnitType.WARRIOR: defInfSize += pair.Value; break;
                }
            }
        }

        private void countAttack()
        {
            foreach (var pair in attacker.getUnits())
            {
                switch (pair.Key)
                {
                    case UnitType.ARCHER: attArch += pair.Value.baseAttackStrength * pair.Value.quantity; break;
                    case UnitType.KNIGHT: attCav += pair.Value.baseAttackStrength * pair.Value.quantity; break;
                    case UnitType.WARRIOR: attInf += pair.Value.baseAttackStrength * pair.Value.quantity; break;
                }
            }

            attSize = attArch + attCav + attInf;
        }

        private void countDefense()
        {
            foreach (var pair in defender.getUnits())
            {
                defArch += pair.Value.baseDefenseArchers * pair.Value.quantity;  //łucznicy
                defCav += pair.Value.baseDefenseCavalry * pair.Value.quantity;  //kawaleria
                defInf += pair.Value.baseDefenseInfantry * pair.Value.quantity; //piechaota
            }

            defSize = defArch + defCav + defInf;
        }

        /*public Report makeBattle() // tworzy bitwe -> attackOrder class
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
                    defLosses.Add(pair.Key, pair.Value.quantity);
                }
                defender.clear();

                float attackerLosses = ((float)overallDefense / (float)attackStrength) - ((float)Math.Sqrt(attackStrength)/(float)overallDefense);
                if (attackerLosses > 0f && attackerLosses <= 1f)
                {
                    foreach (var pair in attacker.getUnits())
                    {
                        float losses = (float)pair.Value.quantity / (float)attSize * attackerLosses;
                        attLosses.Add(pair.Key, (int)(pair.Value.quantity * losses));
                        pair.Value.quantity -= (int)(pair.Value.quantity * losses);
                    }
                }

            }
            else
            {
                foreach (var pair in attacker.getUnits())
                {
                    attLosses.Add(pair.Key, pair.Value.quantity);
                }
                attacker.clear();

                float defenderLosses = ((float)attackStrength / (float)overallDefense) - ((float)Math.Sqrt(overallDefense) / (float)attackStrength);
                if (defenderLosses > 0f && defenderLosses <= 1f)
                {
                    foreach (var pair in defender.getUnits())
                    {
                        float losses = (float)pair.Value.quantity / (float)defSize * defenderLosses;
                        defLosses.Add(pair.Key, (int)(pair.Value.quantity * losses));
                        pair.Value.quantity -= (int)(pair.Value.quantity * losses);
                    }
                }
            }
            
            battleState = chooseWinner();
            return new Report(attArmy, defArmy, attLosses, defLosses, attackerLuck, defenderPercentageDefense, battleState);
        }

        private void listUnits() //pobiera jednostki do listy
        {
            foreach (var pair in attacker.getUnits())
            {
                attArmy.Add(pair.Key, pair.Value.quantity);
            }

            foreach (var pair in defender.getUnits())
            {
                defArmy.Add(pair.Key, pair.Value.quantity);
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
/*
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
        }*/
    }
}
