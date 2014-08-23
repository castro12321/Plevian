using Plevian.Maps;
using Plevian.Villages;
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

namespace Plevian.Orders
{
    public class TileToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            if (!(value is Tile))
                return "/ERROR/";
            // Do the conversion from bool to visibility
            Tile tile = (Tile)value;
            if (tile is Village)
            {
                Village village = tile as Village;
                return village.name;
            }
            else
                return Enum.GetName(typeof(TerrainType), tile.type);
        }

        public object ConvertBack(object value, Type targetType,
         object parameter, CultureInfo culture)
        {
            return null; //NO CONVERSION
        }
    }


    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {
        public OrderControl(Order order)
        {
            this.DataContext = order;
            InitializeComponent();
        }
    }
}
