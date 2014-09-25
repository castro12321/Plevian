using Plevian;
using Plevian.Buildings;
using Plevian.Debugging;
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

namespace Plevian.GUI
{
    public partial class UpgradeTechnology : UserControl
    {
        ViewModel model = new ViewModel();
        public UpgradeTechnology()
        {
            InitializeComponent();
            this.DataContextChanged += UpgradeControl_DataContextChanged;
        }

        void UpgradeControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Technology newValue = e.NewValue as Technology;
            if(newValue != null)
            {
                Village currentVillage = MainWindow.getInstance().villageTab.Village;
                setData(newValue, currentVillage);
            }
        }

        public void setData(Technology data, Village village)
        {
            this.DataContext = model;
            stackPanel.DataContext = model;
            model.setData(data, village);
        }

        public Technology getData()
        {
            return model.data;
        }

        public delegate void upgradeEventHandler(Object sender, Technology technology);
        public event upgradeEventHandler Upgrade;

        private void OnUpgradeClick(object sender, RoutedEventArgs e)
        {
            if (Upgrade != null)
                Upgrade(sender, getData());
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public Technology data = null;
        public Village village = null;

        public ViewModel()
        {
            GameTime.PropertyChanged += GameTime_PropertyChanged;
        }

        public String Name
        {
            get
            {
                return data.Name;
            }
        }

        public Resources Price
        {
            get
            {
                if (showResources)
                    return new Resources();
                return data.Price;
            }
        }

        public bool CanBuild
        {
            get
            {
                return village.canResearch(data);
            }
        }

        public bool showResources
        {
            get
            {
                return village.Owner.technologies.isDiscovered(data);
            }
        }

        public bool HaveTechnology
        {
            get
            {
                return data.Requirements.isFullfilled(village);
            }
        }

        public ViewModel(Technology data, Village village)
        {
            setData(data, village);
        }


        public void setData(Technology data, Village village)
        {
            this.data = data;
            this.village = village;

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
            if (this.village != null)
            {
                village.queues.queueItemAdded -= queues_queueItemAdded;
                village.queues.queueItemFinished -= queues_queueItemFinished;
            }
        }

        void GameTime_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "now")
            {
                NotifyPropertyChanged("CanBuild");
                NotifyPropertyChanged("showResources");
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
            NotifyPropertyChanged("CanBuild");
            NotifyPropertyChanged("HaveTechnology");
        }
    }
}