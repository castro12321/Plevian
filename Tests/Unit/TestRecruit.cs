﻿using System;
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

namespace Tests.Unit
{
    [TestClass]
    public class TestRecruit : TestWithTime
    {
        [TestMethod]
        public void testRecruit()
        {
            fakeTime(0);

            

            const int ARCH_QUANITY = 500;
            const int KNIG_QUANITY = 500;
            const int WARR_QUANITY = 500;


            Archer archer = new Archer(ARCH_QUANITY);
            Game game = new Game();
            Village testVillage = new Village(null, new Player("", Color.Blue), "test");
            testVillage.addResources(new Resources(1000000, 1000000, 1000000, 1000000));
            testVillage.recruit(archer);
            testVillage.recruit(new Knight(KNIG_QUANITY));
            testVillage.recruit(new Warrior(WARR_QUANITY));
            GameTime now = GameTime.now;

            Seconds wait = testVillage.recruitTimeEnd - now;
            wait.seconds += 2;
            int seconds = wait.seconds;
            while (wait.seconds > 0)
            {
                
                wait.seconds--;
                testVillage.tick();
                addFakeTime(1);
                now = GameTime.now;
                Logger.s(""+(testVillage.recruitTimeEnd - now));
            }
            
            
            GameTime.update();
            int czas = getFakeTime();
            Logger.s("Fake time = " + czas);
            if(ARCH_QUANITY > 0)
                 Assert.IsTrue(testVillage.army.get(UnitType.ARCHER).quanity == ARCH_QUANITY);
            if(KNIG_QUANITY > 0)
                 Assert.IsTrue(testVillage.army.get(UnitType.KNIGHT).quanity == KNIG_QUANITY);
            if(WARR_QUANITY > 0)
                Assert.IsTrue(testVillage.army.get(UnitType.WARRIOR).quanity == WARR_QUANITY);
        }
    }
}
