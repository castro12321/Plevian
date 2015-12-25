using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Plevian.GUI
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow(Window parentWindow)
        {
            InitializeComponent();
            Owner = parentWindow;

            WindowStyle = System.Windows.WindowStyle.None;
            ResizeMode = System.Windows.ResizeMode.NoResize;
            parentWindow.SizeChanged += parent_SizeChanged;
            parentWindow.LocationChanged += parent_LocationChanged;
            UpdateLocationAndSizeToParent();

            Show();
        }

        private static int WindowBorderWidth = 8;
        private static int WindowTitleBarWidth = 30;
        private void UpdateLocationAndSizeToParent()
        {
            Top = Owner.Top + WindowTitleBarWidth;
            Left = Owner.Left + WindowBorderWidth;
            Width = Owner.Width - (WindowBorderWidth * 2);
            Height = Owner.Height - WindowTitleBarWidth - WindowBorderWidth; // 5px bottom bar, 32px top menu
        }

        private void parent_LocationChanged(object sender, EventArgs e)
        {
            UpdateLocationAndSizeToParent();
        }

        private void parent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLocationAndSizeToParent();
        }
    }
}
