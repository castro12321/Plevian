using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Debugging;
using Plevian.Units;

namespace Plevian.Battles
{
    public class BattleResolver
    {
        private static Random random = new Random();
        private SimpleArmy attacker, defender;
        //private int attackerUnitCount, defenderUnitCount; // Extracted values in order to keep turn chance same across the battle
        private Report report;
        
        private BattleResolver(Army attacker, Army defender)
        {
            report = new Report();
            this.attacker = SimpleArmy.FromArmy(attacker);
            this.defender = SimpleArmy.FromArmy(defender);
            report.attackerAfter = this.attacker;
            report.defenderAfter = this.defender;
            report.attackerBefore = report.attackerAfter.Clone();
            report.defenderBefore = report.defenderAfter.Clone();
            report.attackerLosses = new SimpleArmy();
            report.defenderLosses = new SimpleArmy();
            report.result = Report.BattleResult.Error;
        }

        public static Report Fight(Army attacker, Army defender)
        {
            return new BattleResolver(attacker, defender).Fight();
        }

        private Report Fight()
        {
            while(true)
            {
                if(IsFightOver())
                    break;
                FightTick();
            }

            if (attacker.Count == 0)
                report.result = Report.BattleResult.DefenderVictory;
            else
                report.result = Report.BattleResult.AttackerVictory;
            
            return report;
        }
        
        private void FightTick()
        {
            SimpleArmy attacking, defending, losses;
            Unit attackingUnit, defendingUnit;

            // Roll who will be attacking
            // The more units an army have, the more likely they'll get this turn
            // We need to Math.Sqrt() unit counts to make the army size difference less accountable
            // (without Math.Sqrt(), 10000 vs 10000 warriors results in about 60% loss on winner's side)
            // (with Math.Sqrt(), 10000 vs 10000 results in 75% loss on winner's side)
            int attackerUnitCount = (int)Math.Sqrt(attacker.Count);
            int defenderUnitCount = (int)Math.Sqrt(defender.Count);
            if (random.Next(defenderUnitCount + attackerUnitCount) > defenderUnitCount)
            {
                //Logger.Trace("Attacker turn");
                attacking = attacker; // Attacker turn
                defending = defender;
                losses = report.defenderLosses;
            }
            else
            {
                //Logger.Trace("Defender turn");
                attacking = defender; // Defender turn
                defending = attacker;
                losses = report.attackerLosses;
            }
            attackingUnit = attacking[random.Next(attacking.Count)]; 
            defendingUnit = defending[random.Next(defending.Count)];
            //Logger.Trace(" - " + attackingUnit.unitType + " vs " + defendingUnit.unitType);

            // Super secret formula for fighting below.
            const double attackBonus = 1.0d; // Or penalty if below 1.0d
            const double defenseBonus = 1.0d; // Or penalty if below 1.0d

            double attack = attackingUnit.baseAttackStrength * attackBonus;
            double defense = 0;
            switch(attackingUnit.unitClass)
            {
                case UnitClass.SUPPORT:
                case UnitClass.INFANTRY: defense = defendingUnit.baseDefenseInfantry; break;
                case UnitClass.ARCHER: defense = defendingUnit.baseDefenseArchers; break;
                case UnitClass.CAVALRY: defense = defendingUnit.baseDefenseCavalry; break;
            }
            defense *= defenseBonus;

            // Likelihood of being killed.
            // For example:
            //     defense = 18, attack = 36 
            //     an unit have doubled chance of being killed
            //     because it have less (half) defense than attacker has attack
            double attackDefenseRatio = attack / defense;
            //Logger.Trace(" - attack: " + attack + "; defense: " + defense);

            if(random.NextDouble() * attackDefenseRatio > 0.5)
            {
                // Bad luck. He's dead now
                //Logger.Trace(" - DEAD: " + defendingUnit);
                defending.Remove(defendingUnit);
                losses.Add(defendingUnit);
            }
        }

        private bool IsFightOver()
        {
            return attacker.Count == 0
                || defender.Count == 0;
        }

        /*
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
        }*/
    }
}
