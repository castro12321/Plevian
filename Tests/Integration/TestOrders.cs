using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian;
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
    public class TestOrders : TestWithTime
    {
        Player player1, player2;
        Village village1, village2;
        Tile plains;
        Army trader, traders;
        Resources testResources;

        public TestOrders()
        {
            fakeTime(0);
            player1 = new Player("test1", Color.Blue);
            player2 = new Player("test2", Color.Red);
            plains = new Tile(new Location(0, 0), TerrainType.PLAINS);
            village1 = new Village(new Location(0, 0), player1, "village1");
            village2 = new Village(new Location(0, 3), player2, "village2");
            trader = new Army();
            trader.add(UnitFactory.createUnit(UnitType.TRADER, 1));
            traders = new Army();
            traders.add(UnitFactory.createUnit(UnitType.TRADER, 3));
            testResources = new Resources();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SourceNullThrowsException()
        {
            new TradeOrder(null, null, village2, trader, testResources, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TargetNullThrowsException()
        {
            new TradeOrder(village1, village1, null, trader, testResources, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArmyNullThrowsException()
        {
            new TradeOrder(village1, village1, village2, null, testResources, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Target tile must be village")]
        public void TargetNotVillageThrowsException()
        {
            new TradeOrder(village1, village1, plains, trader, testResources, null);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void NoTradersInArmyThrowsException()
        {
            new TradeOrder(village1, village1, plains, new Army(), testResources, null);
        }

        [TestMethod]
        public void Trading()
        {
            Logger.log(GameTime.now + " --> " + FakeGameTime.now);
            addFakeTime(10);
            Logger.log(GameTime.now + " ==> " + FakeGameTime.now);
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
            Assert.AreEqual(3, village1.army.get(UnitType.TRADER).quantity);

            Resources toSend = new Resources(50, 100, 150, 200);
            Order order = new TradeOrder(village1, village1, village2, trader, toSend, null);
            village1.addOrder(order);

            // After sending one trader, village should have 2 traders
            Assert.AreEqual(2, village1.army.get(UnitType.TRADER).quantity);

            addFakeTime(order.OverallTime.time);
            GameTime.update();
            order.tick();

            Assert.AreEqual(50, village1.resources.food);
            Assert.AreEqual(100, village1.resources.wood);
            Assert.AreEqual(150, village1.resources.iron);
            Assert.AreEqual(200, village1.resources.stone);
            Assert.AreEqual(50, village2.resources.food);
            Assert.AreEqual(100, village2.resources.wood);
            Assert.AreEqual(150, village2.resources.iron);
            Assert.AreEqual(200, village2.resources.stone);

            addFakeTime(order.OverallTime.time);
            GameTime.update();
            order.tick();

            // Now source village should contain 3 traders again
            Assert.AreEqual(3, village1.army.get(UnitType.TRADER).quantity);
        }

        [TestMethod]
        public void testLoserDisapearance()
        {
            Army loser = new Army().add(new Knight(1));
            Army winner = new Army().add(new Archer(10000));

            Village testVillage = new Village(new Location(0,0), null, "");
            testVillage.addArmy(loser);
            Village winnersVillage = new Village(new Location(10,10), null, "");
            winnersVillage.addArmy(winner);

            Order losersOrder = new AttackOrder(testVillage, testVillage, winnersVillage, loser);
            Assert.IsTrue(testVillage.orders.Count == 0);
            testVillage.addOrder(losersOrder);
            Assert.IsTrue(testVillage.orders.Count == 1);
            
            addFakeTime(losersOrder.OverallTime.time);
            GameTime.update();
            testVillage.tick();
            addFakeTime(losersOrder.OverallTime.time);
            GameTime.update();
            testVillage.tick();
            
            Assert.IsTrue(testVillage.orders.Count == 0);
        }

        [TestMethod]
        public void testLootGathering()
        {
            testLootPart(new Resources(0, 0, 0, 0), new Resources(0, 0, 0, 0));
            testLootPart(new Resources(100, 0, 0, 0), new Resources(100, 0, 0, 0));
            testLootPart(new Resources(0, 100, 0, 0), new Resources(0, 100, 0, 0));
            testLootPart(new Resources(0, 0, 100, 0), new Resources(0, 0, 100, 0));
            testLootPart(new Resources(0, 0, 0, 100), new Resources(0, 0, 0, 100));
            testLootPart(new Resources(25, 25, 25, 25), new Resources(25, 25, 25, 25));
            testLootPart(new Resources(100, 100, 100, 100), new Resources(25, 25, 25, 25));
            testLootPart(new Resources(400, 400, 100, 100), new Resources(40, 40, 10, 10));
            testLootPart(new Resources(500, 300, 150, 50), new Resources(50, 30, 15, 5));
            testLootPart(new Resources(50, 150, 300, 500), new Resources(5, 15, 30, 50));
        }

        private void testLootPart(Resources startResources, Resources toLoot)
        {
            Village attackerVillage = new Village(new Location(0, 0), player1, "");
            Village defenderVillage = new Village(new Location(0, 1), player2, "");
            defenderVillage.takeResources(defenderVillage.resources);
            defenderVillage.addResources(startResources);
            Army attacker = new Army().add(new Warrior(2));
            attackerVillage.addArmy(attacker);
            
            AttackOrder order = new AttackOrder(attackerVillage, attackerVillage, defenderVillage, attacker);
            attackerVillage.addOrder(order);
            
            addFakeTime(order.OverallTime.time);
            GameTime.update();
            attackerVillage.tick();
            addFakeTime(order.OverallTime.time);
            GameTime.update();
            attackerVillage.tick();
            addFakeTime(order.OverallTime.time);
            GameTime.update();
            attackerVillage.tick();
            
            Assert.AreEqual(toLoot, order.loot);
        }
    }
}
