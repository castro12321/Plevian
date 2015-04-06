using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Maps;
using Plevian.TechnologY;

namespace Plevian.GUI.TechnologiesView
{
    public class TechnologiesMapLoader
    {
        public Map map { get; private set; }

        private void placeRoad(int posX, int posY)
        {
            map.place(new Tile(new Location(posX, posY), TerrainType.LAKES));
        }

        private void add(int posX, int posY, Technology technology)
        {
            map.place(new TechnologyTile(new Location(posX, posY), technology));
        }

        public TechnologiesMapLoader()
        {
            const int mapSizeX = 15;
            const int mapSizeY = 10;

            map = new Map(mapSizeX, mapSizeY);

            placeRoad(0, 0);
            placeRoad(0, 1);
            add(1, 1, new TechnologyLeatherArmors());
            placeRoad(2, 1);
            add(3, 1, new TechnologyChainArmors());
            placeRoad(4, 1);
            add(5, 1, new TechnologyPlateArmors());

            placeRoad(0, 2);
            placeRoad(0, 3);
            add(1, 3, new TechnologyFoo());

            placeRoad(0, 4);
            placeRoad(0, 5);
            add(1, 5, new TechnologyFire());

            placeRoad(0, 6);
            placeRoad(0, 7);
            add(1, 7, new TechnologyLasers());
            placeRoad(2, 7);
            add(3, 7, new TechnologyNukes());
        }
    }
}
