using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian;
using Plevian.Players;
using Plevian.Villages;
using SFML.Graphics;
using System;
using Tests.Unit;

namespace Tests.Players
{
    [TestClass]
    public class VillageHandling : TestWithTime
    {
        Player player;
        Village vil1;
        Village vil2;

        public VillageHandling()
        {
            fakeTime(0);
        }

        [TestMethod]
        public void PlayerCreating()
        {
            player = new Player("Test", Color.Blue);
            Assert.IsTrue(player.color.ToString() == Color.Blue.ToString());
            Assert.IsTrue(player.name == "Test");
        }

        [TestMethod]
        public void AddFevVillages()
        {
            player = new Player("Test", Color.Red);
            Village vil1 = new Village(null);
            Village vil2 = new Village(null);
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
