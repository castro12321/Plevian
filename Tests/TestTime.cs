using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian;

namespace Tests.Units
{
    [TestClass]
    public class TestTime : TestWithTime
    {
        [TestMethod]
        public void subsequentCallsReturnSameTime()
        {
            // arrange
            fakeTime(0);

            // acts & asserts
            Assert.AreEqual(new GameTime(0), GameTime.now);
            Assert.AreEqual(new GameTime(0), GameTime.now); // subsequent calls should return the same local time
            addFakeTime(1);
            Assert.AreEqual(new GameTime(0), GameTime.now); // even if the system time changed
        }

        [TestMethod]
        public void systemTimeFollowing()
        {
            // arrange
            fakeTime(0);

            // act
            addFakeTime(1);
            ulong diff = GameTime.update(); // Update game time
            
            // assert
            Assert.IsTrue(diff == 1);
            Assert.IsTrue(GameTime.now == new GameTime(1));
        }

        [TestMethod]
        public void timeAdding()
        {
            // arrange
            GameTime.init(50);

            // act
            GameTime localMinute = new GameTime(60);
            GameTime added = GameTime.now + localMinute;
            GameTime end = new GameTime(110);

            // assert
            Assert.AreEqual(end, added);
        }

        [TestMethod]
        public void timeComparison()
        {
            // arrange
            GameTime a = new GameTime(10);
            GameTime b = new GameTime(10);
            GameTime c = new GameTime(20);

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
