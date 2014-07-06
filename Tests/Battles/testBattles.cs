using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian.Battles;
using Plevian.Units;
namespace Tests.Battles
{
    [TestClass]
    public class testBattles
    {
        [TestMethod]
        public void tastBattleSystem()
        {
            Army attacker = new Army(), defender = new Army();

            attacker += new Knight(100);
            attacker += new Archer(5);

            defender += new Warrior(25);

            Battle battle = new Battle(attacker, defender, 1f, 1f, 0);
            Report report = battle.makeBattle();

            Assert.IsTrue(report.battleResult == BattleState.AttackerVictory);
            Assert.IsTrue(report.attackerArmy[UnitType.KNIGHT] == 100);
            Assert.IsTrue(report.attackerArmy[UnitType.ARCHER] == 5);
            Assert.IsTrue(report.defenderArmy[UnitType.WARRIOR] == 25);
            Assert.IsTrue(report.defenderLosses[UnitType.WARRIOR] == 25);

            defender += new Knight(300);

            battle = new Battle(attacker, defender, 1f, 1f, 0);
            report = battle.makeBattle();

            Assert.IsTrue(report.defenderArmy[UnitType.KNIGHT] == 300);
            Assert.IsTrue(report.battleResult == BattleState.DefenderVictory);
        }


    }
}
