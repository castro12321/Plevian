using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian;

namespace Tests.Unit
{
    [TestClass]
    public class TestTime
    {
        [TestMethod]
        public void subsequentCallsReturnSameTime()
        {
            // arrange
            GameTime gameTime = new GameTime(new LocalTime(0));

            // acts & asserts
            Assert.AreEqual(0ul, GameTime.time.seconds);
            Assert.AreEqual(0ul, GameTime.time.seconds); // subsequent calls should return the same local time
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(0ul, GameTime.time.seconds); // even if the system time changed
        }

        [TestMethod]
        public void systemTimeFollowing()
        {
            // arrange
            GameTime gameTime = new GameTime(new LocalTime(0));

            // act
            System.Threading.Thread.Sleep(1000);
            ulong diff = gameTime.updateTime(); // Update game time

            // assert
            Assert.IsTrue(diff == 1 || diff == 2); // Most likely one second passed. But if there were 0.999 millis, then we may catch 2 seconds instead of one
            Assert.IsTrue(GameTime.time.seconds == 1 || GameTime.time.seconds == 2);
        }

        [TestMethod]
        public void timeAdding()
        {
            // arrange
            GameTime gameTime = new GameTime(new LocalTime(50));

            // act
            LocalTime localMinute = new LocalTime(60);
            LocalTime added = GameTime.add(localMinute);

            // assert
            Assert.AreEqual(110ul, added.seconds);
        }

        [TestMethod]
        public void timeComparison()
        {
            // arrange
            LocalTime a = new LocalTime(10);
            LocalTime b = new LocalTime(10);
            LocalTime c = new LocalTime(20);

            // assert
            Assert.IsTrue(a == b);
            Assert.IsFalse(a == c);

            Assert.IsTrue(a < c);
            Assert.IsFalse(a > c);

            Assert.IsTrue(c > a);
            Assert.IsFalse(c < a);

            Assert.IsTrue(a >= b);
            Assert.IsFalse(a > b);
            Assert.IsTrue(a <= b);
            Assert.IsFalse(a < b);
        }
    }
}
