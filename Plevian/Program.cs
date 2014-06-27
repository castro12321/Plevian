using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Plevian.Map;

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
            /*
            Map.Map map = (new MapGenerator().Generate(10, 10));
            Save save = new Save("save1");
            new Map.MapFileWriter().save(map, save);
             */

            Village.Village village = new Village.Village();
            village.collectProduction();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
