using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Debugging;
using Plevian.Players;
using Plevian.Maps;
using System.Threading;
using Plevian.Save;

namespace Plevian
{
    public class Entry
    {
        public static SaveReader save;

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

            do
            {
                MainWindow main = new MainWindow(save);
                save = null;
                main.InitializeComponent();
                main.Show();
                main.run();
                main.Close();
            }
            while (save != null);

        }
    }
}
