﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plevian;
using Plevian.Maps;

namespace Tests.Unit
{
    [TestClass]
    public class TestMap
    {
        private const int mapSizeX = 4, mapSizeY = 4;

        private Map getDefaultMap()
        {
            return new Map(mapSizeX, mapSizeY);
        }
        
        [TestMethod]
        public void defaultMapIsPlains()
        {
            // arrange
            const TerrainType expectedTerrain = TerrainType.PLAINS;

            // act
            Map map = getDefaultMap();

            // assert
            for (int x = 0; x < mapSizeX; ++x)
                for (int y = 0; y < mapSizeY; ++y)
                    Assert.AreEqual(expectedTerrain, map.typeAt(new Location(x, y)));
        }

        [TestMethod]
        public void mapChanges()
        {
            // arrange
            Map map = getDefaultMap();

            // act
            map.place(new Location(0, 0), TerrainType.LAKES);
            map.place(new Location(2, 1), TerrainType.LAKES);
            map.place(new Location(3, 3), TerrainType.LAKES);
            map.place(new Location(1, 1), TerrainType.MOUNTAINS);
            map.place(new Location(1, 2), TerrainType.VILLAGE);
            map.place(new Location(1, 3), TerrainType.MOUNTAINS);

            // assert
            Assert.AreEqual(TerrainType.LAKES, map.typeAt(new Location(0, 0)));
            Assert.AreEqual(TerrainType.LAKES, map.typeAt(new Location(2, 1)));
            Assert.AreEqual(TerrainType.LAKES, map.typeAt(new Location(3, 3)));
            Assert.AreEqual(TerrainType.PLAINS, map.typeAt(new Location(1, 0)));
            Assert.AreEqual(TerrainType.MOUNTAINS, map.typeAt(new Location(1, 1)));
            Assert.AreEqual(TerrainType.VILLAGE, map.typeAt(new Location(1, 2)));
            Assert.AreEqual(TerrainType.MOUNTAINS, map.typeAt(new Location(1, 3)));
        }
    }
}
