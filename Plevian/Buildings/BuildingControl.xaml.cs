using Plevian.Resource;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Plevian.Buildings
{
    /// <summary>
    /// Interaction logic for BuildingControl.xaml
    /// </summary>
    /// 

    public class BuildingCostConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Resources cost = (Resources) values[1];
            return cost;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Not supported conversion");
        }
    }

    public partial class BuildingControl : UserControl
    {
        public BuildingControl()
        {
            InitializeComponent();
        }

        private void OnUpgradeClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
