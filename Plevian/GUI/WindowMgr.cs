using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Plevian.GUI
{
    public class WindowMgr
    {
        public static void Show(Window window, bool pauseMainWindow = true)
        {
            window.Show();
            if(pauseMainWindow)
            {
                System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(window);
                MainWindow.getInstance().IsEnabled = false;
                window.Closed += window_Closed;
            }
        }

        static void window_Closed(object sender, EventArgs e)
        {
            MainWindow.getInstance().IsEnabled = true;
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(MainWindow.getInstance());
        }
    }
}
