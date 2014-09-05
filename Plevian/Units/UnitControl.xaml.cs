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

        #region Properties
        public static readonly DependencyProperty UnitProperty =
        DependencyProperty.Register("Unit", typeof(UnitType), typeof(UnitControl), new UIPropertyMetadata(MyPropertyChangedHandler));

        public UnitType Unit
        {
            get { return (UnitType)GetValue(UnitProperty); }
            set 
            {
                SetValue(UnitProperty, value);
            }
        }
        #endregion
        #region Events
        public delegate void RecruitEvent(UnitType type, int quanity);
        public event RecruitEvent recruitEvent;
        #endregion

        public static void MyPropertyChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            switch(e.Property.ToString())
            {
                case "Unit" :
                    {
                        ((UnitControl)sender).setData((UnitType)e.NewValue);
                        break;
                    }
            }
        }

        private UnitModel model = new UnitModel();

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
            if(recruitEvent != null)
            {
                recruitEvent(model.Type, model.Quanity);
            }
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

        private void quanityChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tBox = Quanity;
            int quanity;
            if (Int32.TryParse(Quanity.Text.ToString(), out quanity)) ;
            model.Quanity = quanity;
        }


        #region UnitModel
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

            public int Quanity
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
                    
                }
            }

            public void setData(Unit unit)
            {
                data = unit;
            }

            public void setVillage(Village village)
            {
                this.village = village;
                Quanity = 1;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                var handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

       

        

        
    }
}
