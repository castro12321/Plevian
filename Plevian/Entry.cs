using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Debugging;
using Plevian.Players;
using Plevian.Maps;
using System.Threading;

namespace Plevian
{
    public class Entry
    {
        public static List<Player> players;
        public static Map map;

        [STAThread]
        public static void Main(string[] args)
        {
            // Alloc debug console
            NativeMethods.AllocConsole();
            Console.WriteLine("Debug Console");

            //App.Main();
            
            //Plevian.App app = new Plevian.App();
            //app.InitializeComponent();
            //app.Run();

            MainWindow main = new MainWindow(null, null);
            main.InitializeComponent();
            main.Show();
            main.run();
            main.Close();
            while(players != null && map != null)
            {
                MainWindow newMain = new MainWindow(players, map);
                players = null;
                map = null;
                newMain.Show();
                newMain.run();
                newMain.Close();
            }

        }
    }
}
