using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian.Units;
using Plevian;
using Plevian.Villages;
using Plevian.Properties;
namespace Tests.Unit
{
    [TestClass]
    public class TestRecruit
    {
        [TestMethod]
        public void testRecruit()
        {
            Archer archer = new Archer(25);
            Game game = new Game();
            Village testVillage = new Village();
            testVillage.recruit(archer);
            testVillage.recruit(new Knight(100));
            testVillage.recruit(new Warrior(50));
            GameTime now = GameTime.now;

            Seconds wait = testVillage.recruitTimeEnd - now;
            wait.seconds++;
            int seconds = wait.seconds;
            while (wait.seconds > 0)
            {
                wait.seconds--;
                testVillage.tick();
            }
            System.Threading.Thread.Sleep((seconds) * 1000);
            GameTime.update();
            
            Assert.IsTrue(testVillage.army.get(UnitType.ARCHER).quanity == 25);
            Assert.IsTrue(testVillage.army.get(UnitType.KNIGHT).quanity == 100);
            Assert.IsTrue(testVillage.army.get(UnitType.WARRIOR).quanity == 50);
        }
    }
}
