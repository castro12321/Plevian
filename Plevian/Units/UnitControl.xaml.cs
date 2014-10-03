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
            DependencyProperty.Register("Unit", typeof(UnitType), typeof(UnitControl),  new UIPropertyMetadata(UnitPropertyChangedHandler));

        public delegate void RecruitEvent(UnitType type, int quantity);
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
            model.PropertyChanged += model_PropertyChanged;
        }

        void model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "HaveResources")
            {
                if(!model.HaveResources)
                {
                    resourceControl.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
                else
                {
                    resourceControl.Background = new SolidColorBrush(Colors.Transparent);
                }
            }
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
            model.Quantity = 1;
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
            public Unit data;
            public Village village;

            public UnitModel()
            {
                GameTime.PropertyChanged += GameTime_PropertyChanged;
            }

            void GameTime_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if(e.PropertyName == "now")
                {
                    NotifyPropertyChanged("MaxQuantity");
                    NotifyPropertyChanged("RequirementsMet");
                    NotifyPropertyChanged("HaveResources");
                }
            }

            private Unit _unitInVillage;
            public Unit unitInVillage
            {
                get { return _unitInVillage; }
                set { _unitInVillage = value; NotifyPropertyChanged(); }
            }

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
                    return data.quantity;
                }

                set
                {
                    data.quantity = value;
                    NotifyPropertyChanged("Cost");
                    NotifyPropertyChanged();
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
                    if (village == null || data == null)
                        return false;
                    return village.resources.canAfford(data.getWholeUnitCost());
                }
            }

            public int maxQuantity
            {
                get
                {
                    if (village == null || data == null)
                        return 0;
                    Resources vilResources = village.resources;
                    Resources neededResources = data.recruitCost;
                    int quant = vilResources.howMuchAfford(neededResources);
                    return vilResources.howMuchAfford(neededResources);
                }
            }

            public bool RequirementsMet
            {
                get
                {
                    if (village == null || data == null)
                        return false;
                    return data.requirements.isFullfilled(village);
                }
            }

            public void setData(Unit unit)
            {
                data = unit;
            }

            public void setVillage(Village village)
            {
                this.village = village;
                unitInVillage = village.army[data.unitType];
                Quantity = 1;
                NotifyPropertyChanged("QuantityToRecruit");
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
