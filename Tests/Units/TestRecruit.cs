using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian.Units;
using Plevian;
using Plevian.Villages;
using Plevian.Resource;
using Plevian.Debugging;
using Plevian.Players;
using SFML.Graphics;

namespace Tests.Units
{
    [TestClass]
    public class TestRecruit : TestWithTime
    {
        [TestMethod]
        public void testRecruit()
        {
            const int ARCH_QUANTITY = 500;
            const int KNIG_QUANTITY = 500;
            const int WARR_QUANTITY = 500;

            Archer archer = new Archer(ARCH_QUANTITY);
            Game game = new Game();
            Village testVillage = new Village(null, new Player("", Color.Blue), "test");
            testVillage.addResources(new Resources(1000000, 1000000, 1000000, 1000000));
            testVillage.recruit(archer);
            testVillage.recruit(new Knight(KNIG_QUANTITY));
            testVillage.recruit(new Warrior(WARR_QUANTITY));

            fakeTime(0);

            GameTime wait = testVillage.recruitTimeEnd.diffrence(GameTime.now);
            addFakeTime(wait.time);
            GameTime.update();
            testVillage.tick();

            Assert.IsTrue(testVillage.army.get(UnitType.ARCHER).quantity == ARCH_QUANTITY);
            Assert.IsTrue(testVillage.army.get(UnitType.KNIGHT).quantity == KNIG_QUANTITY);
            Assert.IsTrue(testVillage.army.get(UnitType.WARRIOR).quantity == WARR_QUANTITY);
        }
    }
}
