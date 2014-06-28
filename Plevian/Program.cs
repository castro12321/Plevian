﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Plevian.Map;
using Plevian.Debug;

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

            Game game = new Game();
            while (true)
                game.tick();

            NativeMethods.AllocConsole();
            Console.WriteLine("Debug Console");
          
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
