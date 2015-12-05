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
            //Logger.turnOff();


            Army attacker = new Army(), defender = new Army();

            attacker.add(new Warrior(1100));
            defender.add(new Warrior(1000));

            Report report = BattleResolver.Fight(attacker, defender);
            Logger.Trace("R1:" + report);
            
            Assert.AreEqual(Report.BattleResult.AttackerVictory, report.result);
            //Assert.IsTrue(report.attackerArmy[UnitType.KNIGHT] == 100);
            //Assert.IsTrue(report.attackerArmy[UnitType.ARCHER] == 5);
            //Assert.IsTrue(report.defenderArmy[UnitType.WARRIOR] == 25);
            //Assert.IsTrue(report.defenderLosses[UnitType.WARRIOR] == 25);

            attacker.add(new Warrior(400));
            defender.add(new Archer(500));
            report = BattleResolver.Fight(attacker, defender);
            Logger.Trace("R2:\n" + report);

            Assert.AreEqual(Report.BattleResult.DefenderVictory, report.result);
            //Assert.IsTrue(report.defenderArmy[UnitType.KNIGHT] == 300);
        }

        [TestMethod]
        public void testBattleReport()
        {
            Logger.turnOff();
            Assert.Inconclusive();
            /*
            Army attacker = new Army(), defender = new Army();

            attacker.add(new Warrior(100)).add(new Archer(250)).add(new Knight(500));
            defender.add(new Warrior(1000)).add(new Archer(100));

            Battle battle = new Battle(attacker, defender, 1f, 1f, 0);

            Logger.log(battle.makeBattle().ToString());
            */
            //Assert.IsTrue(false);
        }

        [TestMethod]
        public void testBattleFormula()
        {
            Logger.turnOff();
            Assert.Inconclusive();
            /*
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
            }*/
        }
    }
}
