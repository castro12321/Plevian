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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Plevian.Resource
{
    /// <summary>
    /// Interaction logic for ResourceControl.xaml
    /// </summary>
    public partial class ResourceControl : UserControl
    {
        public static readonly DependencyProperty ResourcesProperty = DependencyProperty.Register(
    "Resources", typeof(Resources), typeof(ResourceControl));

        public Resources resources
        {
            get
            {
                return (Resources) this.GetValue(ResourcesProperty); 
            }
            set
            {
                this.SetValue(ResourcesProperty, value);
                this.DataContext = value;
            }
        }


        public ResourceControl()
        {
            InitializeComponent();
        }

        public ResourceControl(Resources resources)
        {
            InitializeComponent();
            this.resources = resources;
        }

        private void countChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e == null)
                return;
            string newValue = e.NewValue.ToString();
            Label label = sender as Label;
            if(newValue == "0")
            {

                StackPanel panel = label.Parent as StackPanel;
                panel.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                StackPanel panel = label.Parent as StackPanel;
                panel.Visibility = System.Windows.Visibility.Visible;
            }
        }

    }
}
