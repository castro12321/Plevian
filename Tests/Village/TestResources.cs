using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian.Resource;

namespace Tests.Units
{
    [TestClass]
    public class TestResources
    {
        private void assertresources(Resources resources, int food, int wood, int iron, int stone)
        {
            Assert.IsTrue(resources.food == food);
            Assert.IsTrue(resources.wood == wood);
            Assert.IsTrue(resources.iron == iron);
            Assert.IsTrue(resources.stone == stone);
        }

        [TestMethod]
        public void resourcesConstructors()
        {
            // arrange
            Resources food = new Food(10);
            Resources wood = new Wood(20);
            Resources iron = new Iron(30);
            Resources stone = new Stone(40);
            Resources merged = food + wood + iron + stone;
            Resources resources = new Resources(10, 20, 30, 40);

            // assert
            assertresources(resources, food.food, wood.wood, iron.iron, stone.stone);
            Assert.IsTrue(resources.stone == stone.stone);
            Assert.IsTrue(resources == merged);
        }

        [TestMethod]
        public void resourcesAdding()
        {
            // arrange
            Resources res1 = new Resources(50, 60, 70, 80);
            Resources res2;

            // act & assert
            res2 = new Food(50);
            Assert.IsTrue(res1.canAfford(res2));
            Assert.IsTrue(res1 >= res2);
            Assert.IsFalse(res1 > res2);
            Assert.IsFalse(res2 >= res1);
            Assert.IsFalse(res2 == res1);

            res2 = new Food(60);
            Assert.IsFalse(res1.canAfford(res2));
            Assert.IsFalse(res1 >= res2);
            Assert.IsFalse(res1 > res2);
            Assert.IsFalse(res2 >= res1);
            Assert.IsFalse(res2 == res1);

            res2 = new Food(30) + new Wood(40) + new Iron(50);
            Assert.IsTrue(res1.canAfford(res2));

            res2 += new Stone(50);
            Assert.IsTrue(res1.canAfford(res2));

            res2 += new Stone(50);
            Assert.IsFalse(res1.canAfford(res2));
        }
    }
}
