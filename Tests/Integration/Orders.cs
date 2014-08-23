using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian.Debugging;
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
    public class Orders : TestWithTime
    {
        public Orders()
        {
            fakeTime(0);
        }

        [TestMethod]
        public void Trading()
        {
            Player player = new Player("test", Color.Blue);
            Village source = new Village(new Location(0, 0), player, "source");
            Village target = new Village(new Location(0, 3), player, "target");
            source.resources.Clear();
            target.resources.Clear();
            source.resources.Add(new Resources(100, 200, 300, 400));
            Army traders = new Army();
            traders += UnitFactory.createUnit(UnitType.TRADER, 3);
            source.addArmy(traders);

            Assert.AreEqual(100, source.resources.food);
            Assert.AreEqual(200, source.resources.wood);
            Assert.AreEqual(300, source.resources.iron);
            Assert.AreEqual(400, source.resources.stone);
            Assert.AreEqual(0, target.resources.food);
            Assert.AreEqual(0, target.resources.wood);
            Assert.AreEqual(0, target.resources.iron);
            Assert.AreEqual(0, target.resources.stone);

            Assert.AreEqual(3, source.army.get(UnitType.TRADER).quanity);

            Army trader = new Army();
            trader += UnitFactory.createUnit(UnitType.TRADER, 1);
            Resources toSend = new Resources(50, 100, 150, 200);
            Order order = new TradeOrder(source, target, trader, toSend, null);
            source.addOrder(order);

            Assert.AreEqual(2, source.army.get(UnitType.TRADER).quanity);

            // Need 9 ticks for traders to go to the target
            order.tick(); order.tick(); order.tick();
            order.tick(); order.tick(); order.tick();
            order.tick(); order.tick(); order.tick();

            Assert.AreEqual(50, source.resources.food);
            Assert.AreEqual(100, source.resources.wood);
            Assert.AreEqual(150, source.resources.iron);
            Assert.AreEqual(200, source.resources.stone);
            Assert.AreEqual(50, target.resources.food);
            Assert.AreEqual(100, target.resources.wood);
            Assert.AreEqual(150, target.resources.iron);
            Assert.AreEqual(200, target.resources.stone);

            // Need 9 ticks for traders to go back to the source
            order.tick(); order.tick(); order.tick();
            order.tick(); order.tick(); order.tick();
            order.tick(); order.tick(); order.tick();

            Assert.AreEqual(3, source.army.get(UnitType.TRADER).quanity);
        }
    }
}
