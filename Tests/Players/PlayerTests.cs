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
    public class PlayerTests : TestWithTime
    {

        Player player;
        Village vil1;
        Village vil2;

        public PlayerTests()
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
    }
}
