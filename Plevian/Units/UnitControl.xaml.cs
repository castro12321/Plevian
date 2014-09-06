using Plevian.Debugging;
using Plevian.Resource;
using Plevian.Villages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Plevian.Units
{
    /// <summary>
    /// Interaction logic for UnitControl.xaml
    /// </summary>
    public partial class UnitControl : UserControl
    {
        public static readonly DependencyProperty UnitProperty =
            //DependencyProperty.Register("Unit", typeof(UnitType), typeof(UnitControl), new UIPropertyMetadata(UnitPropertyChangedHandler));
        DependencyProperty.Register("Unit", typeof(UnitType), typeof(UnitControl), new UIPropertyMetadata(UnitPropertyChangedHandler));

        public delegate void RecruitEvent(UnitType type, int quanity);
        public event RecruitEvent recruitEvent;

        private UnitModel model = new UnitModel();

        public UnitType Unit
        {
            get { return (UnitType)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        public static void UnitPropertyChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((UnitControl)sender).setData((UnitType)e.NewValue);
        }

        public UnitControl()
        {
            InitializeComponent();
        }

        public void setData(UnitType type)
        {
            Unit unit = UnitFactory.createUnit(type, 1);
            model.setData(unit);
            this.DataContext = model;
        }

        public void setVillage(Village village)
        {
            model.setVillage(village);
        }

        private void RecruitClicked(object sender, RoutedEventArgs e)
        {
            if (recruitEvent != null)
                recruitEvent(model.Type, model.Quantity);
        }

        private void onPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Convert.ToInt32(e.Text);
            }
            catch { e.Handled = true; }
            //base.OnPreviewTextInput(e);
        }

        private void quantityChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int quantity = Convert.ToInt32(Quantity.Text);
                if (quantity > 10000)
                {
                    quantity = 10000;
                    (sender as TextBox).Text = quantity.ToString();
                }
                model.Quantity = quantity;
            }
            catch { }
        }


        private class UnitModel : INotifyPropertyChanged
        {
            private Unit data;
            private Village village;

            public String Name
            {
                get
                {
                    return data.name;
                }
            }

            public Resources Cost
            {
                get
                {
                    return data.getWholeUnitCost();
                }
            }

            public int Quantity
            {
                get
                {
                    return data.quanity;
                }

                set
                {
                    data.quanity = value;
                    NotifyPropertyChanged("Cost");
                }
            }

            public UnitType Type
            {
                get
                {
                    return data.unitType;
                }
            }

            public bool HaveResources
            {
                get
                {
                    return true;
                }
            }

            public void setData(Unit unit)
            {
                data = unit;
            }

            public void setVillage(Village village)
            {
                this.village = village;
                Quantity = 1;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                var handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
