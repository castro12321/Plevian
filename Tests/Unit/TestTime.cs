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
            GameTime.init(0);

            // acts & asserts
            Assert.AreEqual(new Seconds(0), GameTime.now);
            Assert.AreEqual(new Seconds(0), GameTime.now); // subsequent calls should return the same local time
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(new Seconds(0), GameTime.now); // even if the system time changed
        }

        [TestMethod]
        public void systemTimeFollowing()
        {
            // arrange
            GameTime.init(0);

            // act
            System.Threading.Thread.Sleep(1000);
            ulong diff = GameTime.update(); // Update game time

            // assert
            Assert.IsTrue(diff == 1 || diff == 2); // Most likely one second passed. But if there were 0.999 millis, then we may catch 2 seconds instead of one
            Assert.IsTrue(GameTime.now == new Seconds(1) || GameTime.now == new Seconds(2));
        }

        [TestMethod]
        public void timeAdding()
        {
            // arrange
            GameTime.init(50);

            // act
            GameTime localMinute = new Seconds(60);
            GameTime added = GameTime.now + localMinute;
            GameTime end = new Seconds(110);

            // assert
            Assert.AreEqual(end, added);
        }

        [TestMethod]
        public void timeComparison()
        {
            // arrange
            GameTime a = new Seconds(10);
            GameTime b = new Seconds(10);
            GameTime c = new Seconds(20);

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
