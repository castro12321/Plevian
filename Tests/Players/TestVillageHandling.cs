using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian;
using Plevian.Players;
using Plevian.Villages;
using SFML.Graphics;
using System;

namespace Tests.Players
{
    [TestClass]
    public class TestVillageHandling : TestWithTime
    {
        Player player;
        Village vil1;
        Village vil2;

        public TestVillageHandling()
        {
            fakeTime(0);
        }

        [TestMethod]
        public void AddFevVillages()
        {
            player = new Player("Test", Color.Red);
            vil1 = new Village(null, player, "test");
            vil2 = new Village(null, player, "test");
            player.addVillage(vil1);
            player.addVillage(vil2);
        }

        [TestMethod]
        public void RemoveVillages()
        {
            AddFevVillages();
            player.removeVillage(vil1);
            player.removeVillage(vil2);
            bool working_throw = false;
            try
            {
                player.removeVillage(vil1);
            }
            catch  (Exception)
            {
                working_throw = true;
            }
            Assert.IsTrue(working_throw);
        }
    }
}
