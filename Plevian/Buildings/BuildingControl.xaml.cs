using Plevian.Resource;
using Plevian.TechnologY;
using Plevian.Villages;
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
                Village village = MainWindow.getInstance().villageTab.Village;

                setData(pair.Value, village);

            }
        }


        public void setData(Building data, Village village)
        {
            this.DataContext = model;
            stackPanel.DataContext = model;
            model.setData(data, village);
        }

        public Building getData()
        {
            return model.data;
        }

        public delegate void upgradeEventHandler(Object sender, Building building);

        public event upgradeEventHandler Upgrade;

        private void OnUpgradeClick(object sender, RoutedEventArgs e)
        {
            if(Upgrade != null)
            {
                Upgrade(sender, getData());
            }
        }
            


    }

    public class ViewModel : INotifyPropertyChanged
    {
        public Building data = null;
        public Village village = null;

        public ViewModel()
        {
            GameTime.PropertyChanged += GameTime_PropertyChanged;
        }

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
                if (showResources)
                    return new Resources();
                return data.getPriceFor(village.getBuildingLevel(data.type, true) + 1);
            }
        }

        public bool CanBuild
        {
            get
            {
                bool test = village.canBuild(data.type);
                return village.canBuild(data.type);
            }
        }

        public bool showResources
        {
            get
            {
                return village.getBuildingLevel(data.type, true) >= data.getMaxLevel();
            }
        }

        public bool HaveTechnology
        {
            get
            {
                return data.requirements.isFullfilled(village);
            }
        }

        public ViewModel(Building data, Village village)
        {
            setData(data, village);
        }


        public void setData(Building data, Village village)
        {
            this.data = data;
            this.village = village;

            this.data.PropertyChanged += data_PropertyChanged;
            village.queues.queueItemAdded += queues_queueItemAdded;
            village.queues.queueItemFinished += queues_queueItemFinished;

            allChanged();
        }

        void queues_queueItemAdded(Village village, Queues.QueueItem item)
        {
            //if(item is BuildingQueueItem
            //|| item is ResearchQueueItem)
            NotifyPropertyChanged("CanBuild");
            NotifyPropertyChanged("Price");
        }

        void queues_queueItemFinished(Village village, Queues.QueueItem item)
        {
            NotifyPropertyChanged("CanBuild");
            NotifyPropertyChanged("Price");
            NotifyPropertyChanged("HaveTechnology");
        }

        public void removeHandlers()
        {
            if(this.data != null)
                this.data.PropertyChanged -= data_PropertyChanged;
            if (this.village != null)
            {
                village.queues.queueItemAdded -= queues_queueItemAdded;
                village.queues.queueItemFinished -= queues_queueItemFinished;
            }
        }

        void GameTime_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "now")
            {
                NotifyPropertyChanged("CanBuild");
                NotifyPropertyChanged("showResources");
            }
        }
        

        private void data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            if (propertyName == "level")
            {
                NotifyPropertyChanged("Level");
                NotifyPropertyChanged("Price");
                NotifyPropertyChanged("CanBuild");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void allChanged()
        {
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("Price");
            NotifyPropertyChanged("Level");
            NotifyPropertyChanged("CanBuild");
            NotifyPropertyChanged("HaveTechnology");
        }
    }
}
