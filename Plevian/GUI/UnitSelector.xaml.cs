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
        UnitType type;
        int maxQuanity = 0;
        public UnitSelector(string unitName, int maxQuanity, UnitType type)
        {
            InitializeComponent();
            UnitName.Content = unitName;
            UnitMaxQuanity.Content = "(" + maxQuanity + ")";
            this.type = type;
            this.maxQuanity = maxQuanity;
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

        private void maxQuanityEnter(object sender, MouseEventArgs e)
        {
            Label quanity = sender as Label;
            quanity.FontWeight = FontWeights.Bold;
        }

        private void maxQuanityLeave(object sender, MouseEventArgs e)
        {
            Label quanity = sender as Label;
            quanity.FontWeight = FontWeights.Normal;
        }

        private void maxQaunityMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.MouseDevice.LeftButton == MouseButtonState.Pressed)
            {
                UnitQuanity.Text = maxQuanity.ToString();
            }
        }

        private void onInputChange(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            string content = txtBox.Text;
            int quanity = 0;
            bool result = Int32.TryParse(content, out quanity);
            if (result == false || quanity > maxQuanity)
            {
                txtBox.Background = Brushes.Red;
            }
            else
            {
                txtBox.Background = Brushes.White;
            }
        }
    }
}
