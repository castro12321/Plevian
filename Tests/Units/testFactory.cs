using System;
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

namespace Tests.Units
{
    [TestClass]
    public class TestFactory
    {
        [TestMethod]
        public void testUnitFactory()
        {
            Unit unit = UnitFactory.createUnit(UnitType.ARCHER, 100);
            Assert.IsTrue((unit.getUnitType() == UnitType.ARCHER) && unit.quanity == 100);
        }
    }
}
