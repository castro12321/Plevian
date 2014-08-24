using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian;
using Plevian.Buildings;
using Plevian.Debugging;
using Plevian.Exceptions;
using Plevian.Maps;
using Plevian.Orders;
using Plevian.Players;
using Plevian.Resource;
using Plevian.Units;
using Plevian.Villages;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Integration
{
    [TestClass]
    public class Building : TestWithTime
    {
        Player player;
        Village village;

        public Building()
        {
            fakeTime(0);
            player = new Player("test", Color.Blue);
            village = new Village(new Location(0, 0), player, "village1");
            village.resources.Add(new Resources(9999, 9999, 9999, 9999));
        }

        [TestMethod]
        [ExpectedException(typeof(ExceptionNotEnoughResources))]
        public void CantAffordThrowsException()
        {
            village.resources.Clear();
            village.build(BuildingType.TOWN_HALL);
        }

        [TestMethod]
        public void BuildingQueue()
        {
            village.build(BuildingType.TOWN_HALL);

            Assert.IsFalse(village.isBuilt(BuildingType.TOWN_HALL));

            addFakeTime(15);
            GameTime.update();
            village.tick();

            Assert.IsTrue(village.isBuilt(BuildingType.TOWN_HALL));
        }
    }
}
