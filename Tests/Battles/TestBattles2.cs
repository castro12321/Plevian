using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian.Battles;
using Plevian.Units;
using Plevian.Resource;
using Plevian.Debugging;
using System.Diagnostics;
using System.Collections.Generic;

namespace Tests.Battles
{
    [TestClass]
    public class TestBattles2
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.Inconclusive();
            /*
            Army attacker = new Army();
            Army defender = new Army();

            attacker.add(new Knight(19));
            defender.add(new Knight(20));

            Battle battle = new Battle(attacker, defender, 0, 1f, 0);

            Report report = battle.makeBattle();

            //check quantity of army
            Debug.WriteLine("***************************************");

            Debug.WriteLine("***************ATTACKER****************");
            Debug.WriteLine("****************before*****************");
            foreach (KeyValuePair<UnitType, int> army in report.attackerArmy)
            {
                Debug.WriteLine("Key = " + army.Key.ToString() + " Value = " + army.Value.ToString());
            }

            Debug.WriteLine("****************after*****************");
            foreach (KeyValuePair<UnitType, int> army in report.attackerLosses)
            {
                Debug.WriteLine("Key = " + army.Key.ToString() + " Value = " + army.Value.ToString());
            }

            Debug.WriteLine("****************DEFENDER***************");
            Debug.WriteLine("****************before*****************");
            foreach (KeyValuePair<UnitType, int> army in report.defenderArmy)
            {
                Debug.WriteLine("Key = " + army.Key.ToString() + " Value = " + army.Value.ToString());
            }

            Debug.WriteLine("****************after*****************");
            foreach (KeyValuePair<UnitType, int> army in report.defenderLosses)
            {
                Debug.WriteLine("Key = " + army.Key.ToString() + " Value = " + army.Value.ToString());
            }

            Debug.WriteLine("Battle result: " + report.battleResult.ToString());
            Debug.WriteLine("Attacker luck: " + report.attackerLuck.ToString());
            Debug.WriteLine("***************************************");
            */
            /*Assert.IsTrue(report.battleResult == BattleState.AttackerVictory);
            Assert.IsTrue(report.attackerLuck == 0);*/
        }
    }
}
