﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Plevian.Debugging;

namespace Plevian.GUI
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow(Window parentWindow)
        {
            if (parentWindow == null)
            {
                Logger.Error("parentWindow is null");
                return;
            }
            
            InitializeComponent();
            Owner = parentWindow;

            Closed += (x, y) => Owner.Close();

            WindowStartupLocation = WindowStartupLocation.Manual;
            parentWindow.SizeChanged += parent_SizeChanged;
            parentWindow.LocationChanged += parent_LocationChanged;
            UpdateLocationAndSizeToParent();

            Show();
        }

        private void UpdateLocationAndSizeToParent()
        {
            Top = Owner.Top;
            Left = Owner.Left;
            Width = Owner.ActualWidth;
            Height = Owner.ActualHeight;
        }

        private void parent_LocationChanged(object sender, EventArgs e)
        {
            UpdateLocationAndSizeToParent();
        }

        private void parent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLocationAndSizeToParent();
        }

        private void OverlayWindow_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Logger.Trace("OVERLAY CLICK");
        }
    }
}
