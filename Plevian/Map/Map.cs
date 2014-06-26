using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plevian
{
    class Map
    {
        private TerrainTypes[,] fields;


        public Map(int x, int y)
        {
            fields = new TerrainTypes[x, y];
            
            generateMap();
        }

        /// <summary>
        /// Loads map from file
        /// </summary>
        /// <param name="path">path to the file</param>
        public Map(string path)
        {

        }

        private void generateMap()
        {
            
        }
    }
}
