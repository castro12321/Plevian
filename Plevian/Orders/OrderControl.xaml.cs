using Plevian.Maps;
using Plevian.Units;
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

    public class OrderTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            if (!(value is Order))
                return "/ERROR/";
            // Do the conversion from bool to visibility
            Order order = value as Order;
            if (order.isGoingBack)
                return "Going back";
            OrderType type = order.Type;
            return Enum.GetName(typeof(OrderType), type);
        }

        public object ConvertBack(object value, Type targetType,
         object parameter, CultureInfo culture)
        {
            return null; //NO CONVERSION
        }
    }

    public class progressBarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            if (!(value is Order))
                return "/ERROR/";
            // Do the conversion from bool to visibility
            Order order = value as Order;
            int remaining = order.Duration.seconds;
            int overall = order.OverallTime.seconds;

            int progress = ((overall - remaining)*100) / overall;
            return progress;
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

        public OrderControl()
        {
            InitializeComponent();
        }

        private void onToolTipShow(object sender, ToolTipEventArgs e)
        {
            if (this.DataContext == null || !(this.DataContext is Order))
                return;
            Order order = this.DataContext as Order;
            int sum = 0;
            string tooltip = "";
            foreach (var pair in order.army.getUnits())
            {
                string unitName = Enum.GetName(typeof(UnitType), pair.Key);
                tooltip += "\n" + pair.Value.ToString();
                sum += pair.Value.quanity;
            }

            tooltip = "Units : " + sum +  tooltip;
            StackPanel.ToolTip = tooltip;

        }
    }
}
