using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian.Units;

namespace Tests.Units
{
    [TestClass]
    public class testArmy
    {
        [TestMethod]
        public void testLootCapacity()
        {
            Army army = new Army();
            Unit knight = new Knight(100);
            Unit archer = new Archer(200);
            int capacity = knight.getLootCapacity() + archer.getLootCapacity();
            army += knight;
            army += archer;
            Assert.IsTrue(capacity == army.getLootCapacity());

        }
    }
}
