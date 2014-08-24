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
        Player player;
        Village village1, village2;
        Tile plains;
        Army trader, traders;
        Resources testResources;

        public Orders()
        {
            fakeTime(0);
            player = new Player("test", Color.Blue);
            plains = new Tile(new Location(0, 0), TerrainType.PLAINS);
            village1 = new Village(new Location(0, 0), player, "village1");
            village2 = new Village(new Location(0, 3), player, "village2");
            trader = new Army();
            trader += UnitFactory.createUnit(UnitType.TRADER, 1);
            traders = new Army();
            traders += UnitFactory.createUnit(UnitType.TRADER, 3);
            testResources = new Resources();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SourceNullThrowsException()
        {
            new TradeOrder(null, village2, trader, testResources, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TargetNullThrowsException()
        {
            new TradeOrder(village1, null, trader, testResources, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArmyNullThrowsException()
        {
            new TradeOrder(village1, village2, null, testResources, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Target tile must be village")]
        public void TargetNotVillageThrowsException()
        {
            new TradeOrder(village1, plains, trader, testResources, null);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void NoTradersInArmyThrowsException()
        {
            new TradeOrder(village1, plains, new Army(), testResources, null);
        }

        [TestMethod]
        public void Trading()
        {
            village1.resources.Clear();
            village1.resources.Add(new Resources(100, 200, 300, 400));
            village1.addArmy(traders);

            village2.resources.Clear();

            // Test initial villages resources
            Assert.AreEqual(100, village1.resources.food);
            Assert.AreEqual(200, village1.resources.wood);
            Assert.AreEqual(300, village1.resources.iron);
            Assert.AreEqual(400, village1.resources.stone);
            Assert.AreEqual(0, village2.resources.food);
            Assert.AreEqual(0, village2.resources.wood);
            Assert.AreEqual(0, village2.resources.iron);
            Assert.AreEqual(0, village2.resources.stone);

            // Before sending any traders, village should have initial value of 3 traders
            Assert.AreEqual(3, village1.army.get(UnitType.TRADER).quanity);

            Resources toSend = new Resources(50, 100, 150, 200);
            Order order = new TradeOrder(village1, village2, trader, toSend, null);
            village1.addOrder(order);

            // After sending one trader, village should have 2 traders
            Assert.AreEqual(2, village1.army.get(UnitType.TRADER).quanity);

            // Need 9 ticks for traders to go to the target
            for(int i = 0;i < order.OverallTime.seconds; ++ i)
            {
                order.tick(); 
            }

            Assert.AreEqual(50, village1.resources.food);
            Assert.AreEqual(100, village1.resources.wood);
            Assert.AreEqual(150, village1.resources.iron);
            Assert.AreEqual(200, village1.resources.stone);
            Assert.AreEqual(50, village2.resources.food);
            Assert.AreEqual(100, village2.resources.wood);
            Assert.AreEqual(150, village2.resources.iron);
            Assert.AreEqual(200, village2.resources.stone);

            // Need 9 ticks for traders to go back to the source
            order.tick(); order.tick(); order.tick();
            order.tick(); order.tick(); order.tick();
            order.tick(); order.tick(); order.tick();

            // Now source village should contain 3 traders again
            Assert.AreEqual(3, village1.army.get(UnitType.TRADER).quanity);
        }

        [TestMethod]
        public void testLoserDisapearance()
        {
            Army loser = new Army() + new Knight(1);
            Army winner = new Army() + new Archer(10000);

            Player pl1 = new Player("", Color.Red);
            Player pl2 = new Player("", Color.Red);

            Village testVillage = new Village(new Location(0, 0), pl1, "");
            testVillage.addArmy(new Army() + new Knight(1));
            Village winnersVillage = new Village(new Location(10, 10), pl2, "");
            winnersVillage.addArmy(winner);

            Order losersOrder = new AttackOrder(testVillage, winnersVillage, loser);
            testVillage.addOrder(losersOrder);
            for (int i = 0; i < losersOrder.OverallTime.seconds + 1; ++i)
                testVillage.tick();
            Assert.IsTrue(testVillage.orders.Count == 0);
        }

        [TestMethod]
        public void takeoverTest()
        {
            Player Ceasar = new Player("Ceaser the great", Color.Yellow);
            Player weakling = new Player("Weakling", Color.Black);

            Army loser = new Army() + new Knight(1);
            Army winner = new Army() + new Archer(10000) + new Duke(1);

            Village testVillage = new Village(new Location(0, 0), weakling, "");
            testVillage.addArmy(loser);
            Village winnersVillage = new Village(new Location(10, 10), Ceasar, "");
            winnersVillage.addArmy(new Army() + new Archer(10000) + new Duke(1));

            Ceasar.addVillage(winnersVillage);
            weakling.addVillage(testVillage);

            Order WinnersOrder = new CaptureOrder(winnersVillage, testVillage, winner);
            winnersVillage.addOrder(WinnersOrder);
            for (int i = 0; i < WinnersOrder.OverallTime.seconds + 1; ++i)
                winnersVillage.tick();
            Assert.IsTrue(winnersVillage.orders.Count == 0);
            Assert.IsTrue(testVillage.Owner == Ceasar);
        }
    }
}
