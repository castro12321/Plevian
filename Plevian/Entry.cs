using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plevian.Debugging;

namespace Plevian
{
    public class Entry
    {
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

            MainWindow main = new MainWindow();
            main.InitializeComponent();
            main.Show();
            main.run();
        }
    }
}
