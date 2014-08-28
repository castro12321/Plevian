﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Plevian.Resource
{
    /// <summary>
    /// Interaction logic for ResourceControl.xaml
    /// </summary>
    public partial class ResourceControl : UserControl
    {
        public ResourceControl()
        {
            InitializeComponent();
        }

        public ResourceControl(Resources resources)
        {
            InitializeComponent();
            this.DataContext = resources;
        }
    }
}
