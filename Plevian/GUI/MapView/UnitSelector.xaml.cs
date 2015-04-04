using Plevian.Debugging;
using Plevian.Units;
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

namespace Plevian.GUI
{
    /// <summary>
    /// Interaction logic for UnitSelector.xaml
    /// </summary>
    public partial class UnitSelector : UserControl
    {
        public UnitType type;
        private int _maxQuantity = 0;
#region attributes
        public int maxQuantity
        {
            get
            {
                return _maxQuantity;
            }
            set
            {
                _maxQuantity = value;
                UnitMaxQuantity.Content = "(" + maxQuantity + ")";
                quantityChanged();
            }

        }

        
        public int quantity
        {
            get
            {
                if(checkCorrectness())
                {
                    return Int32.Parse(UnitQuantity.Text);
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                UnitQuantity.Text = value.ToString();
                quantityChanged();
            }


        }
#endregion

        public UnitSelector(string unitName, int maxQuantity, UnitType type)
        {
            InitializeComponent();
            UnitName.Content = unitName;
            this.type = type;
            this.maxQuantity = maxQuantity;
        }

        private void onPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c))
                e.Handled = false;
            else
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }

        private void maxQuantityEnter(object sender, MouseEventArgs e)
        {
            Label quantity = sender as Label;
            quantity.FontWeight = FontWeights.Bold;
        }

        private void maxQuantityLeave(object sender, MouseEventArgs e)
        {
            Label quantity = sender as Label;
            quantity.FontWeight = FontWeights.Normal;
        }

        private void maxQaunityMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                UnitQuantity.Text = maxQuantity.ToString();
            }
        }

        private void onInputChange(object sender, TextChangedEventArgs e)
        {
            quantityChanged();
        }

        private void quantityChanged()
        {
            if (checkCorrectness())
            {
                UnitQuantity.Background = Brushes.White;
            }
            else
            {
                UnitQuantity.Background = Brushes.Red;
            }
        }

        public bool checkCorrectness()
        {
            string content = UnitQuantity.Text;
            int quantity = 0;
            bool result = Int32.TryParse(content, out quantity);
            if (result == false || quantity > maxQuantity)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    
}
