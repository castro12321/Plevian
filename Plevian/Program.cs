using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Plevian.Maps;
using Plevian.Debug;
using Plevian.Units;

namespace Plevian
{
    public static class Program
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
            NativeMethods.AllocConsole();
            Console.WriteLine("Debug Console");

            armyDebug();

            Game game = new Game();
            while (true)
                game.tick();

            
          
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void armyDebug()
        {
            Army test = new Army();
            test += new Archer(50);
            test += new Knight(25);
            

            Army second = new Army();
            second += new Archer(50);
            second += new Knight(20);


            test -= second;
            Console.WriteLine(test.toString());
        }
    }
}
