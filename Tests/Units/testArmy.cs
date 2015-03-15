using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian.Units;
using Plevian.Debugging;

namespace Tests.Units
{
    [TestClass]
    public class TestArmy
    {
        [TestMethod]
        public void testLootCapacity()
        {
            Army army = new Army();
            Unit knight = new Knight(100);
            Unit archer = new Archer(200);
            int capacity = (knight.baseLootCapacity * 100) + (archer.baseLootCapacity * 200);
            army.add(knight);
            army.add(archer);
            Assert.IsTrue(capacity == army.getLootCapacity());

        }
    }
}
