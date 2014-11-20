using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian.Battles;
using Plevian.Units;
using Plevian.Debugging;
using Plevian.Resource;
namespace Tests.Battles
{
    [TestClass]
    public class TestBattles
    {
        [TestMethod]
        public void tastBattleSystem()
        {
            Logger.turnOff();

            Army attacker = new Army(), defender = new Army();

            attacker.add(new Knight(100)).add(new Archer(5));
            defender.add(new Warrior(25));

            Battle battle = new Battle(attacker, defender, 1f, 1f, 0);
            Report report = battle.makeBattle();
            
            Assert.IsTrue(report.battleResult == BattleState.AttackerVictory);
            Assert.IsTrue(report.attackerArmy[UnitType.KNIGHT] == 100);
            Assert.IsTrue(report.attackerArmy[UnitType.ARCHER] == 5);
            Assert.IsTrue(report.defenderArmy[UnitType.WARRIOR] == 25);
            Assert.IsTrue(report.defenderLosses[UnitType.WARRIOR] == 25);

            defender.add(new Knight(300));

            battle = new Battle(attacker, defender, 1f, 1f, 0);
            report = battle.makeBattle();

            Assert.IsTrue(report.defenderArmy[UnitType.KNIGHT] == 300);
            Assert.IsTrue(report.battleResult == BattleState.DefenderVictory);
        }

        [TestMethod]
        public void testBattleReport()
        {
            Logger.turnOff();

            Army attacker = new Army(), defender = new Army();

            attacker.add(new Warrior(100)).add(new Archer(250)).add(new Knight(500));
            defender.add(new Warrior(1000)).add(new Archer(100));

            Battle battle = new Battle(attacker, defender, 1f, 1f, 0);

            Logger.log(battle.makeBattle().ToString());
            //Assert.IsTrue(false);
        }

        [TestMethod]
        public void testBattleFormula()
        {
            Logger.turnOff();
            Army attacker = new Army(), defender = new Army();

            defender.add(new Warrior(1000));
            int losses = -1;
            for(int i = 0;defender.size() > 0;++i)
            {
                attacker = new Army();
                attacker.add(new Knight(100));

                Battle battle = new Battle(attacker, defender, 1f, 1f, 0);
                Report report = battle.makeBattle();

                int current_losses = report.defenderLosses[UnitType.WARRIOR];
                if(losses == -1)
                {
                    losses = current_losses;
                }
                else
                {
                    Logger.log("losess = " + losses + ", current - " + current_losses);
                    Assert.IsTrue(current_losses >= losses || current_losses == report.defenderArmy[UnitType.WARRIOR]);
                    losses = current_losses;
                }
            }
        }
    }
}
