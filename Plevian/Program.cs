using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Plevian
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            Map.Map map = new Map.Map(4, 4);
            map.place(new Map.Location(0, 0), TerrainTypes.PLAINS);
            map.place(new Map.Location(1, 0), TerrainTypes.MOUNTAINS);
            map.place(new Map.Location(2, 0), TerrainTypes.MOUNTAINS);
            map.place(new Map.Location(3, 0), TerrainTypes.PLAINS);
            map.place(new Map.Location(0, 1), TerrainTypes.VILLAGE);
            map.place(new Map.Location(1, 1), TerrainTypes.LAKES);
            map.place(new Map.Location(2, 1), TerrainTypes.PLAINS);
            map.place(new Map.Location(3, 1), TerrainTypes.LAKES);
            map.place(new Map.Location(0, 2), TerrainTypes.MOUNTAINS);
            map.place(new Map.Location(1, 2), TerrainTypes.PLAINS);
            map.place(new Map.Location(2, 2), TerrainTypes.PLAINS);
            map.place(new Map.Location(3, 2), TerrainTypes.PLAINS);
            map.place(new Map.Location(0, 3), TerrainTypes.VILLAGE);
            map.place(new Map.Location(1, 3), TerrainTypes.MOUNTAINS);
            map.place(new Map.Location(2, 3), TerrainTypes.VILLAGE);
            map.place(new Map.Location(3, 3), TerrainTypes.PLAINS);
        }
    }
}
