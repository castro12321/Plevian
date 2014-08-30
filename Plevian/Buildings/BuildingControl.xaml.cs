using Plevian.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        ViewModel model = new ViewModel();
        public BuildingControl()
        {
            InitializeComponent();

            this.DataContextChanged += BuildingControl_DataContextChanged;
        }

        void BuildingControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Object newValue = e.NewValue;
            if(newValue is KeyValuePair<BuildingType, Building>)
            {
                KeyValuePair<BuildingType, Building> pair = (KeyValuePair<BuildingType, Building>)newValue;
                setData(pair.Value);
            }
        }

        private void OnUpgradeClick(object sender, RoutedEventArgs e)
        {

        }

        public void setData(Building data)
        {
            stackPanel.DataContext = model;
            model.setData(data);
        }


    }

    public class ViewModel : INotifyPropertyChanged
    {
        Building data;
        
        public String Name
        {
            get
            {
                return data.getDisplayName();
            }
        }

        public int Level
        {
            get
            {
                return data.level;
            }
        }

        public Resources Price
        {
            get
            {
                return data.getPriceForNextLevel();
            }
        }


        public ViewModel(Building data)
        {
            setData(data);
        }

        public ViewModel()
        {
            this.data = null;
        }

        public void setData(Building data)
        {
            this.data = data;
            this.data.PropertyChanged += data_PropertyChanged;
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("Price");
            NotifyPropertyChanged("Level");
        }

        private void data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if (propertyName == "level")
            {
                NotifyPropertyChanged("Level");
                NotifyPropertyChanged("Price");
            }
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
